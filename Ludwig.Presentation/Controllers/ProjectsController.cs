using EnTier;
using EnTier.AutoWrap;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("projects")]
    [AutoWrap]
    public class ProjectsController:CrudControllerBase<Project,long>
    {
        public ProjectsController(EnTierEssence essence) : base(essence)
        {
        }

     

        public override IActionResult Update(Project value)
        {
            return base.Update(value);
        }
    }
}