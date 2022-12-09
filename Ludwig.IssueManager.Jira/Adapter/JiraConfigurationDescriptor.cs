using System;
using System.Collections.Generic;
using Ludwig.Common.Configuration;
using Ludwig.Contracts.Configurations;
using Ludwig.IssueManager.Jira.Configuration;

namespace Ludwig.IssueManager.Jira.Adapter
{
    public class JiraConfigurationDescriptor:IConfigurationDescriptor
    {

        private static readonly ConfigurationItemBuilder<JiraConfiguration> Builder = new Cib<JiraConfiguration>();

        public List<ConfigurationDefinition> ConfigurationDefinitions { get; } = new List<ConfigurationDefinition>
        {
            Builder.Clear().FromProperty( c => c.JiraBackChannelUrl)
                .Description("If your ludwig application server does have an internal access to your jira instance " +
                             "server, different than the public address you reach jira with, you can use that here. " +
                             "(for example if you have both jira and ludwig application on the same machine, you can " +
                             "put http://localhost in here).").TypeString()
                .Build(),
            Builder.Clear().FromProperty(c => c.JiraFrontChannelUrl)
                .Description("This would be the address of your jira instance server.").TypeString()
                .Build(),
            Builder.Clear().FromProperty(c => c.JiraProject)
                .Description("The name of the jira project you are trying to sync it's issues with your ludwig " +
                             "application.")
                .Build()
        };

        public Type ConfigurationType { get; } = typeof(JiraConfiguration);
    }
}