using System;
using System.IO;
using System.Linq;
using Ludwig.Presentation.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ludwig.Presentation.Controllers
{
    
   [ApiController]
   [Route("export")]
    public class ExportController:ControllerBase
    {

        private readonly IUserStoryService _userStoryService;

        public ExportController(IUserStoryService userStoryService)
        {
            _userStoryService = userStoryService.UseContext(HttpContext);
        }


        [HttpGet]
        [Route("json/stories")]
        public IActionResult ExportUserStoriesAsJson()
        {
            var allStories = _userStoryService.GetAll().ToList();

            var jsonContent = JsonConvert.SerializeObject(allStories);

            var jsonBytes = System.Text.Encoding.Default.GetBytes(jsonContent);

            var jsonStream = new MemoryStream(jsonBytes);

            var fileName = "user-stories-"+ DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".json";

            return File(jsonStream,  "application/json",fileName, true);
        }
        
    }
}