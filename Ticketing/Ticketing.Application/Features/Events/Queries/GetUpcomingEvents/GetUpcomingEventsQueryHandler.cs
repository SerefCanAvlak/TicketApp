using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Events.Queries.GetUpcomingEvents;

internal sealed class GetUpcomingEventsQueryHandler(
    IEventRepository eventRepository,
    IMapper mapper) : IRequestHandler<GetUpcomingEventsQuery, List<EventDto>>
{
    public async Task<List<EventDto>> Handle(GetUpcomingEventsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var upcomingEvents = await eventRepository
            .GetAll()
            .Where(e => e.StateId == EventStateEnum.Available.Value && e.EventStart > now)
            .OrderBy(e => e.EventStart)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<EventDto>>(upcomingEvents);
    }
}