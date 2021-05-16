using FlowerShop.Core.Constants;
using System.Security.Claims;
using System.Security.Principal;

namespace FlowerShop.Web.Extensions
{
    public static class IdentityExtensionscs
    {
        public static string GetEmail(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Email);

            return claim?.Value ?? string.Empty;
        }

        public static int GetUserId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimsConstants.UserId);

            return int.Parse(claim?.Value);
        }
    }
}