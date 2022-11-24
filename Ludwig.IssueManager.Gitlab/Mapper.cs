using System.Collections.Generic;
using System.Linq;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Extensions;
using Ludwig.IssueManager.Gitlab.Models;

namespace Ludwig.IssueManager.Gitlab
{
    public static class Mapper
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

            List<IssueManagerUser> assignees = new List<IssueManagerUser>();

            if (issue.Assignees != null)
            {
                assignees.AddRange(issue.Assignees.Select(Map));
            }

            return new Issue
            {
                Assignees = assignees,
                Description = issue.Description,
                Priority = Priority.Medium,
                Title = issue.Title,
                UserStory = issue.Description.ExtractStory(),
                IssueType = new IssueType
                {
                    Description = "Gitlab Issue",
                    Name = "Issue",
                    IconUrl = "images/svg/task.svg"
                },
                IssueReferenceLink = issue.WebUrl
            };
        }
    }
}