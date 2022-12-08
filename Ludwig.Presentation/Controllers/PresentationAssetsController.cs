using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    
    [Route("presentation-assets")]
    public class PresentationAssetsController:ControllerBase
    {
        
        
        [HttpGet]
        [Route("png/{name}")]
        public IActionResult ProfileImage(string name)
        {

            var dir = new FileInfo(Assembly.GetEntryAssembly().Location)
                .Directory.FullName;
            
            var fileName = Path.Combine(dir,"Assets","Images",name);

            return new PhysicalFileResult(fileName, "image/png");
            
        }
        
        
        [HttpGet]
        [Route("svg/{name}")]
        public IActionResult Icon(string name)
        {

            var dir = new FileInfo(Assembly.GetEntryAssembly().Location)
                .Directory.FullName;
            
            var fileName = Path.Combine(dir,"Assets","Images",name);

            return new PhysicalFileResult(fileName, "image/svg+xml");
            
        }
    }
}