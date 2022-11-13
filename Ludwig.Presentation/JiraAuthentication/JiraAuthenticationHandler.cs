using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ludwig.Presentation.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ludwig.Presentation.JiraAuthentication
{
    public class JiraAuthenticationHandler:AuthenticationHandler<JiraAuthenticationSchemeOptions>
    {

        
        private readonly Jira _jira;

        public JiraAuthenticationHandler(IOptionsMonitor<JiraAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder,
            ISystemClock clock, Jira jira) : base(options, logger, encoder, clock)
        {
            _jira = jira;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            _jira.UseContext(Context);

            var logged = await _jira.LoggedInUser();

            if (logged)
            {
                var user = logged.Value;
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,user.EmailAddress),
                    new Claim(ClaimTypes.Sid,user.Name),
                    new Claim(ClaimTypes.Webpage,user.Self)
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}