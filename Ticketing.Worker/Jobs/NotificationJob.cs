using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Services;
using Ticketing.Domain.Enum;
using Ticketing.Infrastructure.Context;

namespace Ticketing.Worker.Jobs;

public sealed class NotificationJob : IWorkerJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationJob> _logger;

    public NotificationJob(IServiceProvider serviceProvider, ILogger<NotificationJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public string Name => "NotificationJob";

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Worker] NotificationJob started.");

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

        var pendingNotifications = await db.Notifications
             .Where(n => n.StatusId == NotificationStatusEnum.Pending.Value)
             .ToListAsync(cancellationToken);

        foreach (var notification in pendingNotifications)
        {
            try
            {
                await mailService.SendEmailAsync(notification, cancellationToken);
                notification.Status = NotificationStatusEnum.Sent;
                notification.SentAt = DateTime.UtcNow;
                await db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("[Worker] Mail sent: {To}", notification.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Worker] Failed to send mail: {Id}", notification.Id);
                notification.Status = NotificationStatusEnum.Failed;
                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
