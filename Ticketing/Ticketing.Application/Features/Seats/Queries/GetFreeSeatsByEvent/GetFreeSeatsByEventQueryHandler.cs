using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Seats.Dtos;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Seats.Queries.GetFreeSeatsByEvent;

internal sealed class GetFreeSeatsByEventQueryHandler(
    ISeatRepository seatRepository,
    IEventRepository eventRepository,
    ISeatLockRepository seatLockRepository,
    ITicketRepository ticketRepository,
    IMapper mapper) : IRequestHandler<GetFreeSeatsByEventQuery, List<SeatDto>>
{
    public async Task<List<SeatDto>> Handle(GetFreeSeatsByEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        if (@event == null)
            return new List<SeatDto>();

        var now = DateTime.UtcNow;

        var freeSeats = await seatRepository.GetAll()
            .Where(s => s.PhysicalSeatLayoutId == @event.PhysicalSeatLayoutId)
            .Where(s => !seatLockRepository.GetAll()
                .Any(l => l.SeatId == s.Id && l.ValidUntil > now))
            .Where(s => !ticketRepository.GetAll()
                .Any(t => t.SeatId == s.Id && t.EventId == @event.Id))
            .ToListAsync(cancellationToken);

        return freeSeats.Select(s => mapper.Map<SeatDto>(s)).ToList();
    }
}