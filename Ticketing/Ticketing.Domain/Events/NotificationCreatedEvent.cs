using MediatR;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.Events;

public class NotificationCreatedEvent : INotification
{
    public Notification Notification { get; }

    public NotificationCreatedEvent(Notification notification)
    {
        Notification = notification;
    }
}
