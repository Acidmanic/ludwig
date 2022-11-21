using System.Collections.Generic;

namespace Ludwig.IssueManager.Jira.Models
{
    internal class JiraIssueChunk
    {
        public string Expand { get; set; }

        public int StartAt { get; set; }

        public int MaxResults { get; set; }

        public int Total { get; set; }

        public List<JiraIssue> Issues { get; set; }
    }
}