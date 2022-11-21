using System;
using System.Collections.Generic;
using Ludwig.IssueManager.Jira.Models;
using Newtonsoft.Json.Linq;

namespace Ludwig.IssueManager.Jira.Services
{
    internal static class JiraIssueNormalizer
    {
        public static void Normalize(JiraIssue issue, IEnumerable<CustomFieldDefinition> definitions = null)
        {
            issue.IssueType = Cast<IssueType>(issue, "issuetype");

            issue.Summary = Cast<string>(issue, "summary");

            issue.Description = Cast<string>(issue, "description");

            issue.Project = Cast<JiraProject>(issue, "project");

            issue.Priority = Cast<JiraPriority>(issue, "priority");

            issue.Assignee = Cast<JiraUser>(issue, "assignee");

            if (definitions != null)
                foreach (var definition in definitions)
                {
                    Cast(issue, definition);
                }
        }


        private static void Cast(JiraIssue issue, CustomFieldDefinition definition)
        {
            if (issue.Fields.ContainsKey(definition.FieldName))
            {
                var jObject = issue.Fields[definition.FieldName];

                if (jObject == null)
                {
                    return;
                }

                if (definition.Type.IsInstanceOfType(jObject))
                {
                    definition.SetIntoIssue(issue, jObject);

                    return;
                }

                if (jObject is JObject j)
                {
                    try
                    {
                        var value = j.ToObject(definition.Type);

                        definition.SetIntoIssue(issue, jObject);

                        return;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        private static T Cast<T>(JiraIssue issue, string name)
        {
            if (issue.Fields.ContainsKey(name))
            {
                var jObject = issue.Fields[name];

                if (jObject is T value)
                {
                    return value;
                }

                if (jObject is JObject j)
                {
                    try
                    {
                        return j.ToObject<T>();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return default;
        }
    }
}