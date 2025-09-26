using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler(
    ITicketRepository ticketRepository,
    ISeatLockRepository seatLockRepository,
    IEventRepository eventRepository,
    IRepository<Notification> notificationRepository,
    ILogService logService,
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IMapper mapper) : IRequestHandler<CreateTicketCommand, TicketDto?>
{
    public async Task<TicketDto?> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        await seatLockRepository.DeleteOldLocksAsync();

        var newSeatLock = new Ticketing.Domain.Entities.SeatLock
        {
            EventId = request.EventId,
            SeatId = request.SeatId,
            LockCode = Guid.NewGuid().ToString(),
            CreationTime = DateTime.UtcNow,
            ValidUntil = DateTime.UtcNow.AddMinutes(1)
        };

        var seatLock = await seatLockRepository.TryAddLockAsync(
            request.EventId,
            request.SeatId,
            DateTime.UtcNow.AddMinutes(1),
            Guid.NewGuid().ToString()
);

        if (seatLock is null)
            return null;

        var exists = await ticketRepository
            .GetAll()
            .AnyAsync(t =>
                t.EventId == request.EventId &&
                t.SeatId == request.SeatId &&
                t.StateId == TicketStateEnum.Valid.Value,
                cancellationToken);

        if (exists)
        {
            seatLockRepository.Delete(seatLock);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return null;
        }

        var ticket = new Ticketing.Domain.Entities.Ticket
        {
            EventId = request.EventId,
            SeatId = request.SeatId,
            OwnerName = request.OwnerName,
            CreatedAt = DateTimeOffset.UtcNow,
            State = TicketStateEnum.Valid
        };
        await ticketRepository.AddAsync(ticket, cancellationToken);

         seatLockRepository.Delete(seatLock);



        logService.Info($"Bilet oluşturuldu. TicketId: {ticket.Id}, EventId: {ticket.EventId}, SeatId: {ticket.SeatId}, OwnerName: {ticket.OwnerName}");
        // Mail Send
        var eventEntity = await eventRepository.GetByExpressionWithTrackingAsync(e => e.Id == request.EventId, cancellationToken) ?? throw new Exception("Event bulunamadı");

        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        var email = user?.Email;

        if (!string.IsNullOrWhiteSpace(email))
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                To = email,
                Subject = "Biletiniz oluşturuldu",
                Body = $"Merhaba {request.OwnerName}, {eventEntity?.Name} etkinliği için biletiniz oluşturuldu.",
                Status = NotificationStatusEnum.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(notification, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<TicketDto>(ticket);
    }
}