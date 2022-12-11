using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;

namespace Ludwig.Presentation.Administration
{
    public class AdministrationIssueManager : IIssueManager
    {
        public IssueManagerUser AdministratorUser { get; }


        public AdministrationIssueManager(IConfigurationProvider configurationProvider)
        {
            AdministratorUser = new IssueManagerUser
            {
                Active = false,
                Name = "Admin",
                AvatarUrl = configurationProvider.ReadByName(nameof(LudwigConfigurations.LudwigAddress), "")
                            + "presentation-assets/png/admin-profile.png",
                DisplayName = "Ludwig van Beethoven",
                EmailAddress = "",
                UserReferenceLink = "/configurations"
            };
        }


        public Task<List<IssueManagerUser>> GetAllUsers()
        {
            return Task.Run(() => new List<IssueManagerUser>{AdministratorUser});
        }

        public Task<IssueManagerUser> GetCurrentUser()
        {
            return Task.Run(() => AdministratorUser);
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
    }
}