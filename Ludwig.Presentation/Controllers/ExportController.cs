using System;
using System.IO;
using System.Linq;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ludwig.Presentation.Controllers
{
    
   [ApiController]
   [Route("export")]
    public class ExportController:ControllerBase
    {

        private readonly IUserStoryService _userStoryService;
        private readonly IDatabaseExporter _databaseExporter;
        
        public ExportController(IUserStoryService userStoryService, IDatabaseExporter databaseExporter)
        {
            _databaseExporter = databaseExporter;
            _userStoryService = userStoryService.UseContext(HttpContext);
        }


        [HttpGet]
        [Route("json/stories")]
        public IActionResult ExportUserStoriesAsJson()
        {
            var allStories = _userStoryService.GetAll().ToList();

            var jsonContent = JsonConvert.SerializeObject(allStories);

            return ThrowDownload(jsonContent, "json");
        }
        
        [HttpGet]
        [Route("csv/stories")]
        public IActionResult ExportUserStoriesAsCvs()
        {
            var allStories = _userStoryService.GetAll().ToList();

            var csvContent = new UserStoryCsvConvert().Add(allStories).ToString();

            return ThrowDownload(csvContent, "csv");
        }
        
        
        [HttpGet]
        [Route("json/database")]
        public IActionResult ExportDatabaseJson()
        {
            var database = _databaseExporter.Export();

            var jsonContent = JsonConvert.SerializeObject(database);

            return ThrowDownload(jsonContent, "json");
        }


        private IActionResult ThrowDownload(string fileContent, string format)
        {
            var contentBytes = System.Text.Encoding.Default.GetBytes(fileContent);

            var contentStream = new MemoryStream(contentBytes);

            var fileName = "user-stories-"+ DateTime.Now.ToString("yyyyMMdd-hhmmss") + $".{format}";

            return File(contentStream,  $"application/{format}",fileName, true);
        }
        
        
        
    }
}