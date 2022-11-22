using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Ludwig.IssueManager.Fake
{
    
    [Route("images")]
    public class ImagesController:ControllerBase
    {


        [HttpGet]
        [Route("profile")]
        public IActionResult ProfileImage()
        {

            var dir = new FileInfo(Assembly.GetEntryAssembly().Location)
                .Directory.FullName;
            
            var fileName = Path.Combine(dir,"Images","cat-profile.png");

            return new PhysicalFileResult(fileName, "image/png");
            
        }
        
        [HttpGet]
        [Route("icon/{name}")]
        public IActionResult Icon(string name)
        {
            var dir = new FileInfo(Assembly.GetEntryAssembly().Location)
                .Directory.FullName;
            
            var fileName = Path.Combine(dir,"Images", name + ".png");

            return new PhysicalFileResult(fileName, "image/png");
           
        }
    }
}