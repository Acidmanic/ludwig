using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EnTier.Results;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Download;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationManager
    {
        private readonly IIssueManager _issueManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStore _authenticationStore;

        public IReadOnlyList<IAuthenticator> Authenticators { get; private set; }
        public IReadOnlyList<LoginMethod> LoginMethods { get; private set; }
        public ReadOnlyDictionary<string, IAuthenticator> AuthenticatorsByMethodName { get; private set; }
        public ReadOnlyDictionary<string, LoginMethod> LoginMethodsByMethodName { get; private set; }


        private Task<RequestUpdate> _grantAccessTask = new Task<RequestUpdate>(() => new RequestUpdate());

        public AuthenticationManager(IIssueManager issueManager, AuthenticationStore authenticationStore,
            IHttpContextAccessor httpContextAccessor)
        {
            _issueManager = issueManager;
            _authenticationStore = authenticationStore;
            _httpContextAccessor = httpContextAccessor;

            var authenticators = new List<IAuthenticator>();
            var logins = new List<LoginMethod>();

            // Add any other authenticators
            AddAdditionalAuthenticators(authenticators);

            authenticators.AddRange(_issueManager.Authenticators);

            var authByName = new Dictionary<string, IAuthenticator>();
            var loginByName = new Dictionary<string, LoginMethod>();

            authenticators.ForEach(a =>
            {
                authByName.Add(a.LoginMethod.Name, a);
                loginByName.Add(a.LoginMethod.Name, a.LoginMethod);
                logins.Add(a.LoginMethod);
            });

            Authenticators = authenticators;
            LoginMethods = logins;
            AuthenticatorsByMethodName = new ReadOnlyDictionary<string, IAuthenticator>(authByName);
            LoginMethodsByMethodName = new ReadOnlyDictionary<string, LoginMethod>(loginByName);
        }

        private void AddAdditionalAuthenticators(List<IAuthenticator> authenticators)
        {
            //
        }


        // set grant access
        public async Task<Result<AuthenticationRecord, string>> Login(LoginParameters parameters)
        {
            // getAuthenticator

            var loginName = parameters.LoginMethodName;

            if (AuthenticatorsByMethodName.ContainsKey(loginName))
            {
                var authenticator = AuthenticatorsByMethodName[loginName];

                var authResult = await authenticator.Authenticate(parameters.Parameters);

                if (authResult.Authenticated)
                {
                    var record = _authenticationStore.GenerateToken(authenticator.LoginMethod.Name, authResult);

                    _grantAccessTask = authenticator.GrantAccess(new RequestUpdate());

                    return new Result<AuthenticationRecord, string>(true, "Success", record);
                }

                return new Result<AuthenticationRecord, string>
                    (false, $"{loginName} Unable to authenticate successfully.", null);
            }

            return new Result<AuthenticationRecord, string>
                (false, $"Could not find login method: {loginName}.", null);
        }


        public async Task GrantBackChannelAccess(PatientDownloader backChannelCarrier)
        {
            var requestUpdates = await _grantAccessTask;

            foreach (var header in requestUpdates.Headers)
            {
                backChannelCarrier.Headers.Add(header.Key, header.Value);
            }

            foreach (var cookie in requestUpdates.Cookies)
            {
                backChannelCarrier.InDirectCookies.Add(cookie.Key, cookie.Value);
            }
        }
        
        
        
        public string ReadAuthorizationToken()
        {
            var context = _httpContextAccessor.HttpContext;

            var header = context.Request.Headers["Authorization"];

            if (!StringValues.IsNullOrEmpty(header))
            {
                foreach (var stringValue in header)
                {
                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        if (stringValue.Trim().ToLower().StartsWith("token"))
                        {
                            var tokenLength = "token".Length;

                            var token = stringValue.Substring(tokenLength, stringValue.Length - tokenLength)
                                .Trim();

                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                return token;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public Result<AuthenticationRecord> IsAuthorized()
        {


            var token = ReadAuthorizationToken();

            if (token != null)
            {
                var foundRecord = _authenticationStore.FindAuthenticatedLoginMethodName(token);

                return foundRecord;
            }
            
            return new Result<AuthenticationRecord>().FailAndDefaultValue();
        }

        public void Revoke()
        {
            var token = ReadAuthorizationToken();

            Revoke(token);
        }
        
        public void Revoke(string token)
        {
            
            if (token != null)
            {
                var foundRecord = _authenticationStore.FindAuthenticatedLoginMethodName(token);

                if (foundRecord)
                {
                    _authenticationStore.RemoveAuthorization(token);
                }
            }
        }
    }
}