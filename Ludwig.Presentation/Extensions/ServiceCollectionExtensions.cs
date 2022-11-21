using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Di;
using Ludwig.Contracts.IssueManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {




        public static IServiceCollection AddIssueManagerRegistry<TRegistry>(this IServiceCollection services)
        where TRegistry:IRegistry,new()
        {

            var reg = new TRegistry();

            services.AddSingleton(typeof(IIssueManager), reg.IssueManager);

            foreach (var authenticator in reg.Authenticators)
            {
                services.AddSingleton(authenticator);    
            }


            foreach (var injection in reg.AdditionalSingletonInjections)
            {
                services.AddSingleton(injection.Key, injection.Value);
            }
            
            foreach (var injection in reg.AdditionalTransientInjections)
            {
                services.AddTransient(injection.Key, injection.Value);
            }
            
            foreach (var service in reg.AdditionalTransientServices)
            {
                services.AddTransient(service);
            }
            foreach (var service in reg.AdditionalSingletonServices)
            {
                services.AddSingleton(service);
            }
            return services;
        }
    }
}