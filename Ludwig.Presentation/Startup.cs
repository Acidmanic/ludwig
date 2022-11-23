using EnTier.Services;
using Ludwig.Contracts.Authentication;
using Ludwig.IssueManager.Fake;
using Ludwig.Presentation.Authentication;
using Ludwig.Presentation.Contracts;
using Ludwig.Presentation.Extensions;
using Ludwig.Presentation.Models;
using Ludwig.Presentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddIssueManagerRegistry<FakeIssueManagerRegistry>();

            services.AddTransient<IBackChannelRequestGrant, AuthenticationManager>();
            
            services.AddControllers();

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
        }
    }
}