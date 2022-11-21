using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ludwig.Contracts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ludwig.Presentation.Authentication
{
    public class LudwigTokenAuthenticationHandler : AuthenticationHandler<LudwigAuthenticationSchemeOptions>
    {
        private readonly AuthenticationManager _authenticationManager;


        public LudwigTokenAuthenticationHandler(IOptionsMonitor<LudwigAuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, AuthenticationManager authenticationManager)
            : base(options, logger, encoder, clock)
        {
            _authenticationManager = authenticationManager;
        }

        
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            return Task.Run(() =>
            {
                var logged =  _authenticationManager.IsAuthorized();

                if (logged)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,logged.Value.EmailAddress),
                        new Claim(ClaimTypes.Sid,logged.Value.SubjectId),
                        new Claim(ClaimTypes.Webpage,logged.Value.SubjectWebPage)
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new GenericPrincipal(identity, null);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                return AuthenticateResult.Fail("Unauthorized");
                
            });

            
        }
    }
}