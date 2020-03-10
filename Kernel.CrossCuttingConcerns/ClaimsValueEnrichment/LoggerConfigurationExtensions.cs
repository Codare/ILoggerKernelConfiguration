using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;

namespace Kernel.CrossCuttingConcerns.ClaimsValueEnrichment
{
    public static class LoggerEnrichmentConfigurationExtensions
    {
        //WithAspnetCoreHttpContext
        public static LoggerConfiguration WithClaimsValueEnricher(
            this LoggerEnrichmentConfiguration enrichmentConfiguration,
            IServiceProvider serviceProvider,
            string claimProperty,
            bool sanitizeLogOutput)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));

            var claimValueEnricher = serviceProvider.GetService<ClaimsValueEnricher>();

            if (!string.IsNullOrWhiteSpace(claimProperty))
                claimValueEnricher.SetClaimProperty(claimProperty, sanitizeLogOutput);

            return enrichmentConfiguration.With(claimValueEnricher);
        }

        //public static LoggerConfiguration WithClaimValue(
        //    this LoggerEnrichmentConfiguration enrichmentConfiguration,
        //    IHttpContextAccessor httpContextAccessor,
        //    bool sanitizeLogOutput,
        //    string claimProperty,
        //    string logEventProperty = null)
        //{
        //    if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
        //    return enrichmentConfiguration.With(new ClaimsValueEnricher(httpContextAccessor, claimProperty, logEventProperty, sanitizeLogOutput));
        //}
    }
}
