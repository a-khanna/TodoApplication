using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using NLog;

namespace Todo.Core.Logging
{
    /// <summary>
    /// ILogger wrapper for NLog
    /// </summary>
    /// <typeparam name="T">Type of the calling class</typeparam>
    public class NLogLogger<T> : ILogger<T>
    {
        private readonly Logger logger;

        public NLogLogger()
        {
            logger = LogManager.GetLogger(typeof(T).FullName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            var nLogLevel = LogHelper.ConvertLogLevel(logLevel);
            return logger.IsEnabled(nLogLevel);
        }

        /// <summary>
        /// Log method implementation for ILogger.
        /// Uses NLog internally for logging.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var nLogLevel = LogHelper.ConvertLogLevel(logLevel);
            if (!logger.IsEnabled(nLogLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            LogEventInfo eventInfo;

            if (exception == null)
            {
                var message = formatter(state, null);
                eventInfo = LogEventInfo.Create(nLogLevel, logger.Name, message);
            }
            else
            {
                var message = formatter(state, exception);
                eventInfo = LogEventInfo.Create(nLogLevel, logger.Name, exception, null, message);
                eventInfo.Exception = exception;
            }

            logger.Log(typeof(Microsoft.Extensions.Logging.ILogger), eventInfo);
        }
    }
}
