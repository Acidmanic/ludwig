using Microsoft.AspNetCore.Authorization;

namespace Ludwig.Presentation.Authentication.Attributes
{
    public class AuthorizeAdministrators:AuthorizeAttribute
    {
        public AuthorizeAdministrators() : base(LudwigPolicies.AdministratorsOnly)
        {
        }
    }
}