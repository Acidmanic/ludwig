using System.Collections.Generic;
using Ludwig.Presentation.Authentication.Attributes;
using Ludwig.Presentation.Configuration;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("configurations")]
    [AuthorizeAdministrators]
    public class ConfigurationController:ControllerBase
    {


        private readonly LudwigConfigurationProvider _ludwigConfigurationProvider;

        public ConfigurationController(LudwigConfigurationProvider ludwigConfigurationProvider)
        {
            _ludwigConfigurationProvider = ludwigConfigurationProvider;
        }


        [HttpGet]
        public IActionResult GetAllConfigurations()
        {
            return Ok(_ludwigConfigurationProvider.GetTransferItems());
        }
        
        [HttpPut]
        public IActionResult UpdateAllConfigurations(List<ConfigurationTransferItem> items)
        {

            var updated = _ludwigConfigurationProvider.UpdateTransferItems(items);


            return Ok(new
            {
                Success = updated.Success,
                Items = _ludwigConfigurationProvider.GetTransferItems(),
                Message = updated.Value
            });
        }
    }
}