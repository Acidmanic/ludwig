using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ludwig.Common.Extensions;
using Ludwig.IssueManager.Jira.Models;
using Ludwig.IssueManager.Jira.Services.Jira.Comparators;

namespace Ludwig.IssueManager.Jira.Services
{
    internal class ReleaseNoteDocument
    {
        private readonly List<JiraIssue> _issues;
        private readonly string _jiraFrontChannel;


        public ReleaseNoteDocument(List<JiraIssue> issues, string jiraFrontChannel)
        {
            _issues = issues;
            _jiraFrontChannel = jiraFrontChannel;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("\n\nJira - Release Notes").Append("\n================\n\n\n");

            NormalizeSubTaskVersions(_issues);

            var filteredIssues = FilterReleaseNoteAppropriateIssues(_issues);

            var issuesByVersion = GroupByVersion(filteredIssues);

            var sortedKeys = issuesByVersion.Keys.ToList();

            sortedKeys.Sort(new JiraVersionComparer(true));

            foreach (var keyVersion in sortedKeys)
            {
                var issues = issuesByVersion[keyVersion];

                if (issues.Count > 0)
                {
                    AppendVersion(sb, keyVersion, issues[0].Project.Key);

                    sb.Append("\n\n");
                }

                foreach (var issue in issues)
                {
                    AppendIssue(sb, issue);
                }

                sb.Append("\n\n\n\n");
            }

            return sb.ToString();
        }

        private void NormalizeSubTaskVersions(List<JiraIssue> issues)
        {
            var issuesById = new Dictionary<string, JiraIssue>();

            foreach (var issue in issues)
            {
                var key = issue.Key;

                if (!issuesById.ContainsKey(key))
                {
                    issuesById.Add(key, issue);
                }
            }

            foreach (var issue in issues)
            {
                var parentKey = issue.Parent?.Key;

                if (!string.IsNullOrWhiteSpace(parentKey) && issuesById.ContainsKey(parentKey))
                {
                    issue.Parent = issuesById[parentKey];

                    if (issue.FixVersions == null || issue.FixVersions.Count == 0)
                    {
                        if (issue.Parent.FixVersions != null)
                        {
                            issue.FixVersions = new List<JiraFixVersion>(issue.Parent.FixVersions);
                        }
                    }
                }
            }
        }

        private void AppendIssue(StringBuilder sb, JiraIssue issue)
        {

            var issueUrl = _jiraFrontChannel.Slashend() + "browse/" + issue.Key;
            
            sb.Append("  *  ")
                .Append("[").Append(issue.Key).Append("]")
                .Append("(").Append(issueUrl).Append(") - ")
                .Append(issue.ReleaseNote).Append("\n");
        }

        private void AppendVersion(StringBuilder sb, JiraFixVersion version, string projectKey)
        {
            var title = version.Name;

            var url = "";
            var verse = "";

            // not made up version, but actually coming from jira
            if (!string.IsNullOrWhiteSpace(version.Self))
            {
                url = _jiraFrontChannel.Slashend() + "brows/" + projectKey + "/fixforversion/" + version.Id;

                title = "[" + title + "](" + url + ")";

                verse = "__In This version:__\n\n";
            }

            sb.Append(title).Append("\n-----------\n\n")
                .Append(version.Description)
                .Append("\n\n").Append(verse);
        }


        private List<JiraIssue> FilterReleaseNoteAppropriateIssues(List<JiraIssue> issues)
        {
            var releasyIssues = issues.Where(HaveReleaseNote);

            var doneIssues = releasyIssues.Where(IsDone);


            return doneIssues.ToList();
        }

        private Dictionary<JiraFixVersion, List<JiraIssue>> GroupByVersion(List<JiraIssue> issues)
        {
            var initialProduct = new JiraFixVersion
            {
                Archived = false,
                Description = "These are the features implemented before first tagged version.",
                Name = "Initial Product",
                Self = null,
                Id = "initial"
            };

            var result = new Dictionary<JiraFixVersion, List<JiraIssue>>();

            foreach (var issue in issues)
            {
                issue.FixVersions.Sort(new JiraVersionComparer(true));

                var version = issue.FixVersions?.LastOrDefault();

                if (version == null)
                {
                    version = initialProduct;
                }

                bool Matches(KeyValuePair<JiraFixVersion, List<JiraIssue>> kv) => kv.Key?.Id == version.Id;

                if (!result.Any(Matches))
                {
                    result.Add(version, new List<JiraIssue>());
                }

                result.Single(Matches).Value.Add(issue);
            }

            return result;
        }

        private bool HaveReleaseNote(JiraIssue issue)
        {
            return !string.IsNullOrWhiteSpace(issue.ReleaseNote);
        }

        private bool IsDone(JiraIssue issue)
        {
            var resolution = issue.Resolution?.Name;

            var status = issue.Status?.Name;

            return IsDone(resolution) && IsDone(status);
        }

        private bool IsDone(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Trim().ToLower() == "done";
        }
    }
}