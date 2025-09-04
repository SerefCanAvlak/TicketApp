namespace Ticketing.Application.Services;
        public interface ILogService
        {
            void Info(string message);
            void Debug(string message);
            void Warn(string message);
            void Error(string message, Exception? ex = null);
        }
