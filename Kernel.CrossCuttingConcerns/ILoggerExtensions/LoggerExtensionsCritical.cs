using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Internal;

namespace Kernel.CrossCuttingConcerns.ILoggerExtensions
{
    public static class LoggerExtensionsCritical
    {
        /// <summary>Writes an critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User logged in from"</code></param>
        /// <example>logger.LogCritical(0, exception, "Error while processing request", true|false)</example>
        public static void LogCritical(this ILogger logger, string message, bool triggerAlert)
        {
            LogCriticalImpl(logger, message, triggerAlert);
        }

        public static void LogCritical(this ILogger logger, string message, bool triggerAlert, params  object[] args)
        {
            LogCriticalImpl(logger, message, triggerAlert, null, args);
        }

        /// <summary>Writes an critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User logged in from"</code></param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <example>logger.LogCritical(0, exception, "Error while processing request", true|false)</example>
        public static void LogCritical(this ILogger logger, Exception exception, string message, bool triggerAlert)
        {
            LogCriticalImpl(logger, message, triggerAlert, exception);
        }

        /// <summary>Formats and writes an critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address, true|false)</example>
        public static void LogCritical(this ILogger logger, Exception exception, string message, bool triggerAlert, params object[] args)
        {
            LogCriticalImpl(logger, message, triggerAlert, exception, args);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
        private static void LogCriticalImpl(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
        {
            Dictionary<string, object> state = GetLogMessageState(triggerAlert, message, args);

            var expandoObject = new ExpandoObject();
            var keyValuePairs = (ICollection<KeyValuePair<string, object>>)expandoObject;

            foreach (var keyValuePair in state)
                keyValuePairs.Add(keyValuePair);

            dynamic eoDynamic = expandoObject;

            logger.Log(logLevel: LogLevel.Critical, eventId: new EventId(9999), state: eoDynamic, exception: exception, formatter: null);
        }

        private static Dictionary<string, object> GetLogMessageState(bool triggerAlert, string message, params object[] args)
        {
            Dictionary<string, object> messageState = new Dictionary<string, object>();

            var aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (aspnetcoreEnvironment == EnvironmentName.Production || aspnetcoreEnvironment == EnvironmentName.Staging || aspnetcoreEnvironment == EnvironmentName.Development)
            {
                for (var index = 0; index < args.Length; index++)
                {
                    object arg = args[index];
                    if (arg is string)
                    {
                        args[index] = SanitizeLogOutput.RedactSensitiveInfo(arg.ToString());
                    }
                }
            }

            string formattedLogValues = new FormattedLogValues(message, args).ToString();

            messageState.Add("Message", formattedLogValues);
            messageState.Add("TriggerAlert", triggerAlert ? bool.TrueString : bool.FalseString);

            return messageState;
        }
    }
}
