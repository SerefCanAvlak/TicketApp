using MediatR;

namespace Ticketing.Application.Features.Events.Commands.FinishExpiredEvents;

public sealed record FinishExpiredEventsCommand() : IRequest<int>;
