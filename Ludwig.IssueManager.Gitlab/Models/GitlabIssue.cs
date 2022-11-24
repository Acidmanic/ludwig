using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ludwig.IssueManager.Gitlab.Models
{
    public class GitlabIssue
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public List<GitlabUser> Assignees { get; set; }

        public GitlabUser Author { get; set; }
        
        
        [JsonPropertyName("web_url")]
        public string WebUrl { get; set; }

    }
}
