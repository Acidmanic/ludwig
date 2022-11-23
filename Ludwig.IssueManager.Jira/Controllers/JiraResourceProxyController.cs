using System.IO;
using System.Threading.Tasks;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.IssueManager.Jira.Controllers
{
    
    [Route("jira")]
    public class JiraResourceProxyController:ControllerBase
    {

        private readonly IBackChannelRequestGrant _backChannelRequestGrant;

        public JiraResourceProxyController(IBackChannelRequestGrant backChannelRequestGrant)
        {
            _backChannelRequestGrant = backChannelRequestGrant;
        }


        [HttpGet]
        [Route("image")]
        public async Task<IActionResult> Image([FromQuery] string url)
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var downloadedImage = await downloader.DownloadFile(url, 1000, 2);


            if (downloadedImage)
            {

                var contentType = downloadedImage.ResponseHeaders["Content-Type"];

                if (string.IsNullOrWhiteSpace(contentType))
                {
                    contentType = "image/jpeg";
                }
                
                return new FileStreamResult(new MemoryStream(downloadedImage.Value), contentType);
            }

            return NotFound();
        }
    }
}