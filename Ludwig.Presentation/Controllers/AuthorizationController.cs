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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly LoginMethodFilterService _loginMethodFilter;
        public AuthorizationController(AuthenticationManager authenticationManager, LoginMethodFilterService loginMethodFilter)
        {
            _authenticationManager = authenticationManager;
            _loginMethodFilter = loginMethodFilter;
        }


        [HttpGet]
        [Route("login-methods")]
        public IActionResult GetLoginMethods()
        {
            var methods = _authenticationManager.LoginMethods;

            methods = _loginMethodFilter.FilterByConfiguration(methods);
            
            return Ok(new
            {
                loginMethods = methods
            });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginParameters parameters)
        {
            var loggedIn = await _authenticationManager.Login(parameters);

            if (loggedIn)
            {

                
                var key = AuthenticationManager.CookieAuthorizationField;
                var value = loggedIn.Primary.Cookie;

                HttpContext.Response.Cookies.Append(key,value);

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