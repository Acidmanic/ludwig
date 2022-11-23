using System;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.Presentation.Authentication
{
    public class AuthenticationRecord : AuthenticationToken
    {
        public string SubjectId { get; set; }

        public string EmailAddress { get; set; }

        public string SubjectWebPage { get; set; }

        public string Cookie { get; set; }

        [UniqueMember]
        [AutoValuedMember]
        public long Id { get; set; }

        public string RequestOrigin { get; set; }

        public AuthenticationToken AsToken()
        {
            return new AuthenticationToken
            {
                Token = Token,
                ExpirationEpoch = ExpirationEpoch,
                LoginMethodName = LoginMethodName
            };
        }
    }
}