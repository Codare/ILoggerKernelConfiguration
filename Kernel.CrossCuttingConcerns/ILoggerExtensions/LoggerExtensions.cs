using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Internal;

namespace Kernel.CrossCuttingConcerns.ILoggerExtensions
{
    public static class LoggerMessageHelper
    {
        public static Dictionary<string, object> GetLogMessageState(bool triggerAlert, string message, params object[] args)
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
            messageState.Add("TriggerAlert", triggerAlert ? bool.TrueString.ToLower() : bool.FalseString.ToLower());

            return messageState;
        }
    }
}
