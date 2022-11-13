using System.Collections.Generic;

namespace Ludwig.Presentation.Models
{
    public class JiraProject
    {
     
        public string Self { get; set; }

        public string Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string ProjectTypeKey { get; set; }

        public Dictionary<string, string> AvatarUrls { get; set; }
    }
}