using GenericRepository;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.Interfaces;

public interface ISeatLockRepository : IRepository<SeatLock>
{
    Task<bool> ExistsAsync(Guid eventId, Guid seatId);
    Task DeleteOldLocksAsync();
}
