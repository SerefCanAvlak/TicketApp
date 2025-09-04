namespace Ticketing.Application.Features.Log.Dtos;

public class LogDto
{
    public long Id { get; set; }
    public DateTime LogDate { get; set; }
    public string Thread { get; set; } = string.Empty;
    public string LogLevel { get; set; } = string.Empty;
    public string Logger { get; set; } = string.Empty;
    public string LogMessage { get; set; } = string.Empty;
    public string? Exception { get; set; }
}
