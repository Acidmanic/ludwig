using EnTier.Controllers;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("stories")]
    [Authorize]
    public class StoriesController : CrudControllerBase<UserStory,long>
    {
        
    }
}
