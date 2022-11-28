using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class OpenIdAuthenticator : IAuthenticator
    {
        private readonly GitlabConfigurationProvider _configurationProvider;

        public OpenIdAuthenticator(GitlabConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;

            SetupLoginMethod();
        }

        public async  Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            var token = parameters.Read("token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                var url = _configurationProvider.Configuration.GitlabInstanceBackChannel.Slashend()
                          + "oauth/token/";
            }

            return new AuthenticationResult() { Authenticated = false };
        }

        public Task<List<RequestUpdate>> GrantAccess()
        {
            throw new System.NotImplementedException();
        }

        public LoginMethod LoginMethod { get; private set; }


        private void SetupLoginMethod()
        {

            var conf = _configurationProvider.Configuration;

            var ludwigLogin = conf.LudwigAddress.Slashend()+"login";
            
            LoginMethod = new LoginMethod
            {
                Description =
                    "Please Use The Link below to allow Ludwig to your Gitlab instance. and login to ludwig using " +
                    "gitlab. When you're redirected back here, click login. For this to work, you need to have " +
                    $"an application created in your gitlab instance with redirect-uri: {ludwigLogin}. When you create " +
                    $"this application, gitlab would also give you an APPLICATION-ID which is needed here on first login." +
                    $" Later you can change this application id in your configurations.",
                Link = new UiLink
                {
                    Title = "Login With Gitlab",
                    Url = conf.GitlabInstanceFrontChannel.Slashend()+
                          "/oauth/authorize?client_id="+conf.ClientId + 
                          "&redirect_uri=" + ludwigLogin +
                          "&response_type=code",
                },
                Name = "OAuth - Secure",
                Fields = new List<LoginField>(),
                Queries = new List<LoginQuery>
                {
                    new LoginQuery
                    {
                        Name = "Authorization Code",
                        QueryKey = "code",
                        ProvidedStateDescription = "Authorization Code Received",
                        NotProvidedStateDescription = "AuthorizationCode Needed"
                    }
                }
            };
        }
    }
}