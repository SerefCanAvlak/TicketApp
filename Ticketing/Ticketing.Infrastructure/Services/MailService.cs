using FluentEmail.Core;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Services;

public sealed class MailService(IFluentEmail fluentEmail) : IMailService
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;
    public async Task SendEmailAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _fluentEmail
            .To(notification.To)
            .Subject(notification.Subject)
            .Body(notification.Body, isHtml: true)
            .SendAsync(cancellationToken);
    }
}