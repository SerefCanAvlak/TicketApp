using MediatR;
using Ticketing.Application.Features.Events.Dtos;


namespace Ticketing.Application.Features.Events.Commands.UpdateEvent;

public sealed record UpdateEventCommand(
    Guid Id,
    string Name,
    DateTime EventStart,
    Guid PhysicalSeatLayoutId,
    string State) : IRequest<EventDto>;

