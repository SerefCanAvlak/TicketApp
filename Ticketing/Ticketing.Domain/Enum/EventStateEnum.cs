using Ardalis.SmartEnum;

namespace Ticketing.Domain.Enum;

public sealed class EventStateEnum : SmartEnum<EventStateEnum>
{
    public static readonly EventStateEnum Available = new ("Available", 1);
    public static readonly EventStateEnum Finished = new ("Finished", 2);
    public static readonly EventStateEnum Cancelled = new ("Cancelled", 3);
    public EventStateEnum(string name, int value) : base(name, value)
    {
    }
}