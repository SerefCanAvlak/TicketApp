using log4net;
using Ticketing.Application.Services;

namespace Ticketing.Infrastructure.Services;

public class LogService : ILogService
{
    private readonly ILog _logger;

    public LogService()
    {
        _logger = LogManager.GetLogger(typeof(LogService));
    }

    public void Debug(string message)
    {
        if (_logger.IsDebugEnabled)
            _logger.Debug(message);
    }

    public void Info(string message)
    {
        if (_logger.IsInfoEnabled)
            _logger.Info(message);
    }

    public void Warn(string message)
    {
        if (_logger.IsWarnEnabled)
            _logger.Warn(message);
    }

    public void Error(string message, Exception? ex = null)
    {
        if (ex != null)
            _logger.Error(message, ex);
        else
            _logger.Error(message);
    }
}
