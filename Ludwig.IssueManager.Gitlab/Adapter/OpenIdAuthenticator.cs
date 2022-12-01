using System.Collections.Generic;
using System.Linq;
using Acidmanic.Utilities.Reflection.Attributes;
using Acidmanic.Utilities.Results;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class OpenIdAuthenticator : GitlabAuthenticatorBase
    {
        public OpenIdAuthenticator(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }

        private class PkceRecord : Pkce
        {
            [AutoValuedMember] [UniqueMember] public long Id { get; set; }

            public static PkceRecord FromPkce(Pkce pkce)
            {
                return new PkceRecord
                {
                    Challenge = pkce.Challenge,
                    State = pkce.State,
                    Verifier = pkce.Verifier
                };
            }
        }

        protected override Dictionary<string, string> TokenCallFormEncodedParams(Dictionary<string, string> parameters)
        {
            var code = parameters.Read("code");
            var clientId = ConfigurationProvider.ReadByName<string>("clientId");

            var foundVerifier = GetVerifierForState(parameters);
            
            
            return new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "grant_type", "authorization_code" },
                { "redirect_uri", "http://localhost:13801/login" },
                { "code_verifier", foundVerifier.Value }
            };
        }


        private string CreateStateParams(GitlabConfigurations configurations)
        {
            var pkce = Pkce.CreateNew();

            var parameters = "state=" + pkce.State
                                      + "&challenge=" + pkce.Challenge
                                      + "&code_challenge_method=S256";

            Storage.Store(PkceRecord.FromPkce(pkce));

            return parameters;
        }


        protected override Result PreValidateCollectedInformation(Dictionary<string, string> parameters)
        {
            var foundVerifier = GetVerifierForState(parameters);

            return foundVerifier;
        }
        
        private Result<string> GetVerifierForState(Dictionary<string, string> parameters)
        {
            var state = parameters.Read("state");

            if (!string.IsNullOrWhiteSpace(state))
            {
                var found = Storage.Find<PkceRecord>(p => p.State == state)
                    .FirstOrDefault();

                if (found!=null)
                {
                    return new Result<string>(true, found.Verifier);
                }    
            }
            return new Result<string>().FailAndDefaultValue();
        }


        protected override LoginMethod CreateLoginMethod()
        {
            var conf = ConfigurationProvider.GetConfiguration<GitlabConfigurations>();


            var server = conf.LudwigAddress;

            if (string.IsNullOrWhiteSpace(server))
            {
                server = "http://localhost:13801";
            }

            var ludwigLogin = server.Slashend() + "login";

            return new LoginMethod
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
                    Url = conf.GitlabInstanceFrontChannel.Slashend() +
                          "oauth/authorize?client_id=" + conf.ClientId +
                          "&redirect_uri=" + ludwigLogin +
                          "&scope=api read_api read_user openid profile email" +
                          "&response_type=code&" + CreateStateParams(conf),
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
                    },
                    new LoginQuery
                    {
                        Name = "Client State",
                        QueryKey = "state",
                        ProvidedStateDescription = "Client State Is Present",
                        NotProvidedStateDescription = "Client State Is Needed"
                    }
                },
                ConfigurationRequirements = new List<ConfigurationRequirement>
                {
                    new ConfigurationRequirement
                    {
                        Description = "Application-Id from application you created in gitlab",
                        DisplayName = "APPLICATION-ID",
                        ConfigurationName = nameof(GitlabConfigurations.ClientId)
                    }
                }
            };
        }
    }
}