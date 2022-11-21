using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Extensions;
using Ludwig.Presentation.Models;
using Ludwig.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthenticationManager _authenticationManager;

        public AuthorizationController(AuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }


        [HttpGet]
        [Route("login-methods")]
        public IActionResult GetLoginMethods()
        {
            return Ok(new
            {
                loginMethods = _authenticationManager.LoginMethods
            });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginParameters parameters)
        {
            var loggedIn = await _authenticationManager.Login(parameters);

            if (loggedIn)
            {
                return Ok(loggedIn.Primary.AsToken());
            }

            return Unauthorized(new { message = loggedIn.Secondary });
        }

        [HttpGet]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            _authenticationManager.Revoke();

            return Ok();
        }
        
        [HttpGet]
        [Route("revoke/{token}")]
        public IActionResult Revoke(string token)
        {
            _authenticationManager.Revoke(token);

            return Ok();
        }
    }
}