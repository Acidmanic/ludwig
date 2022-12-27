using EnTier;
using EnTier.Controllers;
using Ludwig.Presentation.Authentication.Attributes;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("story-users")]
    [AuthorizeIssueManagers]
    public class StoryUsersController : CrudControllerBase<StoryUser,long>
    {
        public StoryUsersController(EnTierEssence essence) : base(essence)
        {
        }
    }
}
