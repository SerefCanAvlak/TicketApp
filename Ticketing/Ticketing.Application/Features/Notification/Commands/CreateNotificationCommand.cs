using MediatR;

namespace Ticketing.Application.Features.Notification.Commands;

public record CreateNotificationCommand(
    string To,
    string Subject,
    string Body) : IRequest<Guid>;

