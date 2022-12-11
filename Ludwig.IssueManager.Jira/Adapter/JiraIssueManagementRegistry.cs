using System;
using System.Collections.Generic;
using Ludwig.Contracts.Di;
using Ludwig.IssueManager.Jira.Configuration;
using Ludwig.IssueManager.Jira.Interfaces;
using Ludwig.IssueManager.Jira.Services;

namespace Ludwig.IssueManager.Jira.Adapter
{
    public class JiraIssueManagementRegistry:RegistryBase
    {

        public JiraIssueManagementRegistry()
        {
            Configuration<JiraConfigurationDescriptor>();
            
            AddIssueManager<JiraIssueManager>();
            
            Authenticator<JiraByCredentials>();
            
            Authenticator<JiraCookieHijack>();

            Transient<Services.Jira.Jira>();
            
            Transient<ICustomFieldDefinitionProvider,LudwigJiraFieldDefinitionProvider>();
            
        }
    }
}