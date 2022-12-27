using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.DataAccess.Models
{
    public class UserStoryDal
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
        
        
        public int PriorityValue { get; set; }
        
        public int PriorityName { get; set; }
        
        public bool IsDone { get; set; }
        
        
    }
}