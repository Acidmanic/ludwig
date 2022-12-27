using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.DataAccess.Models
{
    public class StoryUser
    {
        [UniqueMember]
        [AutoValuedMember]
        public long Id { get; set; }
        
        public string Name { get; set; }
    }
}