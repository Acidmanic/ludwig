using Ludwig.Common.Configuration;
using Ludwig.IssueManager.Jira.Configuration;

namespace Ludwig.IssueManager.Jira.Interfaces
{
    internal interface IJiraConfigurationProvider : IConfigurationProvider<JiraConfiguration>
    {
    }
}