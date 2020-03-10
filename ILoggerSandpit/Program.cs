using Kernel.CrossCuttingConcerns;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace ILoggerSandpit
{
    public class Program
    {
        private static ILogger _logger;
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            //EventTelemetryConverter
            //TraceTelemetryConverter

            var webHost = CreateWebHostBuilder(args).Build();

            try
            {
                _logger = webHost.Services.GetRequiredService<ILogger<Program>>();
                _logger.LogInformation("The application is about to start...");
                
                webHost.Run();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "The application has crashed and the process will exit...");
                Console.WriteLine(e);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = GetConfiguration(environment);

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                })
                .UseStartup<Startup>()
                .UseCustomSerilogWebHostBuilder(
                    configuration, environment);

            return webHostBuilder;
        }

        private static IConfiguration GetConfiguration(string environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", false)
                .AddJsonFile($"appsettings.{environment}.loggingConfiguration.json", true);

            if (environment == EnvironmentName.Development)
            {
                try
                {
                    builder.AddUserSecrets<Startup>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            builder.AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }
    }
}
