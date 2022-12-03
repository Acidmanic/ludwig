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


        public static Issue Map(GitlabIssue value)
        {
            if (value == null)
            {
                return null;
            }

            List<IssueManagerUser> assignees = new List<IssueManagerUser>();

            if (value.Assignees != null)
            {
                assignees.AddRange(value.Assignees.Select(Map));
            }

            var issue =  new Issue
            {
                Assignees = assignees,
                Description = value.Description,
                Priority = Priority.Medium,
                Title = value.Title,
                UserStory = "Story",
                IssueType = new IssueType
                {
                    Description = "Gitlab Issue",
                    Name = "Issue",
                    IconUrl = "gitlab-images/svg/task.svg"
                },
                IssueReferenceLink = value.WebUrl
            };

            value.Description.UpdateStoryAndDescription(issue);
            
            return issue;
        }
    }
}