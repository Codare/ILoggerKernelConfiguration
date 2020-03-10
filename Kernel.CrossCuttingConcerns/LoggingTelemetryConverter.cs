using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Channel;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace Kernel.CrossCuttingConcerns
{
    public class LoggingTelemetryConverter : EventTelemetryConverter//, ITelemetryConverter
    {
        private string _roleName;
        private string _roleInstance;

        const string OperationId = "Operation Id";
        const string ParentId = "Parent Id";
        private const string RoleName = "Role Name";
        private const string RoleInstance = "Role Instance";


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
