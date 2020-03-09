using System;
using System.IO;
using Kernel.CrossCuttingConcerns;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ILoggerSandpit
{
    public class Program : SerilogAspNetCoreWebAppBaseProgram
    {
        public static void Main(string[] args)
        {
            //GetConfiguration.  Move to Kernel...  Perhaps?
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", false);

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
            //Move to Kernel..  To get it as var configuration = GetConfiguration.

            SerilogMainBootstrapper(() => CreateWebHostBuilder(args).Build().Run(), configuration);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
            BuildWebHostUsingSerilog(WebHost.CreateDefaultBuilder(args).UseStartup<Startup>(), args);
    }
}
