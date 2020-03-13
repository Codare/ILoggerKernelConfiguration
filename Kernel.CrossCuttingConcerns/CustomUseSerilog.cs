using System;
using Destructurama;
using Kernel.CrossCuttingConcerns.ClaimsValueEnrichment;
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
                collection.AddSingleton<ITelemetryInitializer>(s =>
                    new CloudRoleNameTelemetryInitializer(configuration["Logging:ApplicationInsights:RoleName"], configuration["Logging:ApplicationInsights:RoleInstance"]));

                var provider = collection.BuildServiceProvider();
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                var redactSensitiveInformation =
                    GetBooleanFromConfigFile(configuration["Logging:RedactSensitiveInformation"]);

                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .Destructure.UsingAttributes()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithClaimsValueEnricher(provider, "BusinessId", redactSensitiveInformation)
                    .Enrich.WithClaimsValueEnricher(provider, "UserAccountId", redactSensitiveInformation)
                    .Enrich.WithClaimsValueEnricher(provider, "Email", redactSensitiveInformation);

                Logger logger = loggerConfiguration.CreateLogger();
                collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(logger, true));
            });
            return webHostBuilder;
        }

        private static bool GetBooleanFromConfigFile(string configSetting)
        {
            var success = bool.TryParse(configSetting, out var result);

            if (!success) return true;
            return result;
        }
    }
}
