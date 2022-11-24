using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Models;

namespace Ludwig.IssueManager.Gitlab
{
    public class Mapper
    {
        public static IssueManagerUser Map(GitlabUser user)
        {
            if (user == null)
            {
                return null;
            }
            return new IssueManagerUser
            {
                Active = "active".Equals(user.State?.ToLower()),
                Name = user.Username,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.Name,
                UserReferenceLink = user.WebUrl
            };
        }
    }
}