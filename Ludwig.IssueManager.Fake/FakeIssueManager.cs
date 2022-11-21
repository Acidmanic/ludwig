using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acidmanic.Utilities.Results;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;

namespace Ludwig.IssueManager.Fake
{
    public class FakeIssueManager : IIssueManager
    {
        public static IssueManagerUser User { get; } = new IssueManagerUser();

        private static IssueManagerUser TheUser { get; } = new IssueManagerUser
        {
            Active = true,
            Name = "fake.user",
            AvatarUrl = "",
            DisplayName = "Fake User",
            EmailAddress = "fake@ludwig.user"
        };

        public static Result<IssueManagerUser> LoggedInUser { get; private set; }
            = new Result<IssueManagerUser>().FailAndDefaultValue();

        public static List<Issue> Issues { get; }= new List<Issue>
        {
            new Issue
            {
                Assignee = TheUser,
                Description = "This is a fake issue.",
                Priority = Priority.Medium,
                Title = "Implement Real Issue Management",
                IssueType = new IssueType
                {
                    Description = "A Regular Backlog Task",
                    Name = "Task",
                    IconUrl = ""
                },
                UserStory = "Implement Manager",
                IssueReferenceLink = ""
            },
            new Issue
            {
                Assignee = TheUser,
                Description = "This is another fake issue.",
                Priority = Priority.High,
                Title = "Implement Authorization For Issue Manager",
                IssueType = new IssueType
                {
                    Description = "A Regular Backlog Task",
                    Name = "Task",
                    IconUrl = ""
                },
                UserStory = "Implement Authentication",
                IssueReferenceLink = ""
            }
        };

        public Task<List<IssueManagerUser>> GetAllUsers()
        {
            return Task.Run(() => { return new List<IssueManagerUser> { User }; });
        }

        public Task<IssueManagerUser> GetCurrentUser()
        {
            return Task.Run(() =>
            {
                if (LoggedInUser)
                {
                    return LoggedInUser.Value;
                }

                return null;
            });
        }

        public Task<List<Issue>> GetAllIssues()
        {
            return Task.Run(() => Issues);
        }

        public Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            return Task.Run(() =>
            {
                return Issues.Where(i => i.Title == userStory).ToList();
            });
        }

        public List<IAuthenticator> Authenticators { get; } =
            new List<IAuthenticator> { new UsernamePasswordAuthenticator() };
    }
}