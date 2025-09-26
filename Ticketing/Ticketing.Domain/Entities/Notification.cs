using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Enum;

namespace Ticketing.Domain.Entities;

public class Notification : Entity
{
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public int StatusId { get; set; } = NotificationStatusEnum.Pending.Value;
    public NotificationStatusEnum Status
    {
        get => NotificationStatusEnum.FromValue(StatusId);
        set => StatusId = value.Value;
    }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
}