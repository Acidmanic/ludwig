using System;
using System.Collections.Generic;
using System.Security.Claims;
using Acidmanic.Utilities.Reflection.Attributes;
using Ludwig.Contracts.Models;

namespace Ludwig.Presentation.Authentication
{
    public class AuthorizationRecord : AuthorizationToken
    {
        public string SubjectId { get; set; }

        public string EmailAddress { get; set; }

        public string SubjectWebPage { get; set; }
        
        public bool IsIssueManager { get; set; } 
        
        public bool IsAdministrator { get; set; }

        public string Cookie { get; set; }
        
        public List<RequestUpdate> BackChannelGrantAccessUpdates { get; set; }
        
        
        [UniqueMember]
        [AutoValuedMember]
        public long Id { get; set; }

        public string RequestOrigin { get; set; }

        public AuthorizationToken AsToken()
        {
            return new AuthorizationToken
            {
                Token = Token,
                ExpirationEpoch = ExpirationEpoch,
                LoginMethodName = LoginMethodName
            };
        }
    }
}