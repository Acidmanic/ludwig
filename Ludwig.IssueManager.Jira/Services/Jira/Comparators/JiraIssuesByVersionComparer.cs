using System.Collections.Generic;
using System.Linq;
using Acidmanic.Utilities.Results;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Services.Jira.Comparators
{
    internal class JiraIssuesByVersionComparer : Comparer<JiraIssue>
    {
        public override int Compare(JiraIssue x, JiraIssue y)
        {
            var topX = TopVersion(x);
            var topY = TopVersion(y);

            if (!topX && !topY)
            {
                return 0;
            }

            if (!topX)
            {
                return -1;
            }

            if (!topY)
            {
                return 1;
            }

            return new JiraVersionComparer().Compare(topX, topY);
        }

        private Result<JiraFixVersion> TopVersion(JiraIssue issue)
        {
            if (issue != null && issue.FixVersions != null && issue.FixVersions.Count > 0)
            {
                issue.FixVersions.Sort(new JiraVersionComparer());

                return issue.FixVersions.LastOrDefault();
            }

            return new Result<JiraFixVersion>().FailAndDefaultValue();
        }
    }
}