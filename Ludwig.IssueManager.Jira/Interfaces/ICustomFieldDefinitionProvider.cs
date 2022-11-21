using System.Collections.Generic;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Interfaces
{
    public interface ICustomFieldDefinitionProvider
    {
        IEnumerable<CustomFieldDefinition> Provide(IEnumerable<JiraField> availableFields);
    }
}