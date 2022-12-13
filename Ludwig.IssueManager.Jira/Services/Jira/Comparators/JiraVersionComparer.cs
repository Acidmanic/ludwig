using System;
using System.Collections.Generic;
using Acidmanic.Utilities.Results;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Services.Jira.Comparators
{
    internal class JiraVersionComparer : Comparer<JiraFixVersion>
    {
        public override int Compare(JiraFixVersion x, JiraFixVersion y)
        {
            var v1 = TryParse(x?.Name);
            var v2 = TryParse(y?.Name);

            if (!v1 && !v2)
            {
                return 0;
            }

            if (!v1)
            {
                return -1;
            }

            if (!v2)
            {
                return 1;
            }

            return Math.Sign(ToDouble(v1) - ToDouble(v2));
        }

        private double ToDouble(Version v)
        {
            double bilGan = 1;
            double minGan = 100000;
            double majGan = minGan * minGan;

            return bilGan * v.Build + majGan * v.Minor + majGan * v.Major;
        }

        private Result<Version> TryParse(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (Version.TryParse(name, out var version))
                {
                    return version;
                }
            }

            return new Result<Version>().FailAndDefaultValue();
        }
    }


}