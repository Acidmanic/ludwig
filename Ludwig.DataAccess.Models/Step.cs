using System.Collections.Generic;

namespace Ludwig.DataAccess.Models
{
    public class Step:Card
    {
        public long ProjectId { get; set; }

        public long GoalId { get; set; }
        
        public List<Task> Tasks { get; set; }


    }
}