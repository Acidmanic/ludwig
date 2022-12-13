using System;
using System.Collections.Generic;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.IssueManagement;

namespace Ludwig.Contracts.Di
{
    public abstract  class RegistryBase:IRegistry
    {
        
        
        public Type IssueManager { get; protected set; }
        
        public Type ConfigurationDescriptor { get; protected set; }

        public List<Type> Authenticators { get; } = new List<Type>();
        
        public List<Type> AdditionalTransientServices { get; } = new List<Type>();
        public List<Type> AdditionalSingletonServices { get; } = new List<Type>();

        public Dictionary<Type, Type> AdditionalTransientInjections { get; set; } = new Dictionary<Type, Type>();
        public Dictionary<Type, Type> AdditionalSingletonInjections { get; set; } = new Dictionary<Type, Type>();

        public List<Type> Exporters { get; } = new List<Type>();

        protected void Transient<TAbstraction, TImplementation>()
        where TImplementation:TAbstraction
        {
            AdditionalTransientInjections.Add(typeof(TAbstraction),typeof(TImplementation));
        }
        
        protected void Singleton<TAbstraction, TImplementation>()
            where TImplementation:TAbstraction
        {
            AdditionalSingletonInjections.Add(typeof(TAbstraction),typeof(TImplementation));
        }
        protected void Transient<TImplementation>()
        {
            AdditionalTransientServices.Add(typeof(TImplementation));
        }
        
        protected void Singleton<TImplementation>()
        {
            AdditionalSingletonServices.Add(typeof(TImplementation));
        }


        protected void Authenticator<TAuth>()
            where TAuth : IAuthenticator
        {
            this.Authenticators.Add(typeof(TAuth));
        }
        
        protected void AddIssueManager<TIssueManager>()
            where TIssueManager : IIssueManager
        {
            this.IssueManager = typeof(TIssueManager);
        }

        protected void Configuration<TDescriptor>()
            where TDescriptor : IConfigurationDescriptor, new()
        {
            this.ConfigurationDescriptor = typeof(TDescriptor);
        }

        protected void Exports<TExporter>()
        {
            this.Exporters.Add(typeof(TExporter));
        }
    }
}