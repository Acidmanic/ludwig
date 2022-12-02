using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Common.Configuration;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Configurations;
using Ludwig.IssueManager.Gitlab.Models;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class GitlabIssueManager:IIssueManager
    {


        private readonly IBackChannelRequestGrant _backChannelRequestGrant;
        private readonly string _backChannelUrl;

        public GitlabIssueManager(IBackChannelRequestGrant backChannelRequestGrant,
            IConfigurationProvider configurationProvider)
        {
            _backChannelRequestGrant = backChannelRequestGrant;
            _backChannelUrl = configurationProvider.GetConfiguration<GitlabConfigurations>()
                .GitlabInstanceBackChannel.Slashend();
        }

        public async Task<List<IssueManagerUser>> GetAllUsers()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var users = await downloader.DownloadObject<List<GitlabUser>>
                (_backChannelUrl+"api/v4/users", 400, 3);

            if (users)
            {
                var imUsers = users.Value.Select(Mapper.Map).ToList();

                return imUsers;
            }

            return new List<IssueManagerUser>();
        }

        public async  Task<IssueManagerUser> GetCurrentUser()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var me = await downloader.DownloadObject<GitlabUser>
                (_backChannelUrl+"api/v4/user", 400, 3);

            if (me)
            {
                return Mapper.Map(me.Value);
            }

            return null;
        }

        public async Task<List<Issue>> GetAllIssues()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var gitlabIssues = await downloader.DownloadObject<List<GitlabIssue>>
                (_backChannelUrl+"api/v4/issues", 1000, 3);

            if (gitlabIssues)
            {
                var issues = gitlabIssues.Value.Select(Mapper.Map).ToList();

                return issues;
            }

            return new List<Issue>();
        }

        public  async Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var gitlabIssues = await downloader.DownloadObject<List<GitlabIssue>>
                (_backChannelUrl+"api/v4/search?scope=issues&search=$"+userStory, 1000, 3);

            if (gitlabIssues)
            {
                var issues = gitlabIssues.Value.Select(Mapper.Map).ToList();

                return issues;
            }

            return new List<Issue>();
        }
    }
}