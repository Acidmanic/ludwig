using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts;
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
                            IsAdministrator = true,
                            IsIssueManager = true
                        };
                    }
                }

                return new AuthenticationResult() { Authenticated = false };
            });
        }

        public Task<List<RequestUpdate>> GrantAccess()
        {
            return Task.Run(() => new List<RequestUpdate>
            {
                new RequestUpdate
                {
                    Key = "OTP_SESSION_ID",
                    Value = Code
                }
            });
        }


        private static readonly LoginMethod Method = new LoginMethod
        {
            Fields = new List<LoginField>
            {
                LoginField.Otp
            },
            Name = "One Time Password",
            IconUrl = "fake-images/svg/sad-rect.svg"
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

        public void UseStorage(IStorage storage)
        {
            
        }
    }
}