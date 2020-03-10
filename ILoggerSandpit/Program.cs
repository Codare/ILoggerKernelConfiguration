using Kernel.CrossCuttingConcerns;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ILoggerSandpit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = GetConfiguration(environment);

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                //.ConfigureLogging((context, logging) =>
                //{
                //    logging.ClearProviders();
                //})
                .UseStartup<Startup>()
                .UseCustomSerilogWebHostBuilder(
                    (provider, context, loggerConfiguration) => { },
                    configuration, environment);

            return webHostBuilder;


            //        (provider, context, loggerConfiguration) =>
            //{
            //    //return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
            //}, configuration, environment);
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
