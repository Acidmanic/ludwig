using System;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Configurations;
using Ludwig.Contracts.Di;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Presentation.Extensions
{
    public static class ApplicationBuilderAuthenticatorsExtensions
    {



        public static IApplicationBuilder UseIssueManagerRegistry<TRegistry>(this IApplicationBuilder app)
        where TRegistry:IRegistry,new()
        {

            var authenticatorsReference = app.ApplicationServices.GetService<AuthenticatorsListReference>();

            if (authenticatorsReference == null)
            {
                throw new Exception("Please register AuthenticatorsListReference class in di, at" +
                                    " startup class. IT MUST BE SINGLETON");
            }
            
            var reg = new TRegistry();

            var authTypes = reg.Authenticators;

            foreach (var authType in authTypes)
            {
                if (app.ApplicationServices.GetService(authType) is IAuthenticator authenticator)
                {
                    authenticatorsReference.UseAuthenticator(authenticator);
                }
            }

            var configurationDescriptorType = reg.ConfigurationDescriptor;

            if (configurationDescriptorType != null)
            {
                if (app.ApplicationServices.GetService(configurationDescriptorType) is IConfigurationDescriptor descriptor)
                {
                    var ludwigConfiguration = app.ApplicationServices.GetService<LudwigConfigurationProvider>();
                    
                    ludwigConfiguration?.AddDefinitions(descriptor.ConfigurationDefinitions);
                }
            }

            return app;
        }
    }
}