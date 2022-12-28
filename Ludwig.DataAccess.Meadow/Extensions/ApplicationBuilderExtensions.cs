using System.Reflection;
using Meadow;
using Meadow.MySql;
using Meadow.Scaffolding.Contracts;
using Microsoft.AspNetCore.Builder;

namespace Ludwig.DataAccess.Meadow.Extensions
{
    public static class ApplicationBuilderExtensions
    {



        public static IApplicationBuilder ConfigureMeadowDatabase
            (this IApplicationBuilder app,Assembly assembly,bool deleteDatabase = false)
        {

            var configurationProviderObject = app.ApplicationServices.GetService(typeof(IMeadowConfigurationProvider));

            if (configurationProviderObject is IMeadowConfigurationProvider configurationProvider)
            {
                var engine = new MeadowEngine(configurationProvider.GetConfigurations(),assembly);

                engine.UseMySql();

                if (deleteDatabase)
                {
                    if (engine.DatabaseExists())
                    {
                        engine.DropDatabase();
                    }
                }

                engine.CreateIfNotExist();
                
                engine.BuildUpDatabase();
            }

            return app;
        }
    }
}