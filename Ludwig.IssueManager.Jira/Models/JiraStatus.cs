namespace Ludwig.IssueManager.Jira.Models
{
    public class JiraStatus
    {
        public string Name { get; set; }
        
        public string Self { get; set;}
        
        public string Description { get; set; }
        
        public string IconUrl { get; set; }
        
        public string Id { get; set; }
        
        public JiraStatusCategory StatusCategory { get; set; }
    }
}