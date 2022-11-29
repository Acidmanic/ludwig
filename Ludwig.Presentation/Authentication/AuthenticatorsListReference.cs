using System.Collections.Generic;
using Ludwig.Contracts.Authentication;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticatorsListReference
    {

        private readonly List<IAuthenticator> _authenticators = new List<IAuthenticator>();
        private readonly Storage.Storage _storage;

        public AuthenticatorsListReference(Storage.Storage storage)
        {
            _storage = storage;
        }

        public IReadOnlyList<IAuthenticator> Authenticators => _authenticators;


        public void UseAuthenticator(IAuthenticator authenticator)
        {
            authenticator.UseStorage(_storage);
            _authenticators.Add(authenticator);
        }
    }
}