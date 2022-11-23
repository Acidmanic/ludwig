using Ludwig.Common.Configuration;
using Ludwig.IssueManager.Jira.Interfaces;

namespace Ludwig.IssueManager.Jira.Configuration
{
    internal class JiraConfigurationProvider : LudwigConfigurationProvider<JiraConfiguration>,
        IJiraConfigurationProvider
    {
        protected override void OnFirstWrite(JiraConfiguration configuration)
        {
            configuration.JiraBackChannelUrl = "http://litbid.ir:8888/";
            configuration.JiraFrontChannelUrl = "http://litbid.ir:8888/";
        }
    }
}