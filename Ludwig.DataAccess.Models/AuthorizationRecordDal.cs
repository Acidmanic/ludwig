using System.Collections.Generic;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.DataAccess.Models
{
    [OwnerName("AuthorizationRecords")]
    public class AuthorizationRecordDal
    {
        
        [UniqueMember]
        [AutoValuedMember]
        public long Id { get; set; }
        
        [UniqueMember]
        public string Token { get; set; }
        
        public long ExpirationEpoch { get; set; }
        
        public string LoginMethodName { get; set; }
        
        public bool IsAdministrator { get; set; }
        
        public bool IsIssueManager { get; set; }
        
        public string SubjectId { get; set; }

        public string EmailAddress { get; set; }

        public string SubjectWebPage { get; set; }
        
        public string Cookie { get; set; }
        
        public List<RequestUpdateDal> BackChannelGrantAccessUpdates { get; set; }

        public string RequestOrigin { get; set; }
    }
}