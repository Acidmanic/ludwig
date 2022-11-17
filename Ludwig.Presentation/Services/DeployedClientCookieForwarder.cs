using System;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Download;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Services
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