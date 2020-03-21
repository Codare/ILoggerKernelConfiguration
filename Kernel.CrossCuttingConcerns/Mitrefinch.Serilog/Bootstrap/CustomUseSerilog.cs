using System;
using Destructurama;
using Kernel.CrossCuttingConcerns.Mitrefinch.Serilog.ApplicationInsights;
using Kernel.CrossCuttingConcerns.Mitrefinch.Serilog.ClaimsValueEnrichment;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Core;
using Serilog.Exceptions;

namespace Kernel.CrossCuttingConcerns.Mitrefinch.Serilog.Bootstrap
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
                collection.AddSingleton<ITelemetryInitializer>(s =>
                    new CloudRoleNameTelemetryInitializer(configuration["Logging:ApplicationInsights:RoleName"], configuration["Logging:ApplicationInsights:RoleInstance"]));

                var provider = collection.BuildServiceProvider();
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                bool result = bool.TryParse(configuration["Logging:RedactSensitiveInformation"], out var redactSensitiveInformation);
                if (!result)
                    redactSensitiveInformation = true;

                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Destructure.UsingAttributes()
                    .Enrich.WithClaimsValueEnricher(provider, "businessAccountId", redactSensitiveInformation)
                    .Enrich.WithClaimsValueEnricher(provider, "userAccountId", redactSensitiveInformation)
                    .Enrich.WithClaimsValueEnricher(provider, "email", redactSensitiveInformation);

                Logger logger = loggerConfiguration.CreateLogger();
                collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(logger, true));
            });
            return webHostBuilder;
        }
    }
}
