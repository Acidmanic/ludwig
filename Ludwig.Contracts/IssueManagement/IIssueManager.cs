using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Models;

namespace Ludwig.Contracts.IssueManagement
{
    public interface IIssueManager
    {

        Task<List<IssueManagerUser>> GetAllUsers();
        
        Task<List<Issue>> GetAllIssues();

        Task<List<Issue>> GetIssuesByUserStory(string userStory);
        
        List<IAuthenticator> Authenticators { get; } 

    }
}