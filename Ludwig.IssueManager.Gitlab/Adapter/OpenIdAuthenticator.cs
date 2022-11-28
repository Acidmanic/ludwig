using System;
using System.Collections.Generic;
using System.Net.Http;
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
    public class OpenIdAuthenticator : GitlabAuthenticatorBase
    {
        // private readonly GitlabConfigurationProvider _configurationProvider;
        // private readonly ConfigureByLogin<GitlabConfigurations> _configureByLogin;
        //
        // public OpenIdAuthenticator(GitlabConfigurationProvider configurationProvider)
        // {
        //     _configurationProvider = configurationProvider;
        //     _configureByLogin = new ConfigureByLogin<GitlabConfigurations>(_configurationProvider);
        //     SetupLoginMethod();
        // }
        //
        // public async  Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        // {
        //     var code = parameters.Read("code");
        //     var clientId = _configureByLogin.ReadConfigurationFirst(parameters, "clientId");
        //     
        //     if (code.HasValue(clientId))
        //     {
        //         
        //         var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");
        //         
        //         request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        //         {
        //             { "code", code },
        //             { "client_id", clientId },
        //             { "grant_type", "authorization_code" },
        //         });
        //
        //         var http = new HttpClient
        //         {
        //             BaseAddress = new Uri(_configurationProvider.Configuration.GitlabInstanceBackChannel, UriKind.Absolute)
        //         };
        //
        //         var response = await http.SendAsync(request);
        //
        //         if (response.IsSuccessStatusCode)
        //         {
        //             var json = await response.Content.ReadAsStringAsync();
        //
        //             try
        //             {
        //                 var token = JsonConvert.DeserializeObject<GitlabToken>(json);
        //
        //                 if (token != null)
        //                 {
        //                     var header = "Bearer " + token.AccessToken;
        //
        //                     _authHeaderPersistant.Value = new AuthHeader { Headervalue = header };
        //
        //                     _authHeaderPersistant.Save();
        //
        //                     _configureByLogin.HarvestConfigurations(parameters);
        //
        //                     return new AuthenticationResult
        //                     {
        //                         Authenticated = true,
        //                         EmailAddress = "",
        //                         SubjectId = username,
        //                         SubjectWebPage = url + username
        //                     };
        //                 }
        //             }
        //             catch (Exception)
        //             {
        //             }
        //         }
        //     }
        //
        //     return new AuthenticationResult() { Authenticated = false };
        // }
        //
        // public Task<List<RequestUpdate>> GrantAccess()
        // {
        //     throw new System.NotImplementedException();
        // }
        //
        //
        // public LoginMethod LoginMethod { get; private set; }
        //
        //
        // private void SetupLoginMethod()
        
        public OpenIdAuthenticator(GitlabConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }

        protected override Dictionary<string, string> TokenCallFormEncodedParams(Dictionary<string, string> parameters)
        {
            var code = parameters.Read("code");
            var clientId = ConfigureByLogin.ReadConfigurationFirst(parameters, "clientId");
            
            return new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "grant_type", "authorization_code" },
            };
        }

        protected override LoginMethod CreateLoginMethod()
        {
        
            var conf = ConfigurationProvider.Configuration;


            var server = conf.LudwigAddress;

            if (string.IsNullOrWhiteSpace(server))
            {
                server = "http://localhost:13801";
            }
            var ludwigLogin = server.Slashend()+"login";
            
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
                    Url = conf.GitlabInstanceFrontChannel.Slashend()+
                          "oauth/authorize?client_id="+conf.ClientId + 
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