using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.DataAccess.Models
{
    public class Project
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [AutoValuedMember]
        [UniqueMember]
        public long Id { get; set; }
        
        
    }
}