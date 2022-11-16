using System.Collections.Generic;
using System.Linq;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Utilities
{
    public class UserStoryCsvConvert:CsvConvert<UserStory>
    {
        protected override string CommaSeparate(UserStory value)
        {
            
            return string.Join(',', 
                Escape(value.Title),
                Escape(value.StoryUser.Name),
                Escape(value.StoryFeature),
                Escape(value.StoryBenefit),
                ToString(value.Issues)
                );
        }

        private string ToString(IEnumerable<JiraIssue> issues)
        {
            return string.Join(" | ", issues.Select(i => i.Key));
        }
    }
}