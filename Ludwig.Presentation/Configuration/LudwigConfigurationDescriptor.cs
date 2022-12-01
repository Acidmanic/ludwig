using System;
using System.Collections.Generic;
using Ludwig.Common.Configuration;
using Ludwig.Contracts.Configurations;

namespace Ludwig.Presentation.Configuration
{
    public class LudwigConfigurationDescriptor:IConfigurationDescriptor
    {
        public List<ConfigurationDefinition> ConfigurationDefinitions { get; }
            = new List<ConfigurationDefinition>
            {
                new Cib<LudwigConfigurations>().FromProperty(c => c.LudwigAddress)
                    .Description("This would be the address you reach Ludwig with.")
                    .TypeString().Build()
            };

        public Type ConfigurationType { get; } = typeof(LudwigConfigurations);
    }
}