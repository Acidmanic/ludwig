using System;
using System.Collections.Generic;
using System.Globalization;
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
    public abstract class GitlabAuthenticatorBase : IAuthenticator
    {
        private class AuthHeader
        {
            public string Headervalue { get; set; }
        }

        private readonly Persistant<AuthHeader> _authHeaderPersistant = new Persistant<AuthHeader>();
        protected GitlabConfigurationProvider ConfigurationProvider { get; }
        protected ConfigureByLogin<GitlabConfigurations> ConfigureByLogin { get;  }
        
        private readonly LoginMethod _originalLoginMethod;
        
        
        protected GitlabAuthenticatorBase(GitlabConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
            ConfigureByLogin = new ConfigureByLogin<GitlabConfigurations>(ConfigurationProvider);
            // ReSharper disable once VirtualMemberCallInConstructor
            _originalLoginMethod = CreateLoginMethod();
        }


        protected abstract Dictionary<string, string> TokenCallFormEncodedParams(Dictionary<string, string> parameters);


        protected abstract LoginMethod CreateLoginMethod();
        

        protected async Task<AuthenticationResult> CallForToken(
            Dictionary<string,string> parameters,
            Dictionary<string,string> formParams)
        {
            if (formParams.IsFullyValued())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");

                request.Content = new FormUrlEncodedContent(formParams);
         
                var url = ConfigurationProvider.Configuration.GitlabInstanceBackChannel.Slashend();
            
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

                            ConfigureByLogin.HarvestConfigurations(parameters);

                            return new AuthenticationResult
                            {
                                Authenticated = true,
                                EmailAddress = "",
                                SubjectId = parameters.Read("username"),
                                SubjectWebPage = url + parameters.Read("username")
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

        public Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {

            var formEncodedParams = TokenCallFormEncodedParams(parameters);

            return CallForToken(parameters, formEncodedParams);
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
        

        public LoginMethod LoginMethod => ConfigureByLogin.EquipForUi(_originalLoginMethod);
    }
}