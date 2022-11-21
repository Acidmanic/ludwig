using System.Collections.Generic;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Interfaces
{
    internal interface ICustomFieldDefinitionProvider
    {
        IEnumerable<CustomFieldDefinition> Provide(IEnumerable<JiraField> availableFields);
    }
}