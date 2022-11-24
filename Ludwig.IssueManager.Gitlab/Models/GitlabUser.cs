using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Ludwig.IssueManager.Gitlab.Models
{
    public class GitlabUser
    {
        public string Name { get; set; }
        
        public string Username { get; set; }
        
        public string State { get; set; }
        
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonProperty("web_url")]
        public string WebUrl { get; set; }
        
        public string Email { get; set; }
        
        
        
    }
}