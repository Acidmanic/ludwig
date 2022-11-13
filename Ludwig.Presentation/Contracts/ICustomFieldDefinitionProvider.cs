using System.Collections;
using System.Collections.Generic;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Contracts
{
    public interface ICustomFieldDefinitionProvider
    {
        IEnumerable<CustomFieldDefinition> Provide(IEnumerable<JiraField> availableFields);
    }
}