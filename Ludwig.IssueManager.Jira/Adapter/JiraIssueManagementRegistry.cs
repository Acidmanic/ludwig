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
            AddIssueManager<JiraIssueManager>();
            
            Authenticator<UsernamePasswordAuthenticator>();
            
            Transient<IJiraConfigurationProvider,JiraConfigurationProvider>();
            
            Transient<Services.Jira>();
            
            Transient<ICustomFieldDefinitionProvider,LudwigJiraFieldDefinitionProvider>();
            
        }
    }
}