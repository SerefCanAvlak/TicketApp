namespace Ticketing.Application.Features.Ticket.Dtos;

public class TicketDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid SeatId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public required string State { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
