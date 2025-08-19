using MediatR;
using Ticketing.Application.Features.Events.Dtos;


namespace Ticketing.Application.Features.Events.Queries.GetEvents;

public sealed record GetEventsQuery(): IRequest<List<EventDto>>;
