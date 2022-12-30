using System.Collections.Generic;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Models;

namespace Ludwig.Presentation.Models
{
    public class UserStory 
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        public StoryUser StoryUser { get; set; }
        
        public long StoryUserId { get; set; }
        
        public string StoryFeature { get; set; }
        
        public string StoryBenefit { get; set; }
        
        public string CardColor { get; set; }
        
        public bool IsDone { get; set; }
        
        public List<Issue> Issues { get; set; }
        
        public Priority Priority { get; set; }
    }
}