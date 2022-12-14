using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Configuration;

namespace Ludwig.IssueManager.Jira.Adapter
{
    internal class JiraByCredentials:IAuthenticator
    {


        private class GrantRecord
        {
            public string Token { get; set; }
            
            public long Id { get; set; }
        }
        
        private readonly Services.Jira.Jira _jira;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Persistant<GrantRecord> _persistantGrantRecord = new Persistant<GrantRecord>(); 

        public JiraByCredentials(Services.Jira.Jira jira, IConfigurationProvider configurationProvider)
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

                        var frontChannel = _configurationProvider.GetConfiguration<JiraConfiguration>()
                            .JiraFrontChannelUrl.Slashend();
                        
                        _persistantGrantRecord.Value.Token = loggedIn.Secondary;
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
            Name = "Jira - By Credentials",
            Fields = new List<LoginField>
            {
                LoginField.Username,
                LoginField.Password
            },
            ConfigurationRequirements = new List<ConfigurationRequirement>
            {
              new ConfigurationRequirement
              {
                  Description = "The url which you can access your jira instance with.",
                  ConfigurationName = nameof(JiraConfiguration.JiraFrontChannelUrl),
                  DisplayName = "Jira Address"
              },
              new ConfigurationRequirement
              {
                  Description = "If your ludwig installation server has access locally on the network " +
                                "to your jira instance, you can have ludwig communicating with jira using that address." +
                                "Otherwise just put the same url as jira access.",
                  ConfigurationName = nameof(JiraConfiguration.JiraBackChannelUrl),
                  DisplayName = "Jira Local Access"
              },
            },
            IconUrl = "jira/assets/svg/jira-logo.svg"
        };

        public void UseStorage(IStorage storage)
        {
        }
    }
}