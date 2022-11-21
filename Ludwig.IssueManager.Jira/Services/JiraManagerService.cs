using System.Collections.Generic;
using System.Linq;
using Acidmanic.Utilities.Results;
using Ludwig.IssueManager.Jira.Interfaces;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Services
{
    internal class JiraManagerService : IJiraManagerService
    {
        private List<JiraField> _availableFields = new List<JiraField>();
        private readonly Jira _jira;
        private Result<JiraField> _userStoryField = new Result<JiraField>().FailAndDefaultValue();

        public JiraManagerService(Jira jira)
        {
            _jira = jira;
            UpdateFields();
        }

        public List<JiraIssue> GetAllIssuesByUserStory(string storyName)
        {
            if (_userStoryField)
            {
            }

            return new List<JiraIssue>();
        }

        private void UpdateFields()
        {
            _availableFields = _jira.AllFields().Result;

            var usField = _availableFields.FirstOrDefault(f => f.Name?.ToLower() == "user story");

            _userStoryField = new Result<JiraField>(usField != null, usField);
        }
    }
}