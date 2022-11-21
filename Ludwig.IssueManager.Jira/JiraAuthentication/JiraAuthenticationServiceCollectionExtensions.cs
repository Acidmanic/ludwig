using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.IssueManager.Jira.JiraAuthentication
{
    internal static class JiraAuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddJiraAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Jira")
                .AddScheme<JiraAuthenticationSchemeOptions, JiraAuthenticationHandler>("Jira", null);

            return services;
        }
    }
}