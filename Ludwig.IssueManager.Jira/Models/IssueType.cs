namespace Ludwig.IssueManager.Jira.Models
{
    public class IssueType
    {
        public string Self { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string Name { get; set; }

        public bool SubTask { get; set; }

        public long AvatarId { get; set; }
    }
}