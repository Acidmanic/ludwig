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
        [Route("image/{*_}")]
        public async Task<IActionResult> Image(string _)
        {

            var queryString = Request.QueryString.Value;

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                if (queryString.ToLower().StartsWith("?url="))
                {
                    var url = queryString.Substring(5, queryString.Length - 5);
                    
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
                }
            }
            
            

            return NotFound();
        }
    }
}