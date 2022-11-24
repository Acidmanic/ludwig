using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Configurations;
using Newtonsoft.Json;

namespace Ludwig.IssueManager.Gitlab
{
    public class UsernamePasswordAuthenticator : IAuthenticator
    {
        private class AuthHeader
        {
            public string Headervalue { get; set; }
        }

        private class GitlabToken
        {
            [JsonPropertyName("access_token")] public string AccessToken { get; set; }
            [JsonPropertyName("token_type")] public string TokenType { get; set; }
            [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }
            [JsonPropertyName("scope")] public string Scope { get; set; }
            [JsonPropertyName("created_at")] public long CreatedAt { get; set; }
        }

        private readonly Persistant<AuthHeader> _authHeaderPersistant = new Persistant<AuthHeader>();
        private readonly GitlabConfigurationProvider _configurationProvider;


        public UsernamePasswordAuthenticator(GitlabConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _authHeaderPersistant.Load();
        }


        public async Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            var url = _configurationProvider.GetConfiguration().GitlabInstanceBackChannel.Slashend();

            var username = parameters.Read("username");
            var password = parameters.Read("password");
            var clientId = parameters.Read("applicationId");
            var clientSecret = parameters.Read("clientSecret");

            if (username.HasValue(password, clientId, clientSecret))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");

                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password },
                    { "client_id", clientId },
                    { "client_secret", clientSecret }
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

                            _authHeaderPersistant.Value=new AuthHeader { Headervalue = header };
                            
                            _authHeaderPersistant.Save();

                            return new AuthenticationResult
                            {
                                Authenticated = true,
                                EmailAddress = "",
                                SubjectId = username,
                                SubjectWebPage = url + username
                                
                            };
                        }
                        
                        
                    }
                    catch (Exception) { }
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
                    Value=_authHeaderPersistant.Value.Headervalue,
                    Type = RequestUpdate.RequestUpdateTypeHeader
                }
            });
        }

        public LoginMethod LoginMethod { get; } = new LoginMethod
        {
            Description = "You can create an application in your jira instance, and use it's" +
                          " application-id and secrete here alongside with your gitlab Username and Password." +
                          "Ludwig will use these information to connect with gitlab.",
            Name = "Gitlab Oauth2.0 as Client",
            Fields = new List<LoginField>
            {
                LoginField.Username,
                LoginField.Password,
                new LoginField
                {
                    Description = "Application-Id from application you created in gitlab",
                    Name = "applicationId",
                    DisplayName = "APPLICATION-ID"
                },
                new LoginField
                {
                    Description = "Secrete from application you created in gitlab",
                    Name = "clientSecret",
                    DisplayName = "Secret"
                }
            }
        };
    }
}