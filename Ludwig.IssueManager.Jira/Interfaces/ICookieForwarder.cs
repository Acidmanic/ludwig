using Ludwig.Common.Download;
using Microsoft.AspNetCore.Http;

namespace Ludwig.IssueManager.Jira.Interfaces
{
    public interface ICookieForwarder
    {
        void ForwardCookies(HttpContext context, PatientDownloader downloader);
    }
}