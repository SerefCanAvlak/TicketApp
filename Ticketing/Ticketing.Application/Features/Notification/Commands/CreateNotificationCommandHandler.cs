using MediatR;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Notification.Commands;

public class CreateNotificationCommandHandler(
    INotificationRepository notificationRepository) : IRequestHandler<CreateNotificationCommand, Guid>
{
    public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Ticketing.Domain.Entities.Notification   
        {
            Id = Guid.NewGuid(),
            To = request.To,
            Subject = request.Subject,
            Body = request.Body,
            Status = Ticketing.Domain.Enum.NotificationStatusEnum.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await notificationRepository.AddAsync(notification, cancellationToken);

        return notification.Id;
    }
}
