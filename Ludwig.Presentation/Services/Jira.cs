using System.Collections.Generic;
using System.Threading.Tasks;
using EnTier.Results;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Download;
using Ludwig.Presentation.Extensions;
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
        private string _cookiesString = null;
        
        private readonly string _baseUrl;

        public Jira(ILudwigConfigurationProvider configurationProvider)
        {
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
        
        public Jira UseCookies(string cookiesString)
        {

            _cookiesString = cookiesString;

            return this;
        }
        
        private PatientDownloader GetDownloader()
        {
            var downloader = new PatientDownloader();
            
            if (_httpContext != null)
            {
                downloader.Cookies = _httpContext.Request.Cookies;
            }

            if (!string.IsNullOrEmpty(_cookiesString) && !string.IsNullOrWhiteSpace(_cookiesString))
            {
                downloader.InDirectCookies = _cookiesString.LoadCookies();
            }

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