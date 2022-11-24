using System.Globalization;
using Ludwig.Common.Configuration;

namespace Ludwig.IssueManager.Gitlab.Configurations
{
    public class GitlabConfigurations
    {
        public string GitlabInstanceBackChannel { get; set; }
        
        public string GitlabInstanceFrontChannel { get; set; }
        
        public string ClientId { get; set; }
        
        public string ClientSecret { get; set; }
    }
}