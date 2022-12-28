using System;
using Ludwig.Contracts;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Di;
using Ludwig.Contracts.IssueManagement;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Configuration;
using Ludwig.Presentation.ExporterManagement;
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
            var exporterManager = app.ApplicationServices.GetService<ExporterManager>();

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

            if (exporterManager == null)
            {
                throw new Exception("You must Register ExporterManager as a" +
                                    " singleton in your dir registration (configure services).");
            }
            
            
            foreach (var authType in authTypes)
            {
                if (app.ApplicationServices.GetService(authType) is IAuthenticator authenticator)
                {
                    authenticatorsReference.UseAuthenticator(authenticator);
                    issueManagerAggregator.Register(currentIssueManager,authenticator);

                    foreach (var exporterType in reg.Exporters)
                    {
                        if (app.ApplicationServices.GetService(exporterType) is IExporter exporter)
                        {
                            exporterManager.Register(authenticator, exporter);
                        }
                    }
                }
            }

            return app;
        }
    }
}