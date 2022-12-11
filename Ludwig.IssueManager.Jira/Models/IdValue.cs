namespace Ludwig.IssueManager.Jira.Models
{
    public class IdValue
    {
        public string Id { get; set; }
        
        
        public static implicit operator IdValue (string value)
        {
            return new IdValue { Id = value };
        }
        
        public static implicit operator string (IdValue id)
        {
            return id.Id;
        }
    }
}