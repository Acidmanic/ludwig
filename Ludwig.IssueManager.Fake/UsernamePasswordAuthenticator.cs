using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Models;

namespace Ludwig.IssueManager.Fake
{
    public class UsernamePasswordAuthenticator:IAuthenticator
    {

        private string _accessCookie = "";
        
        public Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            return Task.Run(() =>
            {
                if (parameters.ContainsKey("Username"))
                {
                    var username = parameters["Username"];

                    if (parameters.ContainsKey("Password"))
                    {
                        var password = parameters["Password"];

                        if (username == FakeIssueManager.User.Name && password == FakeIssueManager.User.Name)
                        {
                            _accessCookie = Guid.NewGuid().ToString();
                            
                            return new AuthenticationResult
                            {
                                Authenticated = true,
                                EmailAddress = FakeIssueManager.User.EmailAddress,
                                SubjectId = FakeIssueManager.User.Name,
                                SubjectWebPage = ""
                            };
                        }
                    }
                }

                return new AuthenticationResult { Authenticated = false };
            });
        }

        public Task<RequestUpdate> GrantAccess(RequestUpdate requestUpdate)
        {
            return Task.Run(() =>
            {
                var update = new RequestUpdate();
                
                update.Cookies.Add("FAKE_AUTH_ID",_accessCookie);

                return update;
            });
        }

        public LoginMethod LoginMethod { get; } = new LoginMethod
        {
            Description = $"Please use {FakeIssueManager.User.Name} as both Username and Password",
            Name = "UsernamePassword",
            Fields = new List<LoginField>
            {
                LoginField.Username,
                LoginField.Password
            }
        };
    }
}