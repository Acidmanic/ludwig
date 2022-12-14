using Acidmanic.Utilities.Reflection;
using Ludwig.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("statics")]
    public class StaticsController:ControllerBase
    {


        [HttpGet]
        [Route("priorities")]
        public IActionResult GetPriorities()
        {
            return Ok(new EnumerableDynamicWrapper<Priority>().Wrap(Priority.Priorities));
        }
    }
}