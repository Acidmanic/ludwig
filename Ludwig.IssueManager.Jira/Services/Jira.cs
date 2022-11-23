using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Acidmanic.Utilities.Results;
using Ludwig.Common.Download;
using Ludwig.Common.Extensions;
using Ludwig.Contracts.Authentication;
using Ludwig.IssueManager.Jira.Interfaces;
using Ludwig.IssueManager.Jira.Models;
using Microsoft.AspNetCore.Http;

namespace Ludwig.IssueManager.Jira.Services
{
    internal class Jira
    {
        private class Resources
        {
            public const string AllUsers = "rest/api/2/user/search?username=\".\"";
            public const string Self = "rest/api/2/myself";
            public const string AllIssues = "rest/api/2/search";
            public const string AllFields = "rest/api/2/field";
        }


        private readonly IBackChannelRequestGrant _backChannelRequestGrant;
        private readonly string _baseUrl;
        private readonly ICustomFieldDefinitionProvider _definitionProvider;

        public Jira(IJiraConfigurationProvider configurationProvider,
            ICustomFieldDefinitionProvider definitionProvider, IBackChannelRequestGrant backChannelRequestGrant)
        {
            _definitionProvider = definitionProvider;
            _backChannelRequestGrant = backChannelRequestGrant;

            var config = configurationProvider.GetConfiguration();

            var baseUrl = config.JiraBackChannelUrl;

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            _baseUrl = baseUrl;
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

        public async Task<List<JiraField>> AllFields()
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


        public async Task<List<JiraIssue>> AllIssues()
        {
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

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
            var downloader = _backChannelRequestGrant.CreateGrantedDownloader();

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