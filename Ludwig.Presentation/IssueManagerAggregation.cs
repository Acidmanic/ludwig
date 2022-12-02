using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Authentication;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation
{
    public class IssueManagerAggregation:IIssueManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly Dictionary<string, IIssueManager> _issueManagersByLoginMethodName =
            new Dictionary<string, IIssueManager>();


        public IssueManagerAggregation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        
        public void Register(IIssueManager issueManager, IAuthenticator authenticator)
        {
            var loginMethodName = authenticator.LoginMethod.Name;

            if (!_issueManagersByLoginMethodName.ContainsKey(loginMethodName))
            {
                _issueManagersByLoginMethodName.Add(loginMethodName, issueManager);
            }
            else
            {
                //TODO: log
            }
        }


        public IIssueManager CurrentIssueManager
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                
                var loginMethodName = context.User.Claims.
                    FirstOrDefault(p=>p.Type == LudwigClaims.LoginMethod)?.Value;

                if (!string.IsNullOrWhiteSpace(loginMethodName))
                {
                    if (_issueManagersByLoginMethodName.ContainsKey(loginMethodName))
                    {
                        return _issueManagersByLoginMethodName[loginMethodName];
                    }
                }
                //TODO: Replace NullObject
                return null;
            }
        }

        public Task<List<IssueManagerUser>> GetAllUsers()
        {
            return CurrentIssueManager.GetAllUsers();
        }

        public Task<IssueManagerUser> GetCurrentUser()
        {
            return CurrentIssueManager.GetCurrentUser();
        }

        public Task<List<Issue>> GetAllIssues()
        {
            return CurrentIssueManager.GetAllIssues();
        }

        public Task<List<Issue>> GetIssuesByUserStory(string userStory)
        {
            return CurrentIssueManager.GetIssuesByUserStory(userStory);
        }
    }
}