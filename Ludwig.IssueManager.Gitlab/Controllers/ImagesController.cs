using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.IssueManager.Gitlab.Controllers
{
    [Route("images")]
    public class ImagesController : ControllerBase
    {
        [HttpGet]
        [Route("svg/{name}")]
        public IActionResult GetSvgImage(string name)
        {
            return LoadImage(name, "image/svg+xml");
        }

        [HttpGet]
        [Route("png/{name}")]
        public IActionResult GetPngImage(string name)
        {
            return LoadImage(name, "image/png");
        }


        private IActionResult LoadImage(string name, string contentType)
        {
            var dir = new FileInfo(Assembly.GetEntryAssembly().Location)
                .Directory.FullName;

            var fileName = Path.Combine(dir, "GtilabAdapterImages", name);

            return new PhysicalFileResult(fileName, contentType);
        }
    }
}