﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Kernel.CrossCuttingConcerns.ILoggerExtensions
{
    public static class LoggerExtensionsTrace
    {
        /// <summary>
        /// Writes a trace log message.
        ///Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User logged in from"</code></param>
        /// <example>logger.LogTrace("Error while processing request", true|false)</example>
        public static void LogTrace(this ILogger logger, string message, bool triggerAlert)
        {
            LogTraceImpl(logger, message, triggerAlert);
        }

        /// <summary>
        /// Writes a trace log message.
        ///Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User logged in from"</code></param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace("Error while processing request from {Address}", true|false, address)</example>
        public static void LogTrace(this ILogger logger, string message, bool triggerAlert, params object[] args)
        {
            LogTraceImpl(logger, message, triggerAlert, null, args);
        }

        /// <summary>
        /// Writes a trace log message.
        ///Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User logged in from"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", true|false, address)</example>
        public static void LogTrace(this ILogger logger, Exception exception, string message, params object[] args)
        {
            LogTraceImpl(logger, message, false, exception, args);
        }

        /// <summary>
        /// Writes a trace log message.
        ///Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User logged in from"</code></param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <example>logger.LogTrace(exception, "Error while processing request", true|false)</example>
        public static void LogTrace(this ILogger logger, Exception exception, string message, bool triggerAlert)
        {
            LogTraceImpl(logger, message, triggerAlert, exception);
        }

        /// <summary>
        /// Writes a trace log message.
        ///Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", true|false, address)</example>
        public static void LogTrace(this ILogger logger, Exception exception, string message, bool triggerAlert, params object[] args)
        {
            LogTraceImpl(logger, message, triggerAlert, exception, args);
        }

        /// <summary>
        /// Writes a trace log message.
        ///Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogTrace("Error while processing request from {Address}", true|false, exception, address)</example>
        private static void LogTraceImpl(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
        {
            Dictionary<string, object> state = LoggerMessageHelper.GetLogMessageState(triggerAlert, message, args);

            var expandoObject = new ExpandoObject();
            var keyValuePairs = (ICollection<KeyValuePair<string, object>>)expandoObject;

            foreach (var keyValuePair in state)
                keyValuePairs.Add(keyValuePair);

            dynamic eoDynamic = expandoObject;

            logger.Log(logLevel: LogLevel.Trace, eventId: new EventId(9999), state: eoDynamic, exception: exception, formatter: null);
        }
    }
}
