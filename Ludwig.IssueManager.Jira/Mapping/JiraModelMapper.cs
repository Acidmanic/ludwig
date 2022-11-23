using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Extensions;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Mapping
{
    internal class JiraModelMapper
    {


        private readonly string _jiraBase ;

        public JiraModelMapper(string jiraBase)
        {
            _jiraBase = jiraBase;
        }

        public IssueManagerUser Map(JiraUser jiraUser)
        {
            if (jiraUser == null)
            {
                return null;
            }
            
            return new IssueManagerUser
            {
                Active = jiraUser.Active,
                Name = jiraUser.Name,
                DisplayName = jiraUser.DisplayName,
                AvatarUrl = jiraUser.ProxiedAvatarUrl(),
                EmailAddress = jiraUser.EmailAddress,
                UserReferenceLink = _jiraBase+"secure/ViewProfile.jspa?name="+jiraUser.Name
            };
        }
        
        public Priority Map(JiraPriority jiraPriority)
        {
            if (jiraPriority == null)
            {
                return null;
            }
            return new Priority()
            {
                Name = jiraPriority.Name,
                Value = 0,
            };
        }
        
        public IssueType Map(JiraIssueType jiraIssueType)
        {
            if (jiraIssueType == null)
            {
                return null;
            }
            return new IssueType
            {
                Description = jiraIssueType.Description,
                Name = jiraIssueType.Name,
                IconUrl = jiraIssueType.ProxiedIcon()
            };
        }
        
        public Issue Map(JiraIssue jiraIssue)
        {
            if (jiraIssue == null)
            {
                return null;
            }
            return new Issue
            {
                Assignee = Map(jiraIssue.Assignee),
                Description = jiraIssue.Description,
                Priority = Map(jiraIssue.Priority),
                Title = jiraIssue.Summary,
                UserStory = jiraIssue.UserStory,
                IssueReferenceLink = _jiraBase + "projects/" + jiraIssue.Project.Key +
                                     "/issues/" + jiraIssue.Key,
                IssueType = Map(jiraIssue.IssueType)
            };
        }
    }
}