using System;
using CoreCommandLine.DotnetDi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.LightWeight;

namespace Ludwig.Development.Tool
{
    class Program
    {
        static void Main(string[] args)
        {

            var logger = new ConsoleLogger().Shorten().EnableAll().Disable(LogLevel.Trace);

            var app = new DotnetCommandlineApplicationBuilder<LdtApplication>()
                .UseStartup<LdtStartup>()
                .Describe("Ludwig Development Tool","This application is for helping development process about inserting, resetting, manipulating data status and etc.")
                .UseLogger(logger)
                .Build();
            
            app.ExecuteInteractive();
        }
    }
}
