using MediatR;
using Ticketing.Application.Features.Events.Dtos;


namespace Ticketing.Application.Features.Events.Queries;

public sealed record GetEventsQuery(): IRequest<List<EventDto>>;
