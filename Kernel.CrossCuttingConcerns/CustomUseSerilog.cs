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

                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
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
