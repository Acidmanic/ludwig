using System;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Di;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Presentation.Extensions
{
    public static class ApplicationBuilderAuthenticatorsExtensions
    {
        public static IApplicationBuilder UseIssueManagerRegistry<TRegistry>(this IApplicationBuilder app)
            where TRegistry : IRegistry, new()
        {
            var reg = new TRegistry();
            var ludwigConfiguration = app.ApplicationServices.GetService<LudwigConfigurationProvider>();
            var issueManagerAggregator = app.ApplicationServices.GetService<IssueManagerAggregation>();

            if (issueManagerAggregator == null)
            {
                throw new Exception("Register issue manager aggregation.");
            }
            
            var configurationDescriptorType = reg.ConfigurationDescriptor;

            if (configurationDescriptorType != null)
            {
                if (app.ApplicationServices.GetService(configurationDescriptorType) is IConfigurationDescriptor
                    descriptor)
                {
                    ludwigConfiguration?.AddDefinitions(descriptor.ConfigurationDefinitions);
                }
            }

            var authenticatorsReference = app.ApplicationServices.GetService<AuthenticatorsListReference>();

            if (authenticatorsReference == null)
            {
                throw new Exception("Please register AuthenticatorsListReference class in di, at" +
                                    " startup class. IT MUST BE SINGLETON");
            }

            var authTypes = reg.Authenticators;
            var currentIssueManager = app.ApplicationServices.GetService(reg.IssueManager) as IIssueManager;

            if (currentIssueManager == null)
            {
                throw new Exception("Register IssueManager at services extension for registry");
            }
            
            foreach (var authType in authTypes)
            {
                if (app.ApplicationServices.GetService(authType) is IAuthenticator authenticator)
                {
                    authenticatorsReference.UseAuthenticator(authenticator);
                    issueManagerAggregator.Register(currentIssueManager,authenticator);
                }
            }

            return app;
        }
    }
}