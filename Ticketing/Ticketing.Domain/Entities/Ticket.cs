using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Enum;

namespace Ticketing.Domain.Entities;

public sealed class Ticket : Entity
{
    public string OwnerName { get; set; } = string.Empty;
    public DateOnly CreatedAt { get; set; }
    public TicketStateEnum State { get; set; } = TicketStateEnum.Valid;
    public Guid EventId { get; set; }
    public Guid SeatId { get; set; }
}
