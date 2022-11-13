using System;
using System.Collections.Generic;
using System.Linq;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Services
{
    public class LudwigJiraFieldDefinitionProvider:ICustomFieldDefinitionProvider
    {
        public IEnumerable<CustomFieldDefinition> Provide(IEnumerable<JiraField> availableFields)
        {

            var definitions = new List<CustomFieldDefinition>();
            
            var userStoryField = availableFields.FirstOrDefault(f => f.Name?.ToLower() == "user story");

            if (userStoryField != null)
            {
                var id = userStoryField.Id;

                var def = new CustomFieldDefinition
                {
                    Type = typeof(string),
                    FieldName = userStoryField.Id,
                    SetIntoIssue = (i, o) => i.UserStory = (string)o
                };
                definitions.Add(def);
            }
            
            return definitions;
        }
    }
}