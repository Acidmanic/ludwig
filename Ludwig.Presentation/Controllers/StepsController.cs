using EnTier;
using EnTier.AutoWrap;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [AutoWrap]
    [ApiController]
    [Route("steps")]
    public class StepsController:CrudControllerBase<Step,long>
    {
        public StepsController(EnTierEssence essence) : base(essence)
        {
        }
    }
}