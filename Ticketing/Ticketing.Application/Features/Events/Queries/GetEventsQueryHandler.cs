using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Domain.Interfaces;


namespace Ticketing.Application.Features.Events.Queries;

internal sealed class GetEventsQueryHandler(
    IEventRepository eventRepository,
    IMapper mapper) : IRequestHandler<GetEventsQuery, List<EventDto>>
{
    public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await eventRepository.GetAll().ToListAsync(cancellationToken);

        var eventDtos = events.Select(e => mapper.Map<EventDto>(e)).ToList();

        return eventDtos;
    }
}