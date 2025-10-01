using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Notification.Events;

public class TicketCreatedEventHandler(
    IUnitOfWork unitOfWork,
    IEventRepository eventRepository,
    INotificationRepository notificationRepository,
    UserManager<AppUser> userManager) : INotificationHandler<TicketCreatedEvent>
{
    public async Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Event bilgisi
        var eventEntity = await eventRepository.GetByExpressionWithTrackingAsync(
            e => e.Id == notification.EventId, cancellationToken
        ) ?? throw new Exception("Event bulunamadı");

        // Kullanıcı bilgisi
        var user = await userManager.FindByIdAsync(notification.UserId.ToString());
        var email = user?.Email ?? notification.OwnerEmail; // fallback

        if (!string.IsNullOrWhiteSpace(email))
        {
            var newNotification = new Ticketing.Domain.Entities.Notification
            {
                Id = Guid.NewGuid(),
                To = email,
                Subject = "Biletiniz oluşturuldu",
                Body = $"Merhaba {notification.OwnerName}, {eventEntity.Name} etkinliği için biletiniz oluşturuldu. Koltuk Numaranız: {notification.SeatId} abi bu event sınıfı haberin olsun",
                Status = NotificationStatusEnum.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(newNotification, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
