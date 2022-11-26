using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Interfaces;

namespace Ludwig.IssueManager.Jira.Adapter
{
    internal class UsernamePasswordAuthenticator:IAuthenticator
    {


        private class GrantRecord
        {
            public string Token { get; set; }
            
            public long Id { get; set; }
        }
        
        private readonly Services.Jira _jira;
        private readonly IJiraConfigurationProvider _configurationProvider;
        private readonly Persistant<GrantRecord> _persistantGrantRecord = new Persistant<GrantRecord>(); 

        public UsernamePasswordAuthenticator(Services.Jira jira, IJiraConfigurationProvider configurationProvider)
        {
            _jira = jira;
            _configurationProvider = configurationProvider;
            _persistantGrantRecord.Load();
        }

        public async Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            
                var username = parameters.Read("Username");
                var password = parameters.Read("Password");
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {

                    var loggedIn = await _jira.LoginByCredentials(username, password);

                    if (loggedIn)
                    {

                        var frontChannel = _configurationProvider.Configuration.JiraFrontChannelUrl.Slashend();
                        
                        _persistantGrantRecord.Value.Token = loggedIn.Secondary;
                        _persistantGrantRecord.Save();
                        return new AuthenticationResult
                        {
                            Authenticated = true,
                            EmailAddress = loggedIn.Primary.EmailAddress,
                            SubjectId = loggedIn.Primary.Name,
                            SubjectWebPage = frontChannel + "secure/ViewProfile.jspa?name=" + loggedIn.Primary.Name
                        };
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
                    Value=_persistantGrantRecord.Value.Token,
                    Type = RequestUpdate.RequestUpdateTypeHeader
                }
            });
        }

        public LoginMethod LoginMethod { get; } = new LoginMethod
        {
            Description = $"Please use your Jira username and password to login. I know it's not secure, but " +
                          $"that is all we can do while using Basic Authorization!",
            Name = "By Jira Credentials",
            TextInputFields = new List<LoginField>
            {
                LoginField.Username,
                LoginField.Password
            }
        };
    }
}