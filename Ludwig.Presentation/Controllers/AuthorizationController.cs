using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading.Tasks;
using Ludwig.Presentation.Download;
using Ludwig.Presentation.Extensions;
using Ludwig.Presentation.Models;
using Ludwig.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    
    [ApiController]
    [Route("auth")]
    public class AuthorizationController:ControllerBase
    {

        private readonly Jira _jira;

        public AuthorizationController(Jira jira)
        {
            _jira = jira;
        }


        [HttpPost]
        [Route("jira/login")]
        public async Task<IActionResult> JiraLogin(Credentials credentials)
        {

            var loggedIn = await _jira.LoginByCredentials(credentials.Username, credentials.Password);

            if (loggedIn)
            {
                
                var rawSentCookies = loggedIn.Secondary.RawSentCookies();

                rawSentCookies.ForEach(c => HttpContext.Response.Headers.Add("Set-Cookie",c));
                
                return Ok(loggedIn.Primary);
            }

            return Unauthorized();
        }
        
        
        [HttpPost]
        [Route("admin/login")]
        public IActionResult AdminLogin(string password)
        {

            return Unauthorized();
        }
        
    }
}