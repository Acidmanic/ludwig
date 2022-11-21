using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Interfaces;
using Ludwig.IssueManager.Jira.Models;
using IssueType = Ludwig.Contracts.Models.IssueType;

namespace Ludwig.IssueManager.Jira.Adapter
{
    internal class JiraIssueManager:IIssueManager
    {

        private readonly Services.Jira _jira;
        private readonly string _jiraBase;
        
        public JiraIssueManager(IAuthenticator userPassAuth, Services.Jira jira, IJiraConfigurationProvider configurationProvider)
        {
            _jira = jira;

            var jiraBase = configurationProvider.GetConfiguration().JiraFrontChannelUrl;

            if (!jiraBase.EndsWith("/"))
            {
                jiraBase = jiraBase + "/";
            }

            _jiraBase = jiraBase;
            
            Authenticators.Add(userPassAuth);
        }


        private IssueManagerUser Map(JiraUser jiraUser)
        {
            return new IssueManagerUser
            {
                Active = jiraUser.Active,
                Name = jiraUser.Name,
                DisplayName = jiraUser.DisplayName,
                AvatarUrl = jiraUser.AvatarUrls["48x48"],
                EmailAddress = jiraUser.EmailAddress
            };
        }
        
        private Priority Map(JiraPriority jiraPriority)
        {
            return new Priority()
            {
                Name = jiraPriority.Name,
                Value = 0,
            };
        }
        
        private IssueType Map(JiraIssueType jiraIssueType)
        {
            return new IssueType
            {
                Description = jiraIssueType.Description,
                Name = jiraIssueType.Name,
                IconUrl = jiraIssueType.IconUrl
            };
        }
        
        private Issue Map(JiraIssue jiraIssue)
        {
            return new Issue
            {
                Assignee = Map(jiraIssue.Assignee),
                Description = jiraIssue.Description,
                Priority = Map(jiraIssue.Priority),
                Title = jiraIssue.Summary,
                UserStory = jiraIssue.UserStory,
                IssueReferenceLink = _jiraBase + "projects/" + jiraIssue.Project.Key +
                                     "issues/" + jiraIssue.Key,
                IssueType = Map(jiraIssue.IssueType)
            };
        }
        
        
        public async  Task<List<IssueManagerUser>> GetAllUsers()
        {
            var jiraUsers = await _jira.AllUsers();

            var users = jiraUsers.Select(Map).ToList();
            
            return users;
        }

        public async Task<IssueManagerUser> GetCurrentUser()
        {
            var currentUser = await _jira.LoggedInUser();

            if (currentUser)
            {
                return Map(currentUser.Primary);
            }

            return null;
        }

        public async Task<List<Issue>> GetAllIssues()
        {
            var jiraIssues = await _jira.AllIssues();

            var issues = jiraIssues.Select(Map).ToList();

            return issues;
        }

        public async Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            var jiraIssues = await _jira.IssuesByUserStory(userStory);

            var issues = jiraIssues.Select(Map).ToList();

            return issues;
        }

        public List<IAuthenticator> Authenticators { get; } = new List<IAuthenticator>();
    }
}