using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Entities;

public sealed class Seat: Entity
{
    public string Row { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public Guid PhysicalSeatLayoutId { get; set; }
}

