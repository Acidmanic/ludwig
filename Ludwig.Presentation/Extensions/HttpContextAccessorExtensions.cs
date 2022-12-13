using System.Linq;
using Ludwig.Presentation.Authentication;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Extensions
{
    public static class HttpContextAccessorExtensions
    {



        public static string GetLoginMethodNameClaim(this IHttpContextAccessor contextAccessor)
        {
            var loginMethodName = contextAccessor.HttpContext.User.Claims.
                FirstOrDefault(p=>p.Type == LudwigClaims.LoginMethod)?.Value;

            return loginMethodName;
        }
    }
}