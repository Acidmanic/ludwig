using System;
using System.Linq;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;
using CoreCommandLine;
using EnTier.Extensions;
using Ludwig.DataAccess.Meadow;
using Ludwig.DataAccess.Meadow.Extensions;
using Ludwig.Development.Tool.Services;
using Meadow.Extensions;
using Meadow.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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


        public void ConfigureServices(IServiceProvider provider,ILogger logger)
        {

            logger.UseForMeadow();
            
            var engine = provider.GetService<MeadowEngineProvider>()?.ProvideEngine();

            engine.UseMySql();
            
            provider.ConfigureEnTierResolver();
        }
    }
}