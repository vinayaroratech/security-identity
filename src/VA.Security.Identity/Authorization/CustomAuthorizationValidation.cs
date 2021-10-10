using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace VA.Security.Identity.Authorization
{
    public static class CustomAuthorizationValidation
    {
        public static bool UserHasValidClaim(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                   context.User.Claims.Any(c =>
                       c.Type == claimName &&
                       c.Value.Split(',').Select(v => v.Trim()).Contains(claimValue));
        }
    }
}
