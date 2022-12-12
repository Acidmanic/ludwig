using System;
using System.Collections.Generic;
using Ludwig.Contracts.Di;

namespace Ludwig.IssueManager.Fake
{
    public class FakeIssueManagerRegistry:IRegistry
    {
        public Type IssueManager { get; } = typeof(FakeIssueManager);

        public Type ConfigurationDescriptor { get; } = typeof(FakeConfigurationDescriptor);

        public List<Type> Authenticators { get; } = new List<Type>
        {
            typeof(UsernamePasswordAuthenticator),
            typeof(OtpAuthenticator)
        };

        public List<Type> Exporters { get; } = new List<Type>
        {
            typeof(FakeExporter)
        };
        
        public List<Type> AdditionalTransientServices { get; } = new List<Type>();
        public List<Type> AdditionalSingletonServices { get; } = new List<Type>();
        public Dictionary<Type, Type> AdditionalTransientInjections { get; set; } = new Dictionary<Type, Type>();
        public Dictionary<Type, Type> AdditionalSingletonInjections { get; set; } = new Dictionary<Type, Type>();
    }
}