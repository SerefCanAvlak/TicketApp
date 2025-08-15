using MediatR;
using Ticketing.Application.Features.Events.Dtos;

namespace Ticketing.Application.Features.Events.Commands.DeleteEvent;

public sealed record DeleteEventByIdCommand(Guid Id) : IRequest<EventDto>;
