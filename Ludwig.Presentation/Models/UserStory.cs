using System.Collections.Generic;
using Ludwig.Contracts.Models;
using Ludwig.DataAccess.Models;

namespace Ludwig.Presentation.Models
{
    public class UserStory : UserStoryDal
    {
        public List<Issue> Issues { get; set; }
        
        public Priority Priority { get; set; }
    }
}