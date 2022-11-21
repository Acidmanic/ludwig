using System.Collections.Generic;

namespace Ludwig.IssueManager.Jira.Models
{
    internal class JiraIssue
    {
        public string Id { get; set; }

        public string Self { get; set; }

        public string Key { get; set; }

        public Dictionary<string, object> Fields { get; set; }


        public JiraIssueType IssueType { get; set; }

        public string Summary { get; set; }


        public string Description { get; set; }

        public JiraProject Project { get; set; }


        public JiraPriority Priority { get; set; }

        public JiraUser Assignee { get; set; }

        public string UserStory { get; set; }
    }
}