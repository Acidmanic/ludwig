using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ludwig.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    // webBuilder.ConfigureServices(services =>
                    //     AddCookieForwarder(services,IsPresent(args,"--fake-cookies"))
                    // );
                });

        //
        // private static bool IsPresent(string[] args, string argument)
        // {
        //     foreach (var arg in args)
        //     {
        //         if (!string.IsNullOrWhiteSpace(arg) && arg.ToLower() == argument.ToLower())
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }
        // private static void AddCookieForwarder(IServiceCollection services, bool useFake)
        // {
        //     if (useFake)
        //     {
        //         services.AddTransient<ICookieForwarder, DevelopmentMockCookieForwarder>();
        //         Console.WriteLine("** Using FAKE Jira Users **");
        //     }
        //     else
        //     {
        //         services.AddTransient<ICookieForwarder, DeployedClientCookieForwarder>();
        //
        //         Console.WriteLine("** Using Actual Jira Users **");
        //     }
        // }
    }
}