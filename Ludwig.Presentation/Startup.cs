using EnTier.Extensions;
using EnTier.Services;
using Ludwig.Contracts.Authentication;
using Ludwig.IssueManager.Gitlab.Adapter;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Configuration;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Extensions;
using Ludwig.Presentation.Models;
using Ludwig.Presentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.LightWeight;
using IConfigurationProvider = Ludwig.Contracts.Configurations.IConfigurationProvider;

namespace Ludwig.Presentation
{
    public class Startup
    {
        private StaticServer _frontEndServer = new StaticServer().ServeForAnguler();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJsonFileUnitOfWork();

            services.AddTransient<ICrudService<UserStory, long>, UserStoryService>();
            
            services.AddTransient<IUserStoryService, UserStoryService>();

            services.AddHttpContextAccessor();

            services.AddTransient<IDatabaseExporter, DatabaseExporterV1>();
            
            services.AddTransient<AuthenticationStore>();
            
            services.AddTransient<AuthenticationManager>();

            services.AddLudwigTokenAuthentication();
            
            services.AddSingleton<AuthenticatorsListReference>();

            services.AddIssueManagerRegistry<GitlabIssueManagerRegistry>();

            var configurationProvider = new LudwigConfigurationProvider();
            
            services.AddSingleton<IConfigurationProvider>(o => configurationProvider);
            
            services.AddSingleton(o => configurationProvider);

            services.AddTransient<IBackChannelRequestGrant, AuthenticationManager>();
            
            var logger = new ConsoleLogger().EnableAll();

            _frontEndServer.UseLogger(logger);
            
            logger.UseLoggerForEnTier();

            services.AddTransient<ILogger>(p => logger);
            
            services.AddControllers();

            services.AddTransient<Storage.Storage>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(LudwigPolicies.AdministratorsOnly,
                    p => p.RequireClaim(LudwigClaimTypes.Administrator));
                options.AddPolicy(LudwigPolicies.IssueManagersOnly,
                    p => p.RequireClaim(LudwigClaimTypes.IssueManager));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            _frontEndServer.ConfigurePreRouting(app, env);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            _frontEndServer.ConfigureMappings(app, env);

            app.IntroduceDotnetResolverToEnTier();

            app.UseIssueManagerRegistry<GitlabIssueManagerRegistry>();

        }
    }
}
