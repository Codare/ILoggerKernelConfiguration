using System;
using Kernel.CrossCuttingConcerns.ClaimsValueEnrichment;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace Kernel.CrossCuttingConcerns
{
    public static class CustomUseSerilog
    {
        public static IWebHostBuilder UseCustomSerilogWebHostBuilder(this IWebHostBuilder webHostBuilder,
            IConfiguration configuration,
            string environment)
        {
            if (webHostBuilder == null)
                throw new ArgumentNullException(nameof(webHostBuilder));

            webHostBuilder.CaptureStartupErrors(false);
            webHostBuilder.ConfigureLogging((context, logging) => { logging.ClearProviders(); });

            webHostBuilder.ConfigureServices((context, collection) =>
            {
                collection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                collection.AddTransient<ClaimsValueEnricher>();
                collection.AddSingleton<ITelemetryInitializer>(
                    new CloudRoleNameInitializer(configuration["Logging:ApplicationInsights:RoleName"]));//, configuration["Logging:ApplicationInsights:RoleName"]));
                
                //new LoggingTelemetryConverter(configuration["Logging:ApplicationInsights:RoleName"], configuration["Logging:ApplicationInsights:RoleName"]));

                var provider = collection.BuildServiceProvider();
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                //var cloudRoleNameInitializer = provider.GetRequiredService<ITelemetryInitializer>();

                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    //.WriteTo.ApplicationInsights(, new EventTelemetryConverter())
                    .Enrich.WithClaimsValueEnricher(provider, "BusinessId", true)
                    .Enrich.WithClaimsValueEnricher(provider, "UserAccountId", true)
                    .Enrich.WithClaimsValueEnricher(provider, "Email", true);

                Logger logger = loggerConfiguration.CreateLogger();
                Log.Logger = logger;
                collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(logger, true));
            });
            return webHostBuilder;
        }
    }
}
