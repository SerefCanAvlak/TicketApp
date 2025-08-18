using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Interfaces;
using Ticketing.Infrastructure.Context;

namespace Ticketing.Infrastructure.Repositories;

internal sealed class SeatLockRepository : Repository<SeatLock, ApplicationDbContext>, ISeatLockRepository
{
    private readonly ApplicationDbContext _context;

    public SeatLockRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task DeleteOldLocksAsync()
    {
        var now = DateTime.UtcNow;
        var oldLocks = await _context.SeatLocks
            .Where(s => s.ValidUntil < DateTime.UtcNow)
            .ToListAsync();

        if (oldLocks.Any())
        {
            _context.SeatLocks.RemoveRange(oldLocks);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid eventId, Guid seatId)
    {
        return await _context.SeatLocks
            .AnyAsync(sl => sl.EventId == eventId && sl.SeatId == seatId);
    }

    public async Task<SeatLock?> TryAddLockAsync(Guid eventId, Guid seatId, DateTime validUntil, string lockCode)
    {
        var seatLock = new SeatLock
        {
            EventId = eventId,
            SeatId = seatId,
            ValidUntil = validUntil,
            LockCode = lockCode,
            CreationTime = DateTime.UtcNow
        };

        try
        {
            _context.SeatLocks.Add(seatLock);
            await _context.SaveChangesAsync();
            return seatLock; // track edilen entity’yi geri döndür
        }
        catch
        {
            return null;
        }
    }
}
