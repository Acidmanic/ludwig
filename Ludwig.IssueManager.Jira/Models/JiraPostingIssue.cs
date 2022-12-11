using System.Collections.Generic;

namespace Ludwig.IssueManager.Jira.Models
{
    public class JiraPostingFields : Dictionary<string, object>
    {
        public string Summary
        {
            get=> Get<string>("summary"); 
            set => Set("summary",value);
        }
        
        public string Description
        {
            get=> Get<string>("description"); 
            set => Set("description",value);
        }

        public IdValue IssueType
        {
            get=> Get<IdValue>("issuetype"); 
            set => Set("issuetype",value);
        }

        public IdValue Project { 
            get=> Get<IdValue>("project"); 
            set => Set("project",value);}
        
        public IdValue Priority { 
            get=> Get<IdValue>("priority"); 
            set => Set("priority",value);}


        private T Get<T>(string key)
        {
            if (this.ContainsKey(key))
            {
                return (T)this[key];
            }

            return default;
        }

        private void Set<T>(string key, T value)
        {
            if (this.ContainsKey(key))
            {
                this.Remove(key);
            }

            this.Add(key, value);
        }
    }

    public class JiraPostingIssue
    {
        public JiraPostingFields Fields { get; set; }
    }
}