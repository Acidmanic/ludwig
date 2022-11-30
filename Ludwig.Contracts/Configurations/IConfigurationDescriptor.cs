using System;
using System.Collections.Generic;

namespace Ludwig.Contracts.Configurations
{
    public interface IConfigurationDescriptor
    {
        List<ConfigurationDefinition> ConfigurationDefinitions { get; }
        
        Type ConfigurationType { get; }
    }
}