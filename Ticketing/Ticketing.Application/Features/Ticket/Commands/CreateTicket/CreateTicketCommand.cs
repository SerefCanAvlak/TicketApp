using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Commands.CreateTicket;

public sealed record CreateTicketCommand(
    Guid EventId,
    Guid SeatId,
    string OwnerName) : IRequest<TicketDto?>;

internal sealed class CreateTicketCommandHandler(
    ITicketRepository ticketRepository,
    ISeatLockRepository seatLockRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateTicketCommand, TicketDto?>
{
    public async Task<TicketDto?> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        // Eski kilitleri temizle
        await seatLockRepository.DeleteOldLocksAsync();

        // Yeni kilidi oluştur
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


        // Seat zaten alınmış mı kontrol et
        var exists = await ticketRepository
            .GetAll()
            .AnyAsync(t =>
                t.EventId == request.EventId &&
                t.SeatId == request.SeatId &&
                t.StateId == TicketStateEnum.Valid.Value,
                cancellationToken);

        if (exists)
        {
            seatLockRepository.Delete(newSeatLock);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return null;
        }

        // Ticket oluştur
        var ticket = new Ticketing.Domain.Entities.Ticket
        {
            EventId = request.EventId,
            SeatId = request.SeatId,
            OwnerName = request.OwnerName,
            CreatedAt = DateTimeOffset.UtcNow,
            State = TicketStateEnum.Valid
        };
        await ticketRepository.AddAsync(ticket, cancellationToken);

        // SeatLock'u sil
         seatLockRepository.Delete(seatLock);


        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<TicketDto>(ticket);
    }
}
