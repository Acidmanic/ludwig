namespace Ludwig.IssueManager.Jira.Models
{
    public class JiraFixVersion
    {
        public string Self { get; set; }
        
        public string Id { get; set; }
        
        public string Description { get; set; }
        
        public string Name { get; set; }
        
        public bool Archived { get; set; }
        
        public bool Released { get; set; }
    }
}
