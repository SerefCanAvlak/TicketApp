using Ardalis.SmartEnum;

namespace Ticketing.Domain.Enum;

public sealed class TicketStateEnum : SmartEnum<TicketStateEnum>
{
    public static readonly TicketStateEnum Valid = new("Valid", 1);
    public static readonly TicketStateEnum Cancelled = new("Cancelled", 2);
  
    public TicketStateEnum(string name, int value) : base(name, value)
    {
    }
}