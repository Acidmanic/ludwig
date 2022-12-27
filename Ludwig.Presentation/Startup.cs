using System;
using EnTier.Services;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.IssueManagement;
using Ludwig.IssueManager.Fake;
using Ludwig.IssueManager.Gitlab.Adapter;
using Ludwig.IssueManager.Jira.Adapter;
using Ludwig.Presentation.Administration;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Configuration;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.ExporterManagement;
using Ludwig.Presentation.Extensions;
using Ludwig.Presentation.Mapping;
using Ludwig.Presentation.Models;
using Ludwig.Presentation.Services;
using Ludwig.Presentation.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            services.AddAutoMapperForEnTier(conf => conf.AddProfile<LudwigMappingProfile>());
            
            services.AddJsonFileUnitOfWork();

            services.AddTransient<ICrudService<UserStory, long>, UserStoryService>();

            services.AddTransient<IUserStoryService, UserStoryService>();

            services.AddHttpContextAccessor();

            services.AddTransient<IDatabaseExporter, DatabaseExporterV1>();

            services.AddTransient<AuthenticationStore>();

            services.AddTransient<AuthenticationManager>();

            services.AddLudwigTokenAuthentication();

            services.AddSingleton<AuthenticatorsListReference>();

            services.AddSingleton<ExporterManager>();

            var issueManagerAggregationSingleton = new Wrap<IssueManagerAggregation>();
            services.AddSingleton(sp => CreateIssueManagerAggregation(issueManagerAggregationSingleton, sp));
            services.AddSingleton<IIssueManager, IssueManagerAggregation>(sp =>
                CreateIssueManagerAggregation(issueManagerAggregationSingleton, sp));

            services.AddIssueManagerRegistry<GitlabIssueManagerRegistry>();
            services.AddIssueManagerRegistry<JiraIssueManagementRegistry>();
            services.AddIssueManagerRegistry<FakeIssueManagerRegistry>();
            services.AddIssueManagerRegistry<AdministrationRegistry>();

            var configurationProvider = new LudwigConfigurationProvider();

            services.AddSingleton<IConfigurationProvider>(o => configurationProvider);

            services.AddSingleton(o => configurationProvider);

            services.AddTransient<IBackChannelRequestGrant, AuthenticationManager>();

            var logger = new ConsoleLogger().EnableAll();

            _frontEndServer.UseLogger(logger);
            
            services.AddTransient<ILogger>(p => logger);

            services.AddControllers();

            services.AddTransient<Storage.Storage>();

            services.AddTransient<LoginMethodFilterService>();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(LudwigPolicies.AdministratorsOnly,
                    p => p.RequireClaim(LudwigClaims.UserScheme,
                        LudwigClaims.UserSchemes.Administrator));
                options.AddPolicy(LudwigPolicies.IssueManagersOnly,
                    p => p.RequireClaim(LudwigClaims.UserScheme,
                        LudwigClaims.UserSchemes.IssueManager));
                options.AddPolicy(LudwigPolicies.AdministratorOrIssueManager,
                    p => p.RequireClaim(LudwigClaims.UserScheme,
                        LudwigClaims.UserSchemes.IssueManager,
                        LudwigClaims.UserSchemes.Administrator));
            });
            
        }

        private IssueManagerAggregation CreateIssueManagerAggregation(Wrap<IssueManagerAggregation> instance,
            IServiceProvider serviceProvider)
        {
            if (instance.Value == null)
            {
                var contextAcc = serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

                instance.Value = new IssueManagerAggregation(contextAcc);
            }

            return instance.Value;
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

            app.UseIssueManagerRegistry<GitlabIssueManagerRegistry>();
            app.UseIssueManagerRegistry<JiraIssueManagementRegistry>();
            app.UseIssueManagerRegistry<FakeIssueManagerRegistry>();
            app.UseIssueManagerRegistry<AdministrationRegistry>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            _frontEndServer.ConfigureMappings(app, env);

            app.ConfigureEnTierResolver();
        }
    }
}