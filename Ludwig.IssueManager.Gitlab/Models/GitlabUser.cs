using System.Text.Json.Serialization;

namespace Ludwig.IssueManager.Gitlab.Models
{
    public class GitlabUser
    {
        public string Name { get; set; }
        
        public string Username { get; set; }
        
        public string State { get; set; }
        
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonPropertyName("web_url")]
        public string WebUrl { get; set; }
        
        
    }
}