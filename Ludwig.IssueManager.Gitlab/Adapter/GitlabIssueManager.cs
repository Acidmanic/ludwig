using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Gitlab.Models;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class GitlabIssueManager:IIssueManager
    {


        private readonly IBackChannelRequestGrant _backChannelRequestGrant;

        public GitlabIssueManager(IBackChannelRequestGrant backChannelRequestGrant)
        {
            _backChannelRequestGrant = backChannelRequestGrant;
        }

        public async Task<List<IssueManagerUser>> GetAllUsers()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var users = await downloader.DownloadObject<List<GitlabUser>>
                ("api/v4/users", 400, 3);

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
                ("api/v4/user", 400, 3);

            if (me)
            {
                return Mapper.Map(me.Value);
            }

            return null;
        }

        public Task<List<Issue>> GetAllIssues()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            throw new System.NotImplementedException();
        }
    }
}