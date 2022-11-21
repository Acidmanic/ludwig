using Ludwig.Common.Download;
using Ludwig.IssueManager.Jira.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ludwig.IssueManager.Jira.Services
{
    public class DeployedClientCookieForwarder:ICookieForwarder
    {
        public void ForwardCookies(HttpContext context, PatientDownloader downloader)
        {
            
            if (context != null && context.Request!=null)
            {
                downloader.Cookies = context.Request.Cookies;
            }
            
        }
    }
}