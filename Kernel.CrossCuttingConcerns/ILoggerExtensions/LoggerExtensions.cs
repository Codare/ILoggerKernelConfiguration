﻿//using System;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Internal;

//namespace Kernel.CrossCuttingConcerns.ILoggerExtensions
//{
//    public static class LoggerExtensions
//    {
//        /// <summary>Formats and writes an informational log message.</summary>
//        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
//        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
//        /// <param name="exception">The exception to log.</param>
//        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
//        /// <param name="args">An object array that contains zero or more objects to format.</param>
//        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
//        public static void LogTrace(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
//        {
//            logger.Log(logLevel: LogLevel.Trace, eventId: new EventId(1, "TriggerAlert"), state: GetTriggerAlert(triggerAlert), exception: exception, formatter: null);
//        }

//        /// <summary>Formats and writes an informational log message.</summary>
//        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
//        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
//        /// <param name="exception">The exception to log.</param>
//        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
//        /// <param name="args">An object array that contains zero or more objects to format.</param>
//        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
//        public static void LogDebug(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
//        {
//            logger.Log(logLevel: LogLevel.Debug, eventId: new EventId(1, "TriggerAlert"), state: GetTriggerAlert(triggerAlert), exception: exception, formatter: null);
//        }

//        /// <summary>Formats and writes an informational log message.</summary>
//        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
//        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
//        /// <param name="exception">The exception to log.</param>
//        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
//        /// <param name="args">An object array that contains zero or more objects to format.</param>
//        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
//        public static void LogInformation(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
//        {
//            logger.Log(logLevel: LogLevel.Information, eventId: new EventId(1, "TriggerAlert"), state: GetTriggerAlert(triggerAlert), exception: exception, formatter: null);
//        }

//        /// <summary>Formats and writes an informational log message.</summary>
//        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
//        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
//        /// <param name="exception">The exception to log.</param>
//        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
//        /// <param name="args">An object array that contains zero or more objects to format.</param>
//        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
//        public static void LogWarning(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
//        {
//            logger.Log(logLevel: LogLevel.Warning, eventId: new EventId(1, "TriggerAlert"), state: GetTriggerAlert(triggerAlert), exception: exception, formatter: null);
//        }

//        /// <summary>Formats and writes an informational log message.</summary>
//        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
//        /// <param name="triggerAlert">Boolean parameter to indicate that an alert is to be triggered.</param>
//        /// <param name="exception">The exception to log.</param>
//        /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
//        /// <param name="args">An object array that contains zero or more objects to format.</param>
//        /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
//        public static void LogError(this ILogger logger, string message, bool triggerAlert, Exception exception = null, params object[] args)
//        {
//            logger.Log(logLevel: LogLevel.Error, eventId: new EventId(1, "TriggerAlert"), state: GetTriggerAlert(triggerAlert), exception: exception, formatter: null);
//        }
//    }
//}
