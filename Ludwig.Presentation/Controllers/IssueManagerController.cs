using System.Threading.Tasks;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Authentication.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    
    
    [ApiController]
    [Route("issue-manager")]
    public class IssueManagerController:ControllerBase
    {

        private readonly IIssueManager _issueManager;

        public IssueManagerController(IIssueManager issueManager)
        {
            _issueManager = issueManager;
        }


        [HttpGet]
        [AuthorizeAdministratorsOrIssueManagers]
        [Route("me")]
        public async Task<IActionResult> GetMe()
        {
            var user = await _issueManager.GetCurrentUser();

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }
        
        
        [HttpGet]
        [Route("users")]
        [AuthorizeIssueManagers]
        public async  Task<IActionResult> AllUsers()
        {
            var users = await _issueManager.GetAllUsers();

            return Ok(users);
        }
        
        [HttpGet]
        [Route("issues")]
        [AuthorizeIssueManagers]
        public async  Task<IActionResult> AllIssues()
        {
            var issues = await _issueManager.GetAllIssues();
            
            return Ok(issues);
        }
        
        
        [HttpPost]
        [Route("issues")]
        [AuthorizeIssueManagers]
        public async  Task<IActionResult> CreateIssue(Issue issue)
        {
            var created = await _issueManager.AddIssue(issue);
            
            return Ok(created);
        }
        
        [HttpGet]
        [Route("issues-by-story/{story}")]
        [AuthorizeIssueManagers]
        public async  Task<IActionResult> IssuesByStory(string story)
        {
            var issues = await _issueManager.GetIssuesByUserStory(story);
            
            return Ok(issues);
        }
    }
}