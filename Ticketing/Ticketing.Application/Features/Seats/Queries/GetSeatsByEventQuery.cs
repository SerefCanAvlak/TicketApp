using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Seats.Dtos;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Seats.Queries;

public sealed record GetSeatsByEventQuery(Guid EventId): IRequest<List<SeatDto>>;


internal sealed class GetSeatsByEventQueryHandler(
    ISeatRepository seatRepository,
    IEventRepository eventRepository,
    IMapper mapper) : IRequestHandler<GetSeatsByEventQuery, List<SeatDto>>
{
    public async Task<List<SeatDto>> Handle(GetSeatsByEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        if (@event == null)
            return new List<SeatDto>();

        var seats = await seatRepository.GetAll()
            .Where(s => s.PhysicalSeatLayoutId == @event.PhysicalSeatLayoutId)
            .ToListAsync(cancellationToken);

        var seatDtos = seats.Select(s => mapper.Map<SeatDto>(s)).ToList();

        return seatDtos;
    }
}