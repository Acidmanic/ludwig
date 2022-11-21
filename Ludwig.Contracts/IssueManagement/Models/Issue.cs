namespace Ludwig.Contracts.IssueManagement.Models
{
    public class Issue
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public ImUser Assignee { get; set; }
        
        public string UserStory { get; set; }
        
        public IssueType IssueType { get; set; }
        
        public Priority Priority { get; set; }
    }
}