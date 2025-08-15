namespace Ticketing.Application.Features.SeatLock.Dtos;

public class SeatLockDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ValidUntil { get; set; }
    public string LockCode { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public Guid SeatId { get; set; }
}
