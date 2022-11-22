using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;

namespace Ludwig.IssueManager.Fake
{
    public class OtpAuthenticator:IAuthenticator
    {
        public Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            return Task.Run(() =>
            {
                var code = parameters.Read("otp");

                if (code != null && !string.IsNullOrEmpty(code))
                {
                    if (code == Code)
                    {
                        return new AuthenticationResult
                        {
                            Authenticated = true,
                            EmailAddress = FakeIssueManager.User.EmailAddress,
                            SubjectId = FakeIssueManager.User.Name,
                            SubjectWebPage = "/profiles/"+FakeIssueManager.User.Name,
                        };
                    }
                }

                return new AuthenticationResult() { Authenticated = false };
            });
        }

        public Task<RequestUpdate> GrantAccess(RequestUpdate requestUpdate)
        {
            return Task.Run(() => requestUpdate);
        }


        private static readonly LoginMethod Method = new LoginMethod
        {
            Fields = new List<LoginField>
            {
                LoginField.Otp
            },
            Name = "One Time Password",
            
        };

        private static readonly string Code=Guid.NewGuid().ToString().Substring(0, 5);

        public LoginMethod LoginMethod
        {
            get
            {
                Method.Description = $"Please enter {Code} to log in!";

                return Method;
            }
        }
    }
}