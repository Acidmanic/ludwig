using System;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.Presentation.Authentication
{
    public class AuthorizationToken
    {
        
        [UniqueMember]
        public string Token { get; set; }
        
        
        public long ExpirationEpoch { get; set; }
        
        public string LoginMethodName { get; set; }
        
    }
}