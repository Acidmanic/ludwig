using System;
using System.Collections.Generic;

namespace Ludwig.Contracts.Di
{
    public interface IRegistry
    {
        
        
        Type IssueManager { get; }
        
        Type ConfigurationDescriptor { get; }
        
        List<Type> Authenticators { get; }
        
        List<Type> AdditionalTransientServices { get; }
        
        List<Type> AdditionalSingletonServices { get; }
        
        Dictionary<Type,Type> AdditionalTransientInjections { get; set; }
        
        Dictionary<Type,Type> AdditionalSingletonInjections { get; set; }
    }
}