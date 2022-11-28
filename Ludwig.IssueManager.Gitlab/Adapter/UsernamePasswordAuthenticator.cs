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
    public class UsernamePasswordAuthenticator : IAuthenticator
    {
        private class AuthHeader
        {
            public string Headervalue { get; set; }
        }

        private readonly Persistant<AuthHeader> _authHeaderPersistant = new Persistant<AuthHeader>();
        private readonly GitlabConfigurationProvider _configurationProvider;
        private readonly ConfigureByLogin<GitlabConfigurations> _configureByLogin;

        public UsernamePasswordAuthenticator(GitlabConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _authHeaderPersistant.Load();
            _configureByLogin = new ConfigureByLogin<GitlabConfigurations>(_configurationProvider);
        }


        public async Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            var url = _configurationProvider.Configuration.GitlabInstanceBackChannel.Slashend();

            var username = parameters.Read("username");
            var password = parameters.Read("password");
            var clientId = _configureByLogin.ReadConfigurationFirst(parameters, "applicationId");
            var clientSecret = _configureByLogin.ReadConfigurationFirst(parameters, "clientSecret");

            if (username.HasValue(password, clientId, clientSecret))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");

                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "grant_type", "password" }
                });

                var http = new HttpClient
                {
                    BaseAddress = new Uri(url, UriKind.Absolute)
                };

                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var token = JsonConvert.DeserializeObject<GitlabToken>(json);

                        if (token != null)
                        {
                            var header = "Bearer " + token.AccessToken;

                            _authHeaderPersistant.Value = new AuthHeader { Headervalue = header };

                            _authHeaderPersistant.Save();

                            _configureByLogin.HarvestConfigurations(parameters);

                            return new AuthenticationResult
                            {
                                Authenticated = true,
                                EmailAddress = "",
                                SubjectId = username,
                                SubjectWebPage = url + username
                            };
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return new AuthenticationResult { Authenticated = false };
        }

        public Task<List<RequestUpdate>> GrantAccess()
        {
            return Task.Run(() => new List<RequestUpdate>
            {
                new RequestUpdate
                {
                    Key = "Authorization",
                    Value = _authHeaderPersistant.Value.Headervalue,
                    Type = RequestUpdate.RequestUpdateTypeHeader
                }
            });
        }

        private readonly LoginMethod _originalMethod = new LoginMethod
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


        public LoginMethod LoginMethod => _configureByLogin.EquipForUi(_originalMethod);
    }
}