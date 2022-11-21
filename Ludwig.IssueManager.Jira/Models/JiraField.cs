namespace Ludwig.IssueManager.Jira.Models
{
    public class JiraField
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public bool Custom { get; set; }
        
        public bool Orderable { get; set; }
        
        public bool Searchable { get; set; }
        
        public bool Navigable { get; set; }
        
    }
}