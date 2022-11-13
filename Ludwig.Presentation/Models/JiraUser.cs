using System.Collections.Generic;

namespace Ludwig.Presentation.Models
{
    public class JiraUser
    {
        public string Self { get; set; }
        
        public string Key { get; set; }
        
        public string Name { get; set; }
        
        public string EmailAddress { get; set; }
        
        public Dictionary<string,string> AvatarUrls { get; set; }
        
        public string DisplayName { get; set; }
        
        public bool Active { get; set; }
        
        public string TimeZone { get; set; }
        
        public string Locale { get; set; }
    }
}