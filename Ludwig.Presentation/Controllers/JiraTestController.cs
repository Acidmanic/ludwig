using System.Threading.Tasks;
using Ludwig.Presentation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("jira")]
    [Authorize]
    public class JiraTestController : ControllerBase
    {

        private readonly Jira _jira;

        public JiraTestController(Jira jira)
        {
            _jira = jira
                .UseContext(HttpContext);
        }

        [HttpGet]
        [Route("users")]
        public async  Task<IActionResult> AllUsers()
        {
            var users = await _jira.AllUsers();

            return Ok(users);
        }
        
        [HttpGet]
        [Route("issues")]
        public async  Task<IActionResult> AllIssues()
        {
            var issues = await _jira.AllIssues();
            
            issues.ForEach( i => i.Fields.Clear());

            return Ok(issues);
        }
        
        [HttpGet]
        [Route("issues-by-story/{story}")]
        public async  Task<IActionResult> IssuesByStory(string story)
        {
            var issues = await _jira.IssuesByUserStory(story);
            
            issues.ForEach( i => i.Fields.Clear());

            return Ok(issues);
        }
        
        [HttpGet]
        [Route("logged-user")]
        public async  Task<IActionResult> LoggedUser()
        {
            var result = await _jira.LoggedInUser();

            return Ok(result);
        }
    }
}