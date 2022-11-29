using Microsoft.AspNetCore.Authorization;

namespace Ludwig.Presentation.Authentication.Attributes
{
    public class AuthorizeIssueManagers:AuthorizeAttribute
    {
        public AuthorizeIssueManagers() : base(LudwigPolicies.IssueManagersOnly)
        {
        }
    }
}