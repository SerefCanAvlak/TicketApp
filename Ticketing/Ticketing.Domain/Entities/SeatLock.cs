using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Entities;

public sealed class SeatLock : Entity
{
    public DateOnly CreationTime { get; set; }
    public string ValidUntil { get; set; } = string.Empty;
    public string LockCode { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public Guid SeatId { get; set; }
}