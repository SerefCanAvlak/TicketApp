using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Enum;

namespace Ticketing.Domain.Entities;

public sealed class Ticket : Entity
{
    public Guid EventId { get; set; }
    public Guid SeatId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public int StateId { get; set; }
    public TicketStateEnum State
    {
        get => TicketStateEnum.FromValue(StateId);
        set => StateId = value.Value;
    }
}
