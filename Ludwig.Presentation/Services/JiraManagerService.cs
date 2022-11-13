using System.Collections.Generic;
using System.Linq;
using EnTier.Results;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Services
{
    public class JiraManagerService:IJiraManagerService
    {

        private List<JiraField> _availableFields = new List<JiraField>();
        private readonly Jira _jira;
        private Result<JiraField> _userStoryField  = new Result<JiraField>().FailAndDefaultValue();
        
        public JiraManagerService(Jira jira)
        {
            _jira = jira;
        }
        
        public List<JiraIssue> GetAllIssuesByUserStory(string storyName)
        {
            if (_userStoryField)
            {
                   
            }
            return new List<JiraIssue>();
        }

        public IJiraManagerService UseContext(HttpContext context)
        {
            _jira.UseContext(context);
            
            _availableFields = _jira.AllFields().Result;

            var usField = _availableFields.FirstOrDefault(f => f.Name?.ToLower() == "user story");

            _userStoryField = new Result<JiraField>(usField != null, usField);
            
            return this;
        }
    }
}