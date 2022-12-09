using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Common.Configuration;
using Ludwig.Common.Utilities;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Configuration;
using Ludwig.IssueManager.Jira.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ludwig.IssueManager.Jira.Adapter
{
    internal class JiraCookieHijack:IAuthenticator
    {


        private static readonly List<string> JiraAuthCookieNames = new List<string>
        {
            "jira.editor.user.mode",
            "jSESSIONID".ToLower(),
            "atlassian.xsrf.token"
        };
        private class GrantRecord
        {
            public List<RequestUpdate> CookiesUpdate { get; set; }
            
            public long Id { get; set; }
        }


        private class IllegalGrant : IBackChannelRequestGrant
        {

            private readonly Dictionary<string, string> _cookies;

            public IllegalGrant(Dictionary<string, string> cookies)
            {
                _cookies = cookies;
            }

            public List<RequestUpdate> GetGrantRequestUpdates()
            {
                var list = new List<RequestUpdate>();

                foreach (var cookie in _cookies)
                {
                    
                    list.Add(new RequestUpdate
                    {
                        Key = cookie.Key,
                        Value = cookie.Value,
                        Type = RequestUpdate.RequestUpdateTypeCookie
                    });
                }

                return list;
            }
        }
        
        private readonly Services.Jira.Jira _jira;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICustomFieldDefinitionProvider _customFieldDefinitionProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        
        private readonly Persistant<GrantRecord> _persistantGrantRecord = new Persistant<GrantRecord>(); 

        public JiraCookieHijack(Services.Jira.Jira jira, IConfigurationProvider configurationProvider, IHttpContextAccessor contextAccessor, ICustomFieldDefinitionProvider customFieldDefinitionProvider)
        {
            _jira = jira;
            _configurationProvider = configurationProvider;
            _contextAccessor = contextAccessor;
            _customFieldDefinitionProvider = customFieldDefinitionProvider;
            _persistantGrantRecord.Load();
        }

        public async Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {

            var jiraCookies = new Dictionary<string, string>();

            var context = _contextAccessor.HttpContext;
            
            foreach (var cookie in context.Request.Cookies)
            {
                var lowKey = cookie.Key.ToLower();
                
                if (JiraAuthCookieNames.Contains(lowKey))
                {
                    jiraCookies.Add(cookie.Key,cookie.Value);   
                }
            }

            var grant = new IllegalGrant(jiraCookies);

            var jira = new Services.Jira.Jira(_configurationProvider, _customFieldDefinitionProvider, grant);
            
            var loggedIn = await jira.LoggedInUser();

            if (loggedIn)
            {
                var frontChannel = _configurationProvider.GetConfiguration<JiraConfiguration>().JiraFrontChannelUrl;

                if (!frontChannel.EndsWith("/"))
                {
                    frontChannel = frontChannel + "/";
                }

                _persistantGrantRecord.Value = new GrantRecord();
                _persistantGrantRecord.Value.CookiesUpdate = grant.GetGrantRequestUpdates();
                _persistantGrantRecord.Save();
                
                return new AuthenticationResult
                {
                    Authenticated = true,
                    EmailAddress = loggedIn.Primary.EmailAddress,
                    SubjectId = loggedIn.Primary.Name,
                    SubjectWebPage = frontChannel + "secure/ViewProfile.jspa?name=" + loggedIn.Primary.Name,
                    IsAdministrator = false,
                    IsIssueManager = true
                };
            }

            return new AuthenticationResult { Authenticated = false };
        }

        public Task<List<RequestUpdate>> GrantAccess()
        {
            return Task.Run(() => _persistantGrantRecord.Value.CookiesUpdate);
        }

        public LoginMethod LoginMethod { get; } = new LoginMethod
        {
            Description = $"Make Sure you have already been logged-in, in your main jira account, then just hit Login. " +
                          $"(This only works when you are serving ludwig under the same domain as your jira instance)",
            Name = "Hijack Jira Auth",
            Fields = new List<LoginField> { },
            IconUrl = "jira/assets/svg/jira-logo.svg"
        };

        public void UseStorage(IStorage storage)
        {
        }
    }
}