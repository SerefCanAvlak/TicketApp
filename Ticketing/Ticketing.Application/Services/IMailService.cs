using Ticketing.Domain.Entities;

namespace Ticketing.Application.Services;

public interface IMailService
{
    Task SendEmailAsync(Notification notification, CancellationToken cancellationToken);
}
