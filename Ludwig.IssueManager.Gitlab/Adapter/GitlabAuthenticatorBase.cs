using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Acidmanic.Utilities.Results;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
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
        protected IConfigurationProvider ConfigurationProvider { get; }
        protected ConfigureByLogin<GitlabConfigurations> ConfigureByLogin { get; }

        protected GitlabAuthenticatorBase(IConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
            ConfigureByLogin = new ConfigureByLogin<GitlabConfigurations>(ConfigurationProvider);
        }


        protected abstract Dictionary<string, string> TokenCallFormEncodedParams(Dictionary<string, string> parameters);


        protected abstract LoginMethod CreateLoginMethod();


        protected IStorage Storage { get; set; }


        protected virtual Result PreValidateCollectedInformation(Dictionary<string, string> parameters)
        {
            return new Result().Succeed();
        }

        protected async Task<AuthenticationResult> CallForToken(
            Dictionary<string, string> parameters,
            Dictionary<string, string> formParams)
        {
            if (formParams.IsFullyValued())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");

                request.Content = new FormUrlEncodedContent(formParams);

                var url = ConfigurationProvider.GetConfiguration<GitlabConfigurations>()
                    .GitlabInstanceBackChannel.Slashend();

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

                            return FromToken(token, parameters, url);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return new AuthenticationResult { Authenticated = false };
        }
        
        private AuthenticationResult FromToken(GitlabToken token,
            Dictionary<string, string> parameters,
            string url)
        {
            var readUsername = parameters.Read("username");

            var jwt = token.IdToken;

            if (string.IsNullOrWhiteSpace(jwt))
            {
                jwt = token.AccessToken;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwt);

                if (jwtSecurityToken != null)
                {
                    return new AuthenticationResult
                    {
                        Authenticated = true,
                        EmailAddress = jwtSecurityToken.Claims
                            .Where(c => c.Type?.Trim().ToLower() == "email")
                            .Select(c => c.Value).FirstOrDefault(),
                        SubjectId = jwtSecurityToken.Subject,
                        SubjectWebPage = url.Slashend(),
                        IsAdministrator = false,
                        IsIssueManager = true
                    };
                }
            }
            catch (Exception _)
            {
            }

            return new AuthenticationResult
            {
                Authenticated = true,
                EmailAddress = "",
                SubjectId = readUsername,
                SubjectWebPage = url.Slashend() + readUsername,
                IsAdministrator = false,
                IsIssueManager = true
            };
        }

        public Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            if (PreValidateCollectedInformation(parameters))
            {
                var formEncodedParams = TokenCallFormEncodedParams(parameters);

                return CallForToken(parameters, formEncodedParams);
            }

            return Task.Run(() => new AuthenticationResult { Authenticated = false });
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
        
        private LoginMethod _originalLoginMethod;

        public LoginMethod LoginMethod => ConfigureByLogin.EquipForUi(_originalLoginMethod);

        public void UseStorage(IStorage storage)
        {
            Storage = storage;

            _originalLoginMethod = CreateLoginMethod();
        }
    }
}