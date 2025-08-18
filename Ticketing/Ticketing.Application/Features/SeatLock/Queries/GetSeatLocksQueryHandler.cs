using MediatR;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.SeatLock.Queries;

internal sealed class GetSeatLocksQueryHandler(
    ISeatLockRepository seatLockRepository
) : IRequestHandler<GetSeatLocksQuery, List<Ticketing.Domain.Entities.SeatLock>>
{
    public async Task<List<Ticketing.Domain.Entities.SeatLock>> Handle(GetSeatLocksQuery request, CancellationToken cancellationToken)
    {
        var allLocks = await Task.Run(() => seatLockRepository.GetAll().ToList(), cancellationToken);
        return allLocks.Where(l => l.EventId == request.EventId).ToList();
    }
}
