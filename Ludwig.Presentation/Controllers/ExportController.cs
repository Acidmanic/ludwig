using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ludwig.Presentation.Authentication.Attributes;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.ExporterManagement;
using Ludwig.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ludwig.Presentation.Controllers
{
    
   [ApiController]
   [Route("export")]
   [AuthorizeIssueManagers]
   public class ExportController:ControllerBase
    {

        private readonly IUserStoryService _userStoryService;
        private readonly IDatabaseExporter _databaseExporter;
        private readonly ExporterManager _exporterManager;
        
        public ExportController(IUserStoryService userStoryService, IDatabaseExporter databaseExporter, ExporterManager exporterManager)
        {
            _databaseExporter = databaseExporter;
            _exporterManager = exporterManager;
            _userStoryService = userStoryService;
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
            
            var fileName = "user-stories-"+ DateTime.Now.ToString("yyyyMMdd-hhmmss") + $".{format}";

            return ThrowDownload(contentBytes, fileContent, $"application/{format}");
        }
        
        private IActionResult ThrowDownload(byte[] fileContent, string fileName, string contentType)
        {
            var contentStream = new MemoryStream(fileContent);

            return File(contentStream,  contentType,fileName, true);
        }

        
        [HttpGet]
        [Route("exporters")]
        public IActionResult GetAvailableExporters()
        {
            var ids = _exporterManager.Exporters.Select(e => e.Id);

            return Ok(ids);
        }

        [HttpGet]
        [Route("{id}")]
        public async  Task<IActionResult> ExportById(string id)
        {
            var exporter = _exporterManager.Exporters.FirstOrDefault(e => e.Id.UniqueKey == id);

            if (exporter != null)
            {
                var exportData = await exporter.ProvideExport();

                return ThrowDownload(exportData.Data, exportData.FileName, exportData.MediaType);
            }

            return NotFound();
        }
        
    }
}