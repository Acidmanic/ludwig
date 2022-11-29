using EnTier.Controllers;
using Ludwig.Presentation.Authentication.Attributes;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("stories")]
    [AuthorizeIssueManagers]
    public class StoriesController : CrudControllerBase<UserStory,long>
    {
        
    }
}
