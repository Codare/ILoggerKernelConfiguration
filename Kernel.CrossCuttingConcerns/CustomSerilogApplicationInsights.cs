using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Kernel.CrossCuttingConcerns
{
    public static class CustomSerilogApplicationInsights
    {
        public static LoggerConfiguration DaftPunk(this LoggerSinkConfiguration sinkConfiguration, string roleName, string roleInstance, LogEventLevel restrictedToMinimumLevel)
        {
            return sinkConfiguration.ApplicationInsights(telemetryConverter: new LoggingTelemetryConverter(roleName, roleInstance),
                restrictedToMinimumLevel: restrictedToMinimumLevel);
        }
    }
}
