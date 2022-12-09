using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acidmanic.Utilities.Results;
using Ludwig.Common.Extensions;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Configuration;
using Ludwig.IssueManager.Jira.Interfaces;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Services.Jira
{
    internal class Jira
    {
        private class Resources
        {
            public const string AllUsers = "rest/api/2/user/search?username=\".\"";
            public const string Self = "rest/api/2/myself";
            public const string AllIssues = "rest/api/2/search";
            public const string AllFields = "rest/api/2/field";
            public const string AllProjects = "rest/api/2/project";
        }


        private readonly IBackChannelRequestGrant _backChannelRequestGrant;
        private readonly string _baseUrl;
        private readonly ICustomFieldDefinitionProvider _definitionProvider;

        private class JiraFields : LazyCache<List<JiraField>>
        {
        }

        private class Projects : LazyCache<List<JiraProject>>
        {
        }

        private class TaskIssueType : LazyCacheRetryNulls<JiraIssueType>
        {
        }

        public Jira(IConfigurationProvider configurationProvider,
            ICustomFieldDefinitionProvider definitionProvider, IBackChannelRequestGrant backChannelRequestGrant)
        {
            _definitionProvider = definitionProvider;

            _backChannelRequestGrant = backChannelRequestGrant;

            _baseUrl = configurationProvider.GetConfiguration<JiraConfiguration>().JiraBackChannelUrl.Slashend();

            JiraFields.Instance.SetProvider(AllFields);

            Projects.Instance.SetProvider(AllProjects);
            
            TaskIssueType.Instance.SetProvider(GetTaskIssueType);
        }

        public async Task<List<JiraUser>> AllUsers()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllUsers;

            var result = await downloader.DownloadObject<List<JiraUser>>(url, 1200, 12);

            if (result)
            {
                return result.Value;
            }

            return new List<JiraUser>();
        }

        public async Task<List<JiraField>> AllFieldsAsync()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllFields;

            var result = await downloader.DownloadObject<List<JiraField>>(url, 1200, 12);

            if (result)
            {
                return result.Value;
            }

            return new List<JiraField>();
        }

        public List<JiraField> AllFields()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllFields;

            var result = downloader.DownloadObject<List<JiraField>>(url, 1200, 12).Result;

            if (result)
            {
                return result.Value;
            }

            return new List<JiraField>();
        }

        public List<JiraProject> AllProjects()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllProjects;

            var result = downloader.DownloadObject<List<JiraProject>>(url, 1200, 12).Result;

            if (result)
            {
                return result.Value;
            }

            return new List<JiraProject>();
        }

        public async Task<List<JiraProject>> AllProjectsAsync()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllProjects;

            var result = await downloader.DownloadObject<List<JiraProject>>(url, 1200, 12);

            if (result)
            {
                return result.Value;
            }

            return new List<JiraProject>();
        }

        public JiraIssueType GetTaskIssueType()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllProjects;

            var result = downloader.DownloadObject<List<JiraIssueType>>(url, 1200, 12).Result;

            if (result)
            {
                var task = result.Value.FirstOrDefault(t => t.Name.ToLower() == "task");

                return task;
            }

            return null;
        }


        public async Task<List<JiraIssue>> AllIssues()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllIssues;

            var result = await downloader.DownloadObject<JiraIssueChunk>(url, 1200, 12);

            if (result)
            {
                var fields = await AllFieldsAsync();

                var definitions = _definitionProvider.Provide(fields);

                result.Value.Issues.ForEach(i => JiraIssueNormalizer.Normalize(i, definitions));

                return result.Value.Issues;
            }

            return new List<JiraIssue>();
        }


        public async Task<List<JiraIssue>> IssuesByUserStory(string userStory)
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            var url = _baseUrl + Resources.AllIssues + $"?jql=\"User%20Story\"%20~%20\"{userStory}\"";

            var result = await downloader.DownloadObject<JiraIssueChunk>(url, 1200, 12);

            if (result)
            {
                var fields = await AllFieldsAsync();

                var definitions = _definitionProvider.Provide(fields);

                result.Value.Issues.ForEach(i => JiraIssueNormalizer.Normalize(i, definitions));

                return result.Value.Issues;
            }

            return new List<JiraIssue>();
        }

        public Task<Result<JiraUser, string>> LoggedInUser()
        {
            return LoggedInUser(null);
        }

        private async Task<Result<JiraUser, string>> LoggedInUser(string authHeader)
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                downloader.Headers.Add("Authorization", authHeader);
            }

            var url = _baseUrl + Resources.Self;

            var result = await downloader.DownloadObject<JiraUser>(url, 1200, 12);

            if (result)
            {
                return new Result<JiraUser, string>(true, authHeader, result.Value);
            }

            return new Result<JiraUser, string>().FailAndDefaultBothValues();
        }

        public Task<Result<JiraUser, string>> LoginByCredentials(string username, string password)
        {
            var credentials = username + ":" + password;

            var credBytes = System.Text.Encoding.Default.GetBytes(credentials);

            var credBase64 = Convert.ToBase64String(credBytes);

            return LoggedInUser("Basic " + credBase64);
        }
    }
}