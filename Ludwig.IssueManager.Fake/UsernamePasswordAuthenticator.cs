using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
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

                var username = parameters.Read("Username");
                var password = parameters.Read("Password");

                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    if (username == FakeIssueManager.User.Name && password == FakeIssueManager.User.Name)
                    {
                        _accessCookie = Guid.NewGuid().ToString();
                            
                        return new AuthenticationResult
                        {
                            Authenticated = true,
                            EmailAddress = FakeIssueManager.User.EmailAddress,
                            SubjectId = FakeIssueManager.User.Name,
                            SubjectWebPage = "",
                            IsAdministrator = false,
                            IsIssueManager = true
                        };
                    }
                }
                
                return new AuthenticationResult { Authenticated = false };
            });
        }

        public Task<List<RequestUpdate>> GrantAccess()
        {
            return Task.Run(() =>
            {
                return new List<RequestUpdate>
                {
                    new RequestUpdate
                    {
                        Key = "HEADER_SESSION_ID",
                        Value = _accessCookie,
                        Type = RequestUpdate.RequestUpdateTypeCookie
                    }
                };
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

        public void UseStorage(IStorage storage)
        {
        }
    }
}