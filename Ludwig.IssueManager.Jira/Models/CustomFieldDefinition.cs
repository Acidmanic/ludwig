using System;

namespace Ludwig.IssueManager.Jira.Models
{
    internal class CustomFieldDefinition
    {
        public string FieldName { get; set; }

        public Type Type { get; set; }

        public Action<JiraIssue, object> SetIntoIssue { get; set; } = (i, o) => { };
    }
}