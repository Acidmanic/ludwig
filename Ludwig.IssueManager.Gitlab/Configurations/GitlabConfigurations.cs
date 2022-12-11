using System.Globalization;
using Ludwig.Common.Configuration;
using Ludwig.Contracts.Configurations;

namespace Ludwig.IssueManager.Gitlab.Configurations
{
    public class GitlabConfigurations:LudwigConfigurations
    {
        public string GitlabInstanceBackChannel { get; set; }
        
        public string GitlabInstanceFrontChannel { get; set; }
        
        public string ClientId { get; set; }
        
        public string ClientSecret { get; set; }
        
        public string GitlabProjectId { get; set; }

    }
}