using System.Collections.Generic;
using System.Linq;
using Ludwig.Contracts.Models;
using Ludwig.Presentation.Models;

namespace Ludwig.Presentation.Utilities
{
    public class UserStoryCsvConvert : CsvConvert<UserStory>
    {
        protected override string CommaSeparate(UserStory value)
        {
            return string.Join(',',
                Escape(value.Title),
                Escape(value.StoryUser.Name),
                Escape(value.StoryFeature),
                Escape(value.StoryBenefit),
                Escape(value.Priority.Name),
                ToString(value.Issues)
            );
        }

        protected override string GetHeaders()
        {
            return "Title,User,Feature,Benefit,Priority,Issues";
        }

        private string ToString(IEnumerable<Issue> issues)
        {
            return string.Join(" | ", issues.Select(i => i.Title));
        }
    }
}