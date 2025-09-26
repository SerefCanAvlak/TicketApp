using Ardalis.SmartEnum;

namespace Ticketing.Domain.Enum;

public sealed class NotificationStatusEnum : SmartEnum<NotificationStatusEnum>
{
    public static readonly NotificationStatusEnum Pending = new ("Pending", 1);
    public static readonly NotificationStatusEnum Sent = new ("Sent", 2);
    public static readonly NotificationStatusEnum Failed = new ("Failed", 3);
    public NotificationStatusEnum(string name, int value) : base(name, value)
    {
    }
}
