using GenericRepository;
using MediatR;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.SeatLock.Commands.LockSeat;

internal sealed class LockSeatCommandHandler(
    ISeatLockRepository seatLockRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<LockSeatCommand, bool>
{
    public async Task<bool> Handle(LockSeatCommand request, CancellationToken cancellationToken)
    {
        
        await seatLockRepository.DeleteOldLocksAsync();

        
        var exists = await seatLockRepository.ExistsAsync(request.EventId, request.SeatId);
        if (exists)
            return false; 

        
        var newseatLock = new Ticketing.Domain.Entities.SeatLock
        {
            EventId = request.EventId,
            SeatId = request.SeatId,
            LockCode = request.LockCode,
            CreationTime = DateTime.UtcNow,
            ValidUntil = DateTime.UtcNow.AddMinutes(1)

        };

        await seatLockRepository.AddAsync(newseatLock, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}