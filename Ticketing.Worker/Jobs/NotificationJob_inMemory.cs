using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Channels;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Context;
using Ticketing.Worker.Configuration;

namespace Ticketing.Worker.Jobs;

public sealed class NotificationJob_inMemory : IWorkerJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkerSettings _settings;
    private readonly Channel<Notification> _queue;
    private readonly ILogger<NotificationJob_inMemory> _logger;

    public NotificationJob_inMemory(IServiceProvider serviceProvider, IOptions<WorkerSettings> settings, ILogger<NotificationJob_inMemory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _queue = Channel.CreateUnbounded<Notification>();
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public string Name => "NotificationJob";

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[Worker] NotificationJob started.");

        // DB polling loop
        _ = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var notifications = await db.Notifications
                        .Where(n => n.StatusId == Domain.Enum.NotificationStatusEnum.Pending.Value)
                        .ToListAsync(stoppingToken);

                    foreach (var n in notifications)
                    {
                        await _queue.Writer.WriteAsync(n, stoppingToken);
                        _logger.LogInformation($"Queued notification Id: {n.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while polling notifications.");
                }

                await Task.Delay(TimeSpan.FromSeconds(_settings.NotificationIntervalSeconds), stoppingToken);
            }
        }, stoppingToken);

        // Queue processing loop
        while (await _queue.Reader.WaitToReadAsync(stoppingToken))
        {
            while (_queue.Reader.TryRead(out var notification))
            {
                var success = false;

                for (int attempt = 1; attempt <= 3 && !success; attempt++)
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                        await mailService.SendEmailAsync(notification, stoppingToken);

                        notification.Status = Domain.Enum.NotificationStatusEnum.Sent;
                        notification.SentAt = DateTime.UtcNow;

                        db.Notifications.Update(notification);
                        await db.SaveChangesAsync(stoppingToken);

                        _logger.LogInformation($"Notification Id {notification.Id} sent successfully.");
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Attempt {attempt} failed for notification Id {notification.Id}.");
                        await Task.Delay(2000, stoppingToken);
                    }
                }

                //  başarısızsa DB’ye Failed yaz
                if (!success)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    notification.Status = Domain.Enum.NotificationStatusEnum.Failed;
                    db.Notifications.Update(notification);
                    await db.SaveChangesAsync(stoppingToken);

                    _logger.LogWarning($"Notification Id {notification.Id} failed after 3 attempts.");
                }
            }
        }
    }
}
