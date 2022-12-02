using Ludwig.Contracts.Configurations;

namespace Ludwig.IssueManager.Jira.Configuration
{
    internal class JiraConfiguration:LudwigConfigurations
    {
        public string JiraBackChannelUrl { get; set; }
        
        public string JiraFrontChannelUrl { get; set; }
        
    }
}