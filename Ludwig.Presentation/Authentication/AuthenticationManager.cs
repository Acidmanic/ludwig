using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EnTier.Results;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Administration;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationManager : IBackChannelRequestGrant
    {
        public static readonly string CookieAuthorizationField = "Ludwig.Session";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStore _authenticationStore;

        public IReadOnlyList<IAuthenticator> Authenticators { get; private set; }
        public IReadOnlyList<LoginMethod> LoginMethods { get; private set; }
        public ReadOnlyDictionary<string, IAuthenticator> AuthenticatorsByMethodName { get; private set; }
        public ReadOnlyDictionary<string, LoginMethod> LoginMethodsByMethodName { get; private set; }


        public AuthenticationManager(
            AuthenticatorsListReference authenticatorsListReference,
            AuthenticationStore authenticationStore,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticationStore = authenticationStore;
            _httpContextAccessor = httpContextAccessor;

            var authenticators = new List<IAuthenticator>();
            var logins = new List<LoginMethod>();


            authenticators.AddRange(authenticatorsListReference.Authenticators);

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

        // set grant access
        public async Task<Result<AuthorizationRecord, string>> Login(LoginParameters parameters)
        {
            // getAuthenticator

            var loginName = parameters.LoginMethodName;

            if (AuthenticatorsByMethodName.ContainsKey(loginName))
            {
                var authenticator = AuthenticatorsByMethodName[loginName];

                var authResult = await authenticator.Authenticate(parameters.Parameters);

                if (authResult.Authenticated)
                {
                    var requestOrigin = _httpContextAccessor.HttpContext.Request.Host.Host;

                    var updates = await authenticator.GrantAccess();

                    var record = _authenticationStore.GenerateToken(
                        authenticator.LoginMethod.Name, authResult, requestOrigin, updates);

                    return new Result<AuthorizationRecord, string>(true, "Success", record);
                }

                return new Result<AuthorizationRecord, string>
                    (false, $"{loginName} Unable to authenticate successfully.", null);
            }

            return new Result<AuthorizationRecord, string>
                (false, $"Could not find login method: {loginName}.", null);
        }


        private string ReadAuthorizationCookie()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context.Request.Cookies.TryGetValue(CookieAuthorizationField, out var cookie))
            {
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    return cookie;
                }
            }

            return null;
        }

        private string ReadAuthorizationToken()
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

        public Result<AuthorizationRecord> IsAuthorized()
        {
            var foundRecord = new Result<AuthorizationRecord>().FailAndDefaultValue();

            var token = ReadAuthorizationToken();

            if (token != null)
            {
                foundRecord = _authenticationStore.IsTokenRegistered(token);
            }

            if (!foundRecord)
            {
                var cookie = ReadAuthorizationCookie();

                if (cookie != null)
                {
                    foundRecord = _authenticationStore.IsCookieRegistered(cookie);
                }
            }

            if (foundRecord)
            {
                foundRecord = VerifyRecord(foundRecord.Value);
            }

            return foundRecord;
        }

        private Result<AuthorizationRecord> VerifyRecord(AuthorizationRecord record)
        {
            if (record != null)
            {
                var nowEpoch = DateTime.Now.Ticks;

                if (record.ExpirationEpoch > nowEpoch)
                {
                    var host = _httpContextAccessor.HttpContext.Request.Host.Host.ToLower();

                    if (!string.IsNullOrWhiteSpace(record.RequestOrigin))
                    {
                        if (record.RequestOrigin.ToLower() == host)
                        {
                            return new Result<AuthorizationRecord>(true, record);
                        }
                    }
                }
            }

            return new Result<AuthorizationRecord>().FailAndDefaultValue();
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
                var foundRecord = _authenticationStore.IsTokenRegistered(token);

                if (foundRecord)
                {
                    _authenticationStore.RemoveAuthorization(token);
                }
            }
        }

        public List<RequestUpdate> GetGrantRequestUpdates()
        {
            var authorized = IsAuthorized();

            if (authorized)
            {
                return authorized.Value.BackChannelGrantAccessUpdates;
            }

            return new List<RequestUpdate>();
        }
    }
}