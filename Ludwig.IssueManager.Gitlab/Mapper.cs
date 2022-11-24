using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Extensions;
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
                UserReferenceLink = user.WebUrl,
                EmailAddress = user.Email
            };
        }


        public static Issue Map(GitlabIssue issue)
        {
            if (issue == null)
            {
                return null;
            }

            return new Issue
            {
                Assignee = Map(issue.Author),
                Description = issue.Description,
                Priority = Priority.Medium,
                Title = issue.Title,
                UserStory = issue.Description.ExtractStory(),
                IssueType = new IssueType
                {
                    Description = "Gitlab Issue",
                    Name = "Issue",
                    IconUrl = "" // TODO
                },
                IssueReferenceLink = issue.WebUrl
            };
        }
    }
}