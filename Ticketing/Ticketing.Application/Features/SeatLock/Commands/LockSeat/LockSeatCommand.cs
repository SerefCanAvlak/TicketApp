using MediatR;

namespace Ticketing.Application.Features.SeatLock.Commands.LockSeat;

public sealed record LockSeatCommand(
    Guid EventId, 
    Guid SeatId, 
    string LockCode) : IRequest<bool>;
