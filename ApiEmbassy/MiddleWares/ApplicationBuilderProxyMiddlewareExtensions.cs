using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using ApiEmbassy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiEmbassy.MiddleWares
{
    public static class ApplicationBuilderProxyMiddlewareExtensions
    {
        public static IApplicationBuilder UseProxy(this IApplicationBuilder app, string redirectionHost)
        {
            app.Use(async (context, next) =>
            {
                var request = await TransmissionConvert.ToRequestRecord(context);

                request.EmbassyId = "";

                if (context.Request.Path.HasValue)
                {
                    var segments = context.Request.Path.Value.Split("/", StringSplitOptions.RemoveEmptyEntries);

                    if (segments.Length > 0)
                    {
                        request.EmbassyId = segments[0].ToLower().Trim();
                    }
                }

                var requestMessage = TransmissionConvert.ToRequestMessage(request);

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(redirectionHost),
                };

                var response = await httpClient.SendAsync(requestMessage);

                await TransmissionConvert.IntoHttpContext(context, response);
            });

            return app;
        }
        
        public static IApplicationBuilder UseRemoteProxy(this IApplicationBuilder app)
        {

            var embassy = app.ApplicationServices.GetService(typeof(Embassy)) as Embassy;
            
            app.Use(async (context, next) =>
            {

                if (embassy == null)
                {
                    Console.WriteLine("There is a problem with injecting embassy.");

                    await next.Invoke();
                }
                else
                {
                    var ambassador = embassy.GetAmbassador();
                
                    await ambassador.Communicate(context);    
                }

            });

            return app;
        }
    }
}