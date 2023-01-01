using System.Collections.Generic;

namespace Ludwig.DataAccess.Models
{
    public class Goal:Card
    {
        public long ProjectId { get; set; }
        
        public List<Step> Steps { get; set; }
    }
}