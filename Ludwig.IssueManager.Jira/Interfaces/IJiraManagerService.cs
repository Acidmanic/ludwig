using System.Collections.Generic;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Interfaces
{
    public interface IJiraManagerService
    {


        List<JiraIssue> GetAllIssuesByUserStory(string storyName);
        
    }
}