using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Configuration;
using Ludwig.IssueManager.Jira.Interfaces;
using Ludwig.IssueManager.Jira.Mapping;

namespace Ludwig.IssueManager.Jira.Adapter
{
    internal class JiraIssueManager : IIssueManager
    {
        private readonly Services.Jira.Jira _jira;
        private readonly JiraModelMapper _mapper;

        public JiraIssueManager(Services.Jira.Jira jira, IConfigurationProvider configurationProvider)
        {
            _jira = jira;

            var jiraBase = configurationProvider.GetConfiguration<JiraConfiguration>().JiraFrontChannelUrl.Slashend();
            
            _mapper = new JiraModelMapper(jiraBase);
        }


        public async Task<List<IssueManagerUser>> GetAllUsers()
        {
            var jiraUsers = await _jira.AllUsers();

            var users = jiraUsers.Select(_mapper.Map).ToList();

            return users;
        }

        public async Task<IssueManagerUser> GetCurrentUser()
        {
            var currentUser = await _jira.LoggedInUser();

            if (currentUser)
            {
                return _mapper.Map(currentUser.Primary);
            }

            return null;
        }

        public async Task<List<Issue>> GetAllIssues()
        {
            var jiraIssues = await _jira.AllIssues();

            var issues = jiraIssues.Select(_mapper.Map).ToList();

            return issues;
        }

        public async Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            var jiraIssues = await _jira.IssuesByUserStory(userStory);

            var issues = jiraIssues.Select(_mapper.Map).ToList();

            return issues;
        }
    }
}