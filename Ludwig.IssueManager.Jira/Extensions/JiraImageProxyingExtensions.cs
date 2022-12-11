using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Extensions
{
    internal  static class JiraImageProxyingExtensions
    {


        internal static string ProxiedAvatarUrl(this JiraUser user)
        {
            var url = "";


            if (user != null && user.AvatarUrls != null)
            {
                var maxSize = 0;
                foreach (var avatarUrl in user.AvatarUrls)
                {
                    var size = GetImageSize(avatarUrl.Key);

                    if (size > maxSize)
                    {
                        maxSize = size;
                        url = avatarUrl.Value;
                    }
                }
            }

            return "jira/image?url=" + url;
        }
        
        internal static string ProxiedIcon(this JiraIssueType issueType)
        {
            var url = "";


            if (issueType != null)
            {
                if (!string.IsNullOrWhiteSpace(issueType.IconUrl))
                {
                    url = issueType.IconUrl;
                }
            }

            return "jira/image?url=" + url;
        }

        private static int GetImageSize(string key)
        {
            if (!string.IsNullOrWhiteSpace(key) && key.Length >= 2)
            {
                var sizeString = key.Substring(0, 2);

                if (int.TryParse(sizeString, out var size))
                {
                    return size;
                }
            }
            return 0;
        }
    }
}