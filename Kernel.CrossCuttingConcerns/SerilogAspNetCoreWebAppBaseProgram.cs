//using System;
//using Kernel.CrossCuttingConcerns.ClaimsValueEnrichment;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Serilog;

//namespace Kernel.CrossCuttingConcerns
//{
    //public abstract class SerilogAspNetCoreWebAppBaseProgram
    //{
    //    //private static IServiceProvider _serviceProvider;

    //    public static IWebHostBuilder BuildWebHostUsingSerilog(IWebHostBuilder webHostBuilder, IConfiguration configuration, string environment, string[] args)
    //    {
    //        return webHostBuilder.UseCustomSerilogWebHostBuilder(provider =>
    //        {
    //            var sanitizeLogOutput = environment == EnvironmentName.Development;

    //            Log.Logger = new LoggerConfiguration()
    //            .ReadFrom.Configuration(configuration)
    //            .Enrich.WithClaimsValueEnricher(provider, "BusinessId", sanitizeLogOutput)
    //            //.Enrich.WithClaimValue(sanitizeLogOutput, "UserAccountId", "UserAccountId")
    //            //.Enrich.WithClaimValue(sanitizeLogOutput, "email", "email")
    //            .CreateLogger();
    //        });
    //    }

        //public static int SerilogMainBootstrapper(Action run, IConfiguration configuration, string environment)
        //{
        //    var sanitizeLogOutput = environment == EnvironmentName.Development;

        //        Log.Logger = new LoggerConfiguration()
        //        .ReadFrom.Configuration(configuration)
        //        .Enrich.WithClaimsValueEnricher(_serviceProvider, "BusinessId", sanitizeLogOutput)
        //        //.Enrich.WithClaimValue(sanitizeLogOutput, "UserAccountId", "UserAccountId")
        //        //.Enrich.WithClaimValue(sanitizeLogOutput, "email", "email")
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
//    }
//}

/* Possible Claims to log..
 * "role": "BusinessOwner",
  "Permission:Account:EditBusinessAccount": "True",
  "Permission:Account:CreateUsers": "True",
  "Permission:Account:ModifyUsers": "True",
  "Permission:Account:DeleteUsers": "True",
  "Permission:Account:ImportEnrolUsers": "True",
  "Permission:Account:AccessAuditLogs": "True",
  "Permission:Account:EditPersonalProfile": "False",
  "Permission:Account:DeleteUserAccount": "False",
  "Permission:Account:CreateGlobalAdmin": "True",
  "Permission:Account:ResetUserPassword": "True",
  "Permission:Account:SuspendUser": "True",
  "Permission:Account:AddRemoveSecurityProfiles": "True",
  "Permission:Account:EditUserGroups": "True",
  "Permission:Account:ManageSecurity": "True",
 */
