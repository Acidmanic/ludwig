using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnTier.Results;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Download;
using Ludwig.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Ludwig.Presentation.Services
{
    public class Jira
    {
        private class Resources
        {
            public const string AllUsers = "rest/api/2/user/search?username=\".\"";
            public const string Self = "rest/api/2/myself";
            public const string AllIssues = "rest/api/2/search";
            public const string AllFields = "rest/api/2/field";
        }


        private HttpContext _httpContext = null;
        private readonly ICookieForwarder _cookieForwarder;
        private readonly string _baseUrl;
        private readonly ICustomFieldDefinitionProvider _definitionProvider;

        public Jira(ILudwigConfigurationProvider configurationProvider, ICookieForwarder cookieForwarder,
            ICustomFieldDefinitionProvider definitionProvider)
        {
            _cookieForwarder = cookieForwarder;
            _definitionProvider = definitionProvider;

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

            _cookieForwarder.ForwardCookies(_httpContext, downloader);

            return downloader;
        }


        public async Task<List<JiraUser>> AllUsers()
        {
            var downloader = GetDownloader();

            var url = _baseUrl + Resources.AllUsers;

            var result = await downloader.DownloadObject<List<JiraUser>>(url, 1200, 12);

            if (result)
            {
                return result.Value;
            }

            return new List<JiraUser>();
        }

        public async Task<List<JiraField>> AllFields()
        {
            var downloader = GetDownloader();

            var url = _baseUrl + Resources.AllFields;

            var result = await downloader.DownloadObject<List<JiraField>>(url, 1200, 12);

            if (result)
            {
                return result.Value;
            }

            return new List<JiraField>();
        }


        public async Task<List<JiraIssue>> AllIssues()
        {
            var downloader = GetDownloader();

            var url = _baseUrl + Resources.AllIssues;

            var result = await downloader.DownloadObject<JiraIssueChunk>(url, 1200, 12);

            if (result)
            {
                var fields = await AllFields();

                var definitions = _definitionProvider.Provide(fields);

                result.Value.Issues.ForEach(i => JiraIssueNormalizer.Normalize(i, definitions));

                return result.Value.Issues;
            }

            return new List<JiraIssue>();
        }


        public async Task<List<JiraIssue>> IssuesByUserStory(string userStory)
        {
            var downloader = GetDownloader();

            var url = _baseUrl + Resources.AllIssues + $"?jql=\"User%20Story\"%20~%20\"{userStory}\"";

            var result = await downloader.DownloadObject<JiraIssueChunk>(url, 1200, 12);

            if (result)
            {
                var fields = await AllFields();

                var definitions = _definitionProvider.Provide(fields);

                result.Value.Issues.ForEach(i => JiraIssueNormalizer.Normalize(i, definitions));

                return result.Value.Issues;
            }

            return new List<JiraIssue>();
        }

        public async Task<Result<JiraUser>> LoggedInUser()
        {
            var downloader = GetDownloader();

            var url = _baseUrl + Resources.Self;

            var result = await downloader.DownloadObject<JiraUser>(url, 1200, 12);

            if (result)
            {
                return new Result<JiraUser>(true, result.Value);
            }

            return new Result<JiraUser>().FailAndDefaultValue();
        }
    }
}