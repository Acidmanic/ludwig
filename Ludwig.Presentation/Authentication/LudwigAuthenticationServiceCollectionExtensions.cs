using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Presentation.Authentication
{
    public static class LudwigAuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddLudwigTokenAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("ludwig-token")
                .AddScheme<LudwigAuthenticationSchemeOptions, LudwigTokenAuthenticationHandler>("ludwig-token", null);

            return services;
        }
    }
}