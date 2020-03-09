using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Kernel.CrossCuttingConcerns
{
    public abstract class SerilogAspNetCoreWebAppBaseProgram
    {
        public static IWebHostBuilder BuildWebHostUsingSerilog(IWebHostBuilder webHostBuilder, string[] args) => webHostBuilder.UseSerilog();

        public static int SerilogMainBootstrapper(Action run, IConfiguration configuration)
        {
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .AddJsonFile($"appsettings.{environment}.json", false);

            //if (environment == EnvironmentName.Development)
            //{
            //    try
            //    {
            //        builder.AddUserSecrets<Startup>();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}

            //builder.AddEnvironmentVariables();
            //var configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                //.Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        //public static int MainWrapper(Action run)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .Build();

        //    Log.Logger = new LoggerConfiguration()
        //        .ReadFrom.Configuration(configuration)
        //        .Enrich.FromLogContext()
        //        .CreateLogger();

        //    try
        //    {
        //        Log.Information("Starting web host");
        //        run();
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Fatal(ex, "Host terminated unexpectedly");
        //        return 1;
        //    }
        //    finally
        //    {
        //        Log.CloseAndFlush();
        //    }
        //}
    }
}
