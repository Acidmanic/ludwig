using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Extensions;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Authentication
{
    public class SimpleAdministratorAuthenticator : IAuthenticator
    {
        private readonly Persistant<Credentials> _credentialsPersistence = new Persistant<Credentials>();


        public SimpleAdministratorAuthenticator()
        {
            _credentialsPersistence.Load();

            if (string.IsNullOrWhiteSpace(_credentialsPersistence.Value.Username))
            {
                _credentialsPersistence.Value.Username = "Admin";
                _credentialsPersistence.Value.Password = "madmean".ToSh256();

                _credentialsPersistence.Save();
            }
        }

        public Task<AuthenticationResult> Authenticate(Dictionary<string, string> parameters)
        {
            return Task.Run(() =>
            {
                var username = parameters.Read("username");
                var password = parameters.Read("password");

                if (username.HasValue(password))
                {
                    username = username.Trim().ToLower();

                    if (username == _credentialsPersistence.Value.Username.ToLower())
                    {
                        var passwordHash = password.Trim().ToSh256();

                        if (passwordHash == _credentialsPersistence.Value.Password)
                        {
                            return new AuthenticationResult
                            {
                                Authenticated = true,
                                EmailAddress = "",
                                IsAdministrator = true,
                                SubjectId = "administrator",
                                IsIssueManager = false,
                                SubjectWebPage = ""
                            };
                        }
                    }
                }

                return new AuthenticationResult { Authenticated = false };
            });
        }

        public Task<List<RequestUpdate>> GrantAccess()
        {
            return Task.Run(() => new List<RequestUpdate>());
        }

        public LoginMethod LoginMethod { get; } = new LoginMethod
        {
            Description = "Please Use Your Administrator Username, and passwords. " +
                          "If you have not changed it yet, your username would be: 'Admin' " +
                          "and your password would be: 'madmean' ",
            Fields = new List<LoginField>
            {
                LoginField.Username,
                LoginField.Password
            },
            Name = "Administrator",
            Queries = new List<LoginQuery>(),
            ConfigurationRequirements = new List<ConfigurationRequirement>()
        };

        public void UseStorage(IStorage storage)
        {
            // I dont want to!
        }
    }
}