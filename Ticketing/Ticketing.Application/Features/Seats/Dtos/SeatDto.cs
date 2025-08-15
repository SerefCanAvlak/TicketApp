namespace Ticketing.Application.Features.Seats.Dtos;

public class SeatDto
{
    public Guid Id { get; set; }
    public string Row { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public Guid PhysicalSeatLayoutId { get; set; }
}
