namespace Ticketing.Worker.Configuration;

public class WorkerSettings
{
    public int EventExpirationJobIntervalMinutes { get; set; }
    public int NotificationIntervalSeconds { get; set; } = 86400;
}
