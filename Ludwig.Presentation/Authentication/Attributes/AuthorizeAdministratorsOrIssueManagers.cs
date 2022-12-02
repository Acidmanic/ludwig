using Microsoft.AspNetCore.Authorization;

namespace Ludwig.Presentation.Authentication.Attributes
{
    public class AuthorizeAdministratorsOrIssueManagers:AuthorizeAttribute
    {
        public AuthorizeAdministratorsOrIssueManagers() : base(LudwigPolicies.AdministratorOrIssueManager)
        {
        }
    }
}