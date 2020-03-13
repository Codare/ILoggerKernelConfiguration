using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace Kernel.CrossCuttingConcerns
{
    public class LoggingTelemetryConverter : TraceTelemetryConverter
    {
        private readonly string _roleName;
        private readonly string _roleInstance;

        public LoggingTelemetryConverter(string roleName, string roleInstance)
        {
            _roleInstance = roleInstance;
            _roleName = roleName;
        }

        public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
        {
            foreach (var telemetry in base.Convert(logEvent, formatProvider))
            {
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

                ISupportProperties propTelematry = (ISupportProperties)telemetry;

                var removeProps = new[] { "UserId", "operation_parentId", "operation_Id" };
                removeProps = removeProps.Where(prop => propTelematry.Properties.ContainsKey(prop)).ToArray();

                foreach (var prop in removeProps)
                {
                    propTelematry.Properties.Remove(prop);
                }

                yield return telemetry;
            }
        }

        public override void ForwardPropertiesToTelemetryProperties(
            LogEvent logEvent,
            ISupportProperties telemetryProperties,
            IFormatProvider formatProvider)
        {
            this.ForwardPropertiesToTelemetryProperties(logEvent, telemetryProperties, formatProvider, true, true, true);
        }
    }
}
