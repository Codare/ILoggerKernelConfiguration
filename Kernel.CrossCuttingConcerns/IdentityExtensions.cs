using System.Security.Claims;
using System.Security.Principal;

namespace Kernel.CrossCuttingConcerns
{
    public static class IdentityExtensions
    {
        public static string GetClaim(this IIdentity identity, string claimName)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(claimName);
            return (claim != null) 
                ? claim.Value 
                : string.Empty;
        }
    }
}
