using System.Net.Http;
using Ludwig.Presentation.Download;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Contracts
{
    public interface ICookieForwarder
    {
        void ForwardCookies(HttpContext context, PatientDownloader downloader);
    }
}