using EnTier;
using EnTier.AutoWrap;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [AutoWrap]
    [ApiController]
    [Route("goals")]
    public class GoalsController : CrudControllerBase<Goal, long>
    {
        public GoalsController(EnTierEssence essence) : base(essence)
        {
        }


        public override IActionResult CreateNew(Goal value)
        {
            return base.CreateNew(value);
        }
    }
}