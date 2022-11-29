namespace Ludwig.Contracts.Models
{
    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }
        
        public string SubjectId { get; set; }
        
        public string EmailAddress { get; set; }
        
        public string SubjectWebPage { get; set; }
        
        public bool IsIssueManager { get; set; } 
        
        public bool IsAdministrator { get; set; }
        
        
    }
}