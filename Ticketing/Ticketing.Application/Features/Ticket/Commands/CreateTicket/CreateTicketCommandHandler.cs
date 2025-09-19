using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Application.Services;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler(
    ITicketRepository ticketRepository,
    ISeatLockRepository seatLockRepository,
    ILogService logService,
    IUnitOfWork unitOfWork,
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

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logService.Info($"Bilet oluşturuldu. TicketId: {ticket.Id}, EventId: {ticket.EventId}, SeatId: {ticket.SeatId}, OwnerName: {ticket.OwnerName}");

        return mapper.Map<TicketDto>(ticket);
    }
}
