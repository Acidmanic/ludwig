using System.Collections.Generic;
using System.Threading.Tasks;
using Ludwig.Contracts.IssueManagement.Models;

namespace Ludwig.Contracts.IssueManagement
{
    public interface IIssueManager
    {

        Task<List<ImUser>> GetAllUsers();
        
        Task<List<Issue>> GetAllIssues();

        Task<List<Issue>> GetIssuesByUserStory(string userStory);
        
        

    }
}