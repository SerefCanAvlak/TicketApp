using MediatR;

namespace Ticketing.Application.Features.Notification.Events;

public record TicketCreatedEvent(
    Guid TicketId, 
    Guid EventId, 
    Guid UserId,
    string OwnerName,
    Guid SeatId, 
    string OwnerEmail
    ) : INotification;
