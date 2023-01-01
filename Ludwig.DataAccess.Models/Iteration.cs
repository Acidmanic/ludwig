using System.Collections.Generic;
using Acidmanic.Utilities.Reflection.Attributes;

namespace Ludwig.DataAccess.Models
{
    [OwnerName("Iterations")]
    public class Iteration
    {
        [AutoValuedMember] [UniqueMember] public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public long ProjectId { get; set; }
        
        public List<Task> Tasks { get; set; }
    }
}