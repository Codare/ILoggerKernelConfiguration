using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Kernel.CrossCuttingConcerns
{
    public class CustomEnricherLogic
    {
        public static CustomEnricherProperties GetBusinessAccountEnricherDetails(IHttpContextAccessor ctx)
        {
            var context = ctx.HttpContext;
            if (context == null) return null;

            var myInfo = new CustomEnricherProperties
            {
                BusinessId = context.Request.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "BusinessId")
                    ?.ToString(),
                UserAccountId = context.Request.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "UserAccountId")
                    ?.ToString()
            };

            //var user = context.User;
            //if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            //{
            //    myInfo.UserClaims = user.Claims.Select(a => new KeyValuePair<string, string>(a.Type, a.Value)).ToList();
            //}

            return myInfo;
        }

        public class CustomEnricherProperties
        {
            public string BusinessId { get; set; }
            public string UserAccountId { get; set; }
        }
    }
}
