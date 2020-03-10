using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace Kernel.CrossCuttingConcerns
{
    public class LoggingTelemetryConverter : TraceTelemetryConverter, ITelemetryConverter
    {
        private string _roleName;
        private string _roleInstance;
        private IConfiguration _configuration;

        const string OperationId = "Operation Id";
        const string ParentId = "Parent Id";
        private const string RoleName = "Role Name";
        private const string RoleInstance = "Role Instance";

        public LoggingTelemetryConverter()
        {
        }

        public LoggingTelemetryConverter(IConfiguration configuration)
        {

        }

        public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
        {
            foreach (var telemetry in base.Convert(logEvent, formatProvider))
            {
                telemetry.Context.Cloud.RoleInstance = _configuration["Logging:ApplicationInsights:RoleName"];
                telemetry.Context.Cloud.RoleName = "go away";

                if (TryGetScalarProperty(logEvent, OperationId, out var operationId))
                    telemetry.Context.Operation.Id = operationId.ToString();

                if (TryGetScalarProperty(logEvent, ParentId, out var parentId))
                    telemetry.Context.Operation.ParentId = parentId.ToString();

                yield return telemetry;
            }
        }

        private bool TryGetScalarProperty(LogEvent logEvent, string propertyName, out object value)
        {
            var hasScalarValue =
                logEvent.Properties.TryGetValue(propertyName, out var someValue) &&
                (someValue is ScalarValue);

            value = hasScalarValue ? ((ScalarValue)someValue).Value : default;

            return hasScalarValue;
        }
    }
}
