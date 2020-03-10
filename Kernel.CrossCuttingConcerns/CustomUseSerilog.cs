using System;
using Kernel.CrossCuttingConcerns.ClaimsValueEnrichment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog.AspNetCore;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using LoggerExtensions = Microsoft.Extensions.Logging.LoggerExtensions;

namespace Kernel.CrossCuttingConcerns
{
    public static class CustomUseSerilog
    {
        //this IWebHostBuilder builder, IConfiguration configuration, string environment)
        public static IWebHostBuilder UseCustomSerilogWebHostBuilder(this IWebHostBuilder webHostBuilder,
            Action<IServiceProvider, WebHostBuilderContext, LoggerConfiguration> configureLogger,
            IConfiguration configuration, string environment)
        {
            if (webHostBuilder == null)
                throw new ArgumentNullException(nameof(webHostBuilder));

            if (configureLogger == null)
                throw new ArgumentNullException(nameof(configureLogger));

            webHostBuilder.ConfigureServices((context, collection) =>
            {
                collection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                collection.AddTransient<ClaimsValueEnricher>();

                var provider = collection.BuildServiceProvider();
                var hca = provider.GetRequiredService<IHttpContextAccessor>();

                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithClaimsValueEnricher(provider, "BusinessId", true)
                        .Enrich.WithClaimsValueEnricher(provider, "UserAccountId", true)
                        .Enrich.WithClaimsValueEnricher(provider, "Email", true)

                //.Enrich.WithClaimValue(sanitizeLogOutput, "UserAccountId", "UserAccountId")
                //.Enrich.WithClaimValue(sanitizeLogOutput, "email", "email")
                //.CreateLogger();
                ;

                configureLogger(collection.BuildServiceProvider(), context, loggerConfiguration);

                Logger logger = loggerConfiguration.CreateLogger();

                //if (preserveStaticLogger)
                //{
                //    collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(logger, true));
                //}
                //else
                //{
                    Log.Logger = logger;
                    collection.AddSingleton(services => (ILoggerFactory) new SerilogLoggerFactory(logger, true));
                //}
            });
            return webHostBuilder;

            //if (builder == null)
            //    throw new ArgumentNullException(nameof(builder));

            //builder.ConfigureServices((context, collection) =>
            //{
            //    collection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //    collection.AddTransient<ClaimsValueEnricher>();

            //    var provider = collection.BuildServiceProvider();
            //    //provider = collection.BuildServiceProvider();

            //    var hca = provider.GetRequiredService<IHttpContextAccessor>();
            //    //var hca1 = provider.GetRequiredService<ClaimsValueEnricher>();

            //    var sanitizeLogOutput = environment == EnvironmentName.Development;

            //    Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .Enrich.WithClaimsValueEnricher(provider, "BusinessId", sanitizeLogOutput)
            //    //.Enrich.WithClaimValue(sanitizeLogOutput, "UserAccountId", "UserAccountId")
            //    //.Enrich.WithClaimValue(sanitizeLogOutput, "email", "email")
            //    .CreateLogger();

            //    //LoggerConfiguration loggerConfiguration = new LoggerConfiguration();
            //    //configureLogger(collection.BuildServiceProvider(), context, loggerConfiguration);

            //    //Logger logger = loggerConfiguration.CreateLogger();

            //    //if (preserveStaticLogger)
            //    //{
            //    //    collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(logger, true));
            //    //}
            //    //else
            //    //{
            //    //    Log.Logger = logger;
            //    //    collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(null, true));
            //    //}
            //});
            //return builder;
        }
    }

}
