using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Configurations;
using Ludwig.IssueManager.Gitlab.Models;
using Newtonsoft.Json;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class UsernamePasswordAuthenticator : GitlabAuthenticatorBase
    {
       
        protected override LoginMethod CreateLoginMethod()
        {
            return
                new LoginMethod
                {
                    Description = "You can create an application in your gitlab instance, and use it's" +
                                  " application-id and secrete here alongside with your gitlab Username and Password." +
                                  "Ludwig will use these information to connect with gitlab.",
                    Name = "Gitlab Oauth2.0 as Client",
                    Fields = new List<LoginField>
                    {
                        LoginField.Username,
                        LoginField.Password
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
                        }
                    }
                };
        }


        // public LoginMethod LoginMethod => _configureByLogin.EquipForUi(_originalMethod);
        public UsernamePasswordAuthenticator(GitlabConfigurationProvider configurationProvider) : base(
            configurationProvider)
        {
        }

        protected override Dictionary<string, string> TokenCallFormEncodedParams(Dictionary<string, string> parameters)
        {
            var username = parameters.Read("username");
            var password = parameters.Read("password");
            var clientId = ConfigureByLogin.ReadConfigurationFirst(parameters, "clientId");
            var clientSecret = ConfigureByLogin.ReadConfigurationFirst(parameters, "clientSecret");

            return new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "password" }
            };
        }
    }
}