using System;
using System.Collections.Generic;
using Ludwig.Common.Configuration;
using Ludwig.Contracts.Configurations;

namespace Ludwig.IssueManager.Fake
{
    public class FakeConfigurationDescriptor:IConfigurationDescriptor
    {
        public List<ConfigurationDefinition> ConfigurationItems { get; } = new List<ConfigurationDefinition>
        {
            new Cib<Configurations>().FromProperty(c => c.FakeUrl).TypeString()
                .Description("This is a fake url").Build(),
            new Cib<Configurations>().FromProperty(c => c.FakeUserLimit)
                .TypeInteger().Description("This is a fake number").Build()
        };

        public Type ConfigurationType { get; } = typeof(Configurations);
    }
}