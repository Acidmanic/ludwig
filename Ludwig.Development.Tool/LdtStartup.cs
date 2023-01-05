using System.Linq;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;
using CoreCommandLine;
using Ludwig.DataAccess.Meadow;
using Ludwig.DataAccess.Meadow.Extensions;
using Ludwig.Development.Tool.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Development.Tool
{
    public class LdtStartup
    {



        public void ConfigureServices(IServiceCollection services)
        {

            CommandsReg(services);

            services.AddEnTier();
            
            services.AddMeadowUnitOfWork(new MeadowConfigurationProvider());
            
            services.AddMeadowCustomRepositories();

            services.AddTransient<MeadowEngineProvider>();
        }

        private void CommandsReg(IServiceCollection services)
        {
            var assembly = GetType().Assembly;

            var nullCommandType = typeof(NullCommand);
            
            var concreteCommandTypes = assembly.GetAvailableTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && 
                            TypeCheck.Implements<ICommand>(t) && t != nullCommandType);

            foreach (var type in concreteCommandTypes)
            {
                services.AddTransient(type);
            }
        }


        public void Configure(IApplicationBuilder app)
        {
            app.ConfigureEnTierResolver();
        }
    }
}