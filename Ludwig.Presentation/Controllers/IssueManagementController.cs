using System.Threading.Tasks;
using Ludwig.Contracts.IssueManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    
    
    [ApiController]
    [Route("issue-manager")]
    [Authorize]
    public class IssueManagementController:ControllerBase
    {

        private readonly IIssueManager _issueManager;

        public IssueManagementController(IIssueManager issueManager)
        {
            _issueManager = issueManager;
        }


        [HttpGet]
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
    }
}