using Ludwig.Common.Configuration;
using Ludwig.IssueManager.Jira.Interfaces;

namespace Ludwig.IssueManager.Jira.Configuration
{
    internal class JiraConfigurationProvider : LudwigConfigurationProvider<JiraConfiguration>,
        IJiraConfigurationProvider
    {
    }
}