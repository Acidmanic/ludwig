using Newtonsoft.Json;

namespace Ludwig.IssueManager.Gitlab.Models
{
    public class GitlabToken
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("id_token")] public string IdToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }
        [JsonProperty("created_at")] public long CreatedAt { get; set; }
    }
}