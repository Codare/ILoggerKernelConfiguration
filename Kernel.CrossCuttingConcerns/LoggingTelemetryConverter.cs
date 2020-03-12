using System;
using System.Collections.Generic;
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
