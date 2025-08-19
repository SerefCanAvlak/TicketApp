using MediatR;
using Ticketing.Application.Features.Events.Dtos;

namespace Ticketing.Application.Features.Events.Queries.GetUpcomingEvents;

public sealed record GetUpcomingEventsQuery() : IRequest<List<EventDto>> ;
