using System;

namespace Ludwig.Presentation.Models
{
    public class CustomFieldDefinition
    {
        public string FieldName { get; set; }
        
        public Type Type { get; set; }

        public Action<JiraIssue, object> SetIntoIssue { get; set; } = (i, o) => { };
    }
}