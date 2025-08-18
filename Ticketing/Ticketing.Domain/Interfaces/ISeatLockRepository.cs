using GenericRepository;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.Interfaces;

public interface ISeatLockRepository : IRepository<SeatLock>
{
    Task<bool> ExistsAsync(Guid eventId, Guid seatId);
    Task DeleteOldLocksAsync();
    Task<SeatLock?> TryAddLockAsync(Guid eventId, Guid seatId, DateTime validUntil, string lockCode);
}
