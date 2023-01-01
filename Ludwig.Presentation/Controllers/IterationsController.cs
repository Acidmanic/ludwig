using EnTier;
using EnTier.AutoWrap;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [AutoWrap("iterations")]
    [ApiController]
    [Route("iterations")]
    public class IterationsController:CrudControllerBase<Iteration,long>
    {
        public IterationsController(EnTierEssence essence) : base(essence)
        {
        }


        public override IActionResult GetAll()
        {
            return base.GetAll();
        }

        public override IActionResult Update(Iteration value)
        {
            return base.Update(value);
        }
    }
}