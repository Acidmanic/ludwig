using System;
using System.Collections.Generic;
using Ludwig.Common.Configuration;
using Ludwig.Contracts.Configurations;
using Ludwig.IssueManager.Gitlab.Configurations;

namespace Ludwig.IssueManager.Gitlab.Adapter
{
    public class GitlabConfigurationDescriptor : IConfigurationDescriptor
    {
        public List<ConfigurationDefinition> ConfigurationDefinitions { get; } = new List<ConfigurationDefinition>
        {
            new Cib<GitlabConfigurations>().FromProperty(c => c.ClientId)
                .Description("The Application Id from gitlab application page.").TypeString()
                .Build(),
            new Cib<GitlabConfigurations>().FromProperty(c => c.ClientSecret)
                .Description("The Client Secret from gitlab application page.").TypeString()
                .Build(),
            new Cib<GitlabConfigurations>().FromProperty(c => c.GitlabInstanceBackChannel)
                .Description("If your ludwig server does have a local network access to your gitlab, you can " +
                             "enter the local address of gitlab instance here, otherwise just put the gitlab " +
                             "server address here.").TypeString()
                .Build(),
            new Cib<GitlabConfigurations>().FromProperty(c => c.GitlabInstanceFrontChannel)
                .Description("Your gitlab instance address.").TypeString()
                .Build(),
            new Cib<GitlabConfigurations>().FromProperty(c => c.GitlabProjectId)
                .Description(
                    "The Id of the your gitlab-project, which it's board (issues) would be connected to ludwig.")
                .TypeString().Build()
        };

        public Type ConfigurationType { get; } = typeof(GitlabConfigurations);
    }
}