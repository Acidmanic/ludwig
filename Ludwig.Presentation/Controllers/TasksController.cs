using EnTier;
using EnTier.AutoWrap;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    
    [AutoWrap]
    [ApiController]
    [Route("tasks")]
    public class TasksController:CrudControllerBase<Task,long>
    {
        public TasksController(EnTierEssence essence) : base(essence)
        {
        }
    }
}