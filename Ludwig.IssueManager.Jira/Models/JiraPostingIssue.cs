using System.Collections.Generic;

namespace Ludwig.IssueManager.Jira.Models
{

        
    public class JiraPostingFields:Dictionary<string,string>
    {
        public string Summary { get; set; }
        
        public IdValue IssueType { get; set; }
        
        public IdValue Project { get; set; }
        
    }
    
    public class JiraPostingIssue
    {
        public string Descriptions { get; set; }
        
        public JiraPostingFields Fields { get; set; }
        
    }
}

