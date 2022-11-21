using Ludwig.IssueManager.Jira.JiraAuthentication;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Presentation.JiraAuthentication
{
    public static class JiraAuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddJiraAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Jira")
                .AddScheme<JiraAuthenticationSchemeOptions, JiraAuthenticationHandler>("Jira", null);

            return services;
        }
    }
}