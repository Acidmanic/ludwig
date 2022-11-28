using Ludwig.IssueManager.Gitlab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.IssueManager.Gitlab.Controllers
{
    [ApiController]
    [Route("authorization-code")]
    public class AuthorizationCodeController:ControllerBase
    {



        [HttpGet]
        [Route("receive")]
        public IActionResult ReceiveCode([FromQuery] string code)
        {


            return Ok();
        } 
    }
}