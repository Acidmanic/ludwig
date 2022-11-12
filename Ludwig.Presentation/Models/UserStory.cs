using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.Presentation.Models
{
    public class UserStory
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        public string StoryUser { get; set; }
        
        public string StoryFeature { get; set; }
        
        public string StoryBenefit { get; set; }
        
        
    }
}