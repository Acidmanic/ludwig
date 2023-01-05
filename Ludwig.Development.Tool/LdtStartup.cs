using Ludwig.DataAccess.Meadow;
using Ludwig.DataAccess.Meadow.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Development.Tool
{
    public class LdtStartup
    {



        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMeadowUnitOfWork(new MeadowConfigurationProvider());
            
            services.AddMeadowCustomRepositories();
            
            
        }


        public void Configure(IApplicationBuilder app)
        {
            app.ConfigureMeadowDatabase(typeof(MeadowConfigurationProvider).Assembly);

        }
    }
}