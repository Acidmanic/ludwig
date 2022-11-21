using System;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationRecord : AuthenticationToken
    {
        public string SubjectId { get; set; }

        public string EmailAddress { get; set; }

        public string SubjectWebPage { get; set; }


        [UniqueMember]
        public string Id
        {
            get
            {
                return Token;
            } set
            {
                Token = value;
            }
        }

        public AuthenticationToken AsToken()
        {
            return new AuthenticationToken
            {
                Token = Id,
                ExpirationEpoch = ExpirationEpoch,
                LoginMethodName = LoginMethodName
            };
        }
    }
}