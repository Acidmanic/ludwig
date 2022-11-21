namespace Ludwig.IssueManager.Jira.Models
{
    internal class JiraIssueType
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