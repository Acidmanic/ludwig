using System;
using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{
    public class IssueManagerUser
    {
        public string Name { get; set; }
        
        public string DisplayName { get; set; }
        
        public string EmailAddress { get; set; }
        
        public bool Active { get; set; }
        
        public string AvatarUrl { get; set; }
        
        public string UserReferenceLink { get; set; }
    }
}