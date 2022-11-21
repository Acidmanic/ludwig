using Ludwig.Common.Download;
using Microsoft.AspNetCore.Http;

namespace Ludwig.IssueManager.Jira.Interfaces
{
    internal interface ICookieForwarder
    {
        void ForwardCookies(HttpContext context, PatientDownloader downloader);
    }
}