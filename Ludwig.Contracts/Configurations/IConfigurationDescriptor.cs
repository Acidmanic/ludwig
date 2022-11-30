using System;
using System.Collections.Generic;

namespace Ludwig.Contracts.Configurations
{
    public interface IConfigurationDescriptor
    {
        List<ConfigurationItem> ConfigurationItems { get; }
        
        Type ConfigurationType { get; }
    }
}