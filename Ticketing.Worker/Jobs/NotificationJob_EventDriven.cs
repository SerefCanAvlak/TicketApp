
using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Enum;
using Ticketing.Infrastructure.Context;

namespace Ticketing.Worker.Jobs;

public class NotificationJob_EventDriven : IWorkerJob
{
    private readonly ApplicationDbContext _context;
    private readonly IFluentEmail _email;
    private readonly ILogger<NotificationJob_EventDriven> _logger;

    public NotificationJob_EventDriven(ApplicationDbContext context, IFluentEmail email, ILogger<NotificationJob_EventDriven> logger)
    {
        _context = context;
        _email = email;
        _logger = logger;
    }

    public string Name => "NotificationJob_EventDriven";

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(n => n.StatusId == Domain.Enum.NotificationStatusEnum.Pending.Value)
            .ToListAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            try
            {
              var email = _email
                .To(notification.To)
                .Subject(notification.Subject)
                .Body(notification.Body, isHtml: true);

                var response = await email.SendAsync();

                notification.StatusId = response.Successful
                    ? NotificationStatusEnum.Sent.Value
                    : NotificationStatusEnum.Failed.Value;

                notification.SentAt = response.Successful ? DateTime.UtcNow : null;

                _logger.LogInformation($"Mail gönderildi: {notification.Id} to {notification.To}");
            }
            catch (Exception ex)
            {
                notification.StatusId = NotificationStatusEnum.Failed.Value;
                _logger.LogError(ex, $"Mail gönderim hatası: {ex.Message}");
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}