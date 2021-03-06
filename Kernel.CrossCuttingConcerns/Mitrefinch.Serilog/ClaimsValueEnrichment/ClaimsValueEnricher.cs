﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.CrossCuttingConcerns.Mitrefinch.Serilog.ClaimsValueEnrichment
{
    public class ClaimsValueEnricher : ILogEventEnricher
    {
        string _claimProperty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _sanitizeLogOutput;

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

            if (_httpContextAccessor?.HttpContext?.Request == null)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(_claimProperty, "There is no HttpContext.Request yet!", true));
                return;
            }

            var user = _httpContextAccessor.HttpContext.User;

            if (user == null)// || !user.Identity.IsAuthenticated)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(_claimProperty, "No user or user is not authenticated!", true));
                return;
            }

            var claims = new List<Claim>();

            claims.Add(new Claim("businessAccountId", Guid.NewGuid().ToString()));
            claims.Add(new Claim("userAccountId", Guid.NewGuid().ToString()));
            claims.Add(new Claim("email", "a@b.com"));

            //var claims = ((ClaimsIdentity)user.Identity).Claims.ToList();

            if(!claims.Any())
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(_claimProperty, "No claims for user!", true));

            var claimValue = GetClaimValueFromClaims(claims);

            if (_sanitizeLogOutput)
                claimValue = GetSanitizedLogOutput(claimValue);

            if (string.IsNullOrWhiteSpace(claimValue)) return;

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
