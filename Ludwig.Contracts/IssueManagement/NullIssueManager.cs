using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts.IssueManagement
{
    public class NullIssueManager : IIssueManager
    {
        public static IssueManagerUser NullUser { get; } = new IssueManagerUser
        {
            Active = false,
            Name = "No one",
            AvatarUrl = "",
            DisplayName = NullUserProfileImage.Url,
            EmailAddress = "null@user.mail",
            UserReferenceLink = ""
        };

        public Task<List<IssueManagerUser>> GetAllUsers()
        {
            return Task.Run(() => new List<IssueManagerUser>());
        }

        public Task<IssueManagerUser> GetCurrentUser()
        {
            return Task.Run(() => NullUser);
        }

        public Task<List<Issue>> GetAllIssues()
        {
            return Task.Run(() => new List<Issue>());
        }

        public Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            return Task.Run(() => new List<Issue>());
        }

        public Task<Issue> AddIssue(Issue issue)
        {
            return Task.Run(() => (Issue)null);
        }

        
        private static object _locker = new object();
        private static NullIssueManager _instance = null;
        private NullIssueManager()
        {
        }

        public static NullIssueManager Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new NullIssueManager();
                    }

                    return _instance;
                }
            }
        }
    }
}