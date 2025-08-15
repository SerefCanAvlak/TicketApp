using MediatR;
using Ticketing.Application.Features.Events.Dtos;

namespace Ticketing.Application.Features.Events.Commands.CreateEvent;

public sealed record CreateEventCommand(CreateEventDto Event) : IRequest<EventDto>;
