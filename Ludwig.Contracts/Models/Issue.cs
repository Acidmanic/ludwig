using System.Collections.Generic;

namespace Ludwig.Contracts.Models
{
    public class Issue
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public List<IssueManagerUser> Assignees { get; set; }
        
        public string UserStory { get; set; }
        
        public IssueType IssueType { get; set; }
        
        public Priority Priority { get; set; }
        
        public string IssueReferenceLink { get; set; }
    }
}