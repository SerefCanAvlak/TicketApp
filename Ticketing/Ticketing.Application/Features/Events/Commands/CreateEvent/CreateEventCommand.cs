using MediatR;
using Ticketing.Application.Features.Events.Dtos;

namespace Ticketing.Application.Features.Events.Commands.CreateEvent;

public sealed record CreateEventCommand(  string Name ,
 DateTime EventStart ,
 Guid PhysicalSeatLayoutId ,
 string State ) : IRequest<EventDto>;
