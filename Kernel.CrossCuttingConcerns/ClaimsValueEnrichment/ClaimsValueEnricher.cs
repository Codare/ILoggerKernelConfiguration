using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.CrossCuttingConcerns.ClaimsValueEnrichment
{
    public class ClaimsValueEnricher : ILogEventEnricher
    {
        string _claimProperty;
        string _logEventProperty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _sanitizeLogOutput;

        //public ClaimsValueEnricher(string claimProperty, string logEventProperty, bool sanitizeLogOutput = true)
        //    : this(new HttpContextAccessor(), claimProperty, logEventProperty, sanitizeLogOutput)
        //{
        //    _claimProperty = claimProperty;
        //    _logEventProperty = logEventProperty;
        //    _sanitizeLogOutput = sanitizeLogOutput;
        //}

        public ClaimsValueEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetClaimProperty(string claimProperty, bool sanitizeLogOutput)
        {
            _claimProperty = claimProperty;
            _sanitizeLogOutput = sanitizeLogOutput;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null)
                throw new ArgumentNullException(nameof(logEvent));

            if (_httpContextAccessor?.HttpContext.Request == null)
                return;

            //var user = _httpContextAccessor.HttpContext.User;

            //if (user == null)// || !user.Identity.IsAuthenticated)
            //    return;

            //var claims = ((ClaimsIdentity)user.Identity).Claims;

            var claims = new List<Claim>();

            claims.Add(new Claim("BusinessId", Guid.NewGuid().ToString()));
            claims.Add(new Claim("UserAccountId", Guid.NewGuid().ToString()));
            claims.Add(new Claim("Email", "a@b.com"));

            var claimValue = GetClaimValueFromClaims(claims);

            if (_sanitizeLogOutput)
                claimValue = GetSanitizedLogOutput(claimValue);

            if (string.IsNullOrWhiteSpace(claimValue)) return;

            //var claimProperty = new LogEventProperty(_logEventProperty, new ScalarValue(claimValue));
            //logEvent.AddPropertyIfAbsent(claimProperty);

            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(_claimProperty, claimValue, true));
        }

        private string GetClaimValueFromClaims(IEnumerable<Claim> claims)
        {
            return claims?.FirstOrDefault(c => c.Type == _claimProperty)?.Value;
        }

        private string GetSanitizedLogOutput(string claimValue)
        {
            return Guid.TryParse(claimValue, out _)
                ? SanitizeLogOutput.GetHashedString(claimValue)
                : SanitizeLogOutput.RedactSensitiveInfo(claimValue);
        }
    }
}
