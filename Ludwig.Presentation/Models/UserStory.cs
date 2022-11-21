using System.Collections.Generic;
using Acidmanic.Utilities.Reflection.Attributes;
using Ludwig.Contracts.IssueManagement.Models;

namespace Ludwig.Presentation.Models
{
    public class UserStory
    {
        [UniqueMember]
        [AutoValuedMember]
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        public StoryUser StoryUser { get; set; }
        
        public long StoryUserId { get; set; }
        
        public string StoryFeature { get; set; }
        
        public string StoryBenefit { get; set; }
        
        public string CardColor { get; set; }
        
        public List<JiraIssue> Issues { get; set; }
        
        public Priority Priority { get; set; }
    }
}