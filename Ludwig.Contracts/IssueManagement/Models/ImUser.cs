namespace Ludwig.Contracts.IssueManagement.Models
{
    public class ImUser
    {
        public string Name { get; set; }
        
        public string DisplayName { get; set; }
        
        public string EmailAddress { get; set; }
        
        public bool Active { get; set; }
        
        public string AvatarUrl { get; set; }
    }
}