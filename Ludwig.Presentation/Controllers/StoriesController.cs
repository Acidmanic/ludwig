using EnTier.Controllers;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("stories")]
    public class StoriesController : CrudControllerBase<UserStory,long>
    {
        
    }
}
