using System;
using System.Collections.Generic;
using System.Linq;
using Acidmanic.Utilities.Reflection.Attributes;
using Acidmanic.Utilities.Results;
using EnTier.UnitOfWork;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class OpenIdAuthenticatorPlus : GitlabAuthenticatorBase
    {
        public OpenIdAuthenticatorPlus(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }


        protected override Dictionary<string, string> TokenCallFormEncodedParams(Dictionary<string, string> parameters)
        {
            var code = parameters.Read("code");
            var clientId = ConfigureByLogin.ReadConfigurationFirst(parameters, "clientId");
            var clientSecret = ConfigureByLogin.ReadConfigurationFirst(parameters, "clientSecret");
            var conf = ConfigurationProvider.GetConfiguration<GitlabConfigurations>();
            
            
            return new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "grant_type", "authorization_code" },
                { "redirect_uri", conf.LudwigAddress.Slashend() + "login" },
                { "client_secret", clientSecret }
            };
        }
        
        private string CreateStateParams(GitlabConfigurations configurations)
        {
            var pkce = Pkce.CreateNew();

            var parameters = "state=" + pkce.State;

            return parameters;
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
                    "this application, gitlab would also give you an APPLICATION-ID which is needed here on first login." +
                    " Later you can change this application id in your configurations.",
                Link = new UiLink
                {
                    Title = "Login With Gitlab",
                    Url = conf.GitlabInstanceFrontChannel.Slashend() +
                          "oauth/authorize?client_id=" + conf.ClientId +
                          "&redirect_uri=" + ludwigLogin +
                          "&scope=api read_api read_user openid profile email" +
                          "&response_type=code&" + CreateStateParams(conf),
                },
                Name = "OAuth - Secure - Plus",
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
                    },
                    new ConfigurationRequirement
                    {
                        Description = "Secrete from application you created in gitlab",
                        ConfigurationName = nameof(GitlabConfigurations.ClientSecret),
                        DisplayName = "Secret"
                    },
                    new ConfigurationRequirement
                    {
                        Description = "Where you have served ludwig",
                        ConfigurationName = nameof(GitlabConfigurations.LudwigAddress),
                        DisplayName = "Ludwig Address"
                    }
                }
            };
        }
    }
}