using System.Collections.Generic;
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
        
        public JiraUser Map(IssueManagerUser user)
        {
            if (user == null)
            {
                return null;
            }
            
            return new JiraUser
            {
                Active = user.Active,
                Name = user.Name,
                DisplayName = user.DisplayName,
                EmailAddress = user.EmailAddress,
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
        
        public JiraPriority Map(Priority priority)
        {
            if (priority == null)
            {
                return null;
            }
            return new JiraPriority()
            {
                Name = priority.Name,
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
        
        public JiraIssueType Map(IssueType issueType)
        {
            if (issueType == null)
            {
                return null;
            }
            return new JiraIssueType
            {
                Description = issueType.Description,
                Name = issueType.Name,
            };
        }
        
        public Issue Map(JiraIssue jiraIssue)
        {
            if (jiraIssue == null)
            {
                return null;
            }

            List<IssueManagerUser> assignees = new List<IssueManagerUser>();

            if (jiraIssue.Assignee != null)
            {
                assignees.Add(Map(jiraIssue.Assignee));
            }

            return new Issue
            {
                Assignees = assignees,
                Description = jiraIssue.Description,
                Priority = Map(jiraIssue.Priority),
                Title = jiraIssue.Summary,
                UserStory = jiraIssue.UserStory,
                IssueReferenceLink = _jiraBase + "projects/" + jiraIssue.Project.Key +
                                     "/issues/" + jiraIssue.Key,
                IssueType = Map(jiraIssue.IssueType)
            };
        }
        
        public JiraIssue Map(Issue issue)
        {
            if (issue == null)
            {
                return null;
            }

            JiraUser assignee = null;
            
            if (issue.Assignees != null && issue.Assignees.Count>0)
            {
                assignee = Map(issue.Assignees[0]);
            }

            return new JiraIssue
            {
                Assignee = assignee,
                Description = issue.Description,
                Priority = Map(issue.Priority),
                Summary = issue.Title,
                UserStory = issue.UserStory,
                IssueType = Map(issue.IssueType)
            };
        }
    }
}