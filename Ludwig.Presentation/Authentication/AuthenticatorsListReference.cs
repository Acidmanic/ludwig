using System.Collections.Generic;
using Ludwig.Contracts.Authentication;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticatorsListReference
    {

        private readonly List<IAuthenticator> _authenticators = new List<IAuthenticator>();

        public IReadOnlyList<IAuthenticator> Authenticators => _authenticators;


        public void UseAuthenticator(IAuthenticator authenticator)
        {
            _authenticators.Add(authenticator);
        }
    }
}