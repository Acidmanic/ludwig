using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnTier.Controllers;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("story-users")]
    public class StoryUsersController : CrudControllerBase<StoryUser,long>
    {
        
    }
}
