using System.Collections.Generic;
using System.Threading.Tasks;
using EnTier.Results;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Download;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace Ludwig.Presentation.Services
{
    public class Jira
    {


        private class Resources
        {
            public const string AllUsersUri = "rest/api/2/user/search?username=\".\"";    
            public const string Self = "rest/api/2/myself";    
        }



        private HttpContext _httpContext = null;
        private readonly ICookieForwarder _cookieForwarder;
        private readonly string _baseUrl;

        public Jira(ILudwigConfigurationProvider configurationProvider, ICookieForwarder cookieForwarder)
        {
            _cookieForwarder = cookieForwarder;
            
            var config = configurationProvider.Configuration;

            var baseUrl = config.JiraBaseUrl;
            
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }
            
            _baseUrl = baseUrl;
        }

        public Jira UseContext(HttpContext httpContext)
        {
            _httpContext = httpContext;

            return this;
        }
     
        private PatientDownloader GetDownloader()
        {
            var downloader = new PatientDownloader();
            
            _cookieForwarder.ForwardCookies(_httpContext,downloader);
            
            return downloader;
        }


        public async Task<List<JiraUser>> AllUsers()
        {

            var downloader = GetDownloader();

            var url = _baseUrl + Resources.AllUsersUri;

            var result = await downloader.DownloadObject<List<JiraUser>>(url, 1200, 12);

            if (result)
            {
                return result.Value;
            }

            return new List<JiraUser>();
        }
        
        
        public async Task<Result<JiraUser>> LoggedInUser()
        {

            var downloader = GetDownloader();

            var url = _baseUrl + Resources.Self;

            var result = await downloader.DownloadObject<JiraUser>(url, 1200, 12);

            if (result)
            {
                return new Result<JiraUser>(true,result.Value);
            }

            return new Result<JiraUser>().FailAndDefaultValue();
        }
        
        
    }
}