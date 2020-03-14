using System;
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.ExtensionMethods;

namespace Kernel.CrossCuttingConcerns
{
    public static class LoggerSinkConfigurationExtensions
    {
        private static string _roleName;
        private static string _roleInstance;

        public static LoggerConfiguration ApplicationInsightsExtension(this LoggerSinkConfiguration sinkConfiguration, string roleName, string roleInstance, LogEventLevel restrictedToMinimumLevel)
        {
            _roleName = roleName;
            _roleInstance = roleInstance;

            return sinkConfiguration.ApplicationInsights(TelemetryConfiguration.Active, ConvertLogEventsToCustomTraceTelemetry, restrictedToMinimumLevel);
        }

        private static ITelemetry ConvertLogEventsToCustomTraceTelemetry(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var telemetry = GetTelemetry(logEvent, formatProvider);

            telemetry.Context.Cloud.RoleInstance = _roleInstance;
            telemetry.Context.Cloud.RoleName = _roleName;

            if (logEvent.Properties.ContainsKey("UserId"))
            {
                telemetry.Context.User.Id = logEvent.Properties["UserId"].ToString();
            }

            if (logEvent.Properties.ContainsKey("operation_Id"))
            {
                telemetry.Context.Operation.Id = logEvent.Properties["operation_Id"].ToString();
            }

            if (logEvent.Properties.ContainsKey("operation_parentId"))
            {
                telemetry.Context.Operation.ParentId = logEvent.Properties["operation_parentId"].ToString();
            }

            ISupportProperties supportProperties = (ISupportProperties)telemetry;

            var removeProps = new[] { "UserId", "operation_parentId", "operation_Id" };
            removeProps = removeProps.Where(prop => supportProperties.Properties.ContainsKey(prop)).ToArray();

            foreach (var prop in removeProps)
            {
                supportProperties.Properties.Remove(prop);
            }

            return telemetry;
        }

        private static ITelemetry GetTelemetry(LogEvent logEvent, IFormatProvider formatProvider)
        {
            if (logEvent.Exception != null)
            {
                // Exception telemetry
                return logEvent.ToDefaultExceptionTelemetry(
                    formatProvider,
                    includeLogLevelAsProperty: false,
                    includeRenderedMessageAsProperty: false,
                    includeMessageTemplateAsProperty: false);
            }
            else
            {
                // default telemetry
                return logEvent.ToDefaultTraceTelemetry(
                    formatProvider,
                    includeLogLevelAsProperty: false,
                    includeRenderedMessageAsProperty: false,
                    includeMessageTemplateAsProperty: false);
            }
        }
    }
}
