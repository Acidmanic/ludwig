using System.Collections.Generic;
using Ludwig.DataAccess.Models;

namespace Ludwig.Presentation.Models
{
    public class DatabaseV1:Database
    {
        public List<UserStory> Stories { get; set; }
        
        public List<StoryUser> StoryUsers { get; set; }
        
    }
}