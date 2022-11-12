using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Ludwig.Presentation
{
    public class StaticServer
    {

        private readonly string _servingDirectoryName;

        private readonly string _defaultFile;

        private  string _frontDirectory;
        
        private  string _indexFile;
        public StaticServer(string servingDirectoryName, string defaultFile)
        {
            _servingDirectoryName = servingDirectoryName;
            _defaultFile = defaultFile;
        }

        
        public StaticServer(string servingDirectoryName):this(servingDirectoryName,"index.html")
        {
            
        }

        public StaticServer():this("front-end")
        {
            
        }

        public void ConfigurePreRouting(IApplicationBuilder app, IHostEnvironment env)
        {
            _frontDirectory = Path.Combine(env.ContentRootPath, _servingDirectoryName);

            _indexFile = Path.Combine(_frontDirectory, _defaultFile);

            if (!Directory.Exists(_frontDirectory))
            {
                Directory.CreateDirectory(_frontDirectory);


                File.WriteAllText(_indexFile,
                    "<H1>Hello!, Apparently I'm being Updated! will be back soon! :D </H1>");
            }

            var fileProvider = new PhysicalFileProvider(_frontDirectory);


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = "",
                ServeUnknownFileTypes = true
            });
            
        }


        public void ConfigureMappings(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", c => c.Response.WriteAsync(File.ReadAllText(_indexFile)));
            });
        }
    }
}