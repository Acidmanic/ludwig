using Newtonsoft.Json;

namespace Ludwig.IssueManager.Gitlab.Models
{
    public class PostingIssue
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        
        public string Description { get; set; }
        
        public string Title { get; set; }
        
        public string Labels { get; set; }
    }
}