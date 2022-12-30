using EnTier;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Authentication.Attributes;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("stories")]
    [AuthorizeIssueManagers]
    public class StoriesController : CrudControllerBase<UserStory,UserStory,UserStoryDal,long,long,long>
    {
        public StoriesController(EnTierEssence essence) : base(essence)
        {
        }
    }
}
