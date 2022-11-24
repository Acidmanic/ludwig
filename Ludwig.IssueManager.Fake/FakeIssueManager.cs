using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;

namespace Ludwig.IssueManager.Fake
{
    public class FakeIssueManager : IIssueManager
    {


        private readonly IBackChannelRequestGrant _backChannelRequestGrant;

        public FakeIssueManager(IBackChannelRequestGrant backChannelRequestGrant)
        {
            _backChannelRequestGrant = backChannelRequestGrant;
        }


        public static IssueManagerUser User { get; } = new IssueManagerUser
        {
            Active = true,
            Name = "fake.user",
            AvatarUrl = "http://localhost:13801/images/profile",
            DisplayName = "Fake User",
            EmailAddress = "fake@ludwig.user",
            UserReferenceLink = "users/fake-user"
        };

        public static List<Issue> Issues { get; }= new List<Issue>
        {
            new Issue
            {
                Assignees = new List<IssueManagerUser>{User},
                Description = "This is a fake issue.",
                Priority = Priority.Medium,
                Title = "Implement Real Issue Management",
                IssueType = new IssueType
                {
                    Description = "A Regular Backlog Task",
                    Name = "Task",
                    IconUrl = "http://localhost:13801/images/icon/task"
                },
                UserStory = "Implement Manager",
                IssueReferenceLink = "issues/fake-issue"
            },
            new Issue
            {
                Assignees = new List<IssueManagerUser>{User},
                Description = "This is another fake issue.",
                Priority = Priority.High,
                Title = "Implement Authorization For Issue Manager",
                IssueType = new IssueType
                {
                    Description = "A Regular Backlog Task",
                    Name = "Task",
                    IconUrl = "http://localhost:13801/images/icon/task"
                },
                UserStory = "Implement Authentication",
                IssueReferenceLink = "issues/fake-issue"
            }
        };



        private void LogGrant(string label)
        {
            var updates = _backChannelRequestGrant.GetGrantRequestUpdates();

            Console.WriteLine($"accessing {label}, using grant updates:");
            
            foreach (var update in updates)
            {
                Console.WriteLine($"{update.Key}:{update.Value} - {update.Type}");
            }
        }
        
        
        public Task<List<IssueManagerUser>> GetAllUsers()
        {
            LogGrant("All Users");
            
            return Task.Run(() => { return new List<IssueManagerUser> { User }; });
        }

        public Task<IssueManagerUser> GetCurrentUser()
        {
            LogGrant("Current User");
            return Task.Run(() =>
            {
                return User;
            });
        }

        public Task<List<Issue>> GetAllIssues()
        {
            LogGrant("All Issues");
            return Task.Run(() => Issues);
        }

        public Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            LogGrant("IssuesByStory");
            return Task.Run(() =>
            {
                return Issues.Where(i => i.UserStory == userStory).ToList();
            });
        }

    }
}