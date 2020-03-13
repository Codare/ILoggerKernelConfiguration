//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Internal;
//using Serilog.Context;

//namespace Kernel.CrossCuttingConcerns.Serilog.SerilogExtensions
//{
//    public static class LoggerExtensions
//    {
//        public static IDisposable MyBeginScope(
//            this ILogger logger,
//            string messageFormat,
//            params object[] args)
//        {
//            if (logger == null)
//                throw new ArgumentNullException(nameof(logger));
//            return logger.BeginScope<FormattedLogValues>(new FormattedLogValues(messageFormat, args));
//        }
//}
