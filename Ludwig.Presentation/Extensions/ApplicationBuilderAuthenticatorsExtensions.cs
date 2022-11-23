using System;
using Ludwig.Contracts.Authentication;
using Ludwig.Contracts.Di;
using Ludwig.Presentation.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Presentation.Extensions
{
    public static class ApplicationBuilderAuthenticatorsExtensions
    {



        public static IApplicationBuilder UseAuthenticators<TRegistry>(this IApplicationBuilder app)
        where TRegistry:IRegistry,new()
        {

            var authenticatorsReference = app.ApplicationServices.GetService<AuthenticatorsListReference>();

            if (authenticatorsReference == null)
            {
                throw new Exception("Please register AuthenticatorsListReference class in di, at" +
                                    " startup class");
            }
            
            var reg = new TRegistry();

            var authTypes = reg.Authenticators;

            foreach (var authType in authTypes)
            {
                var authenticator = app.ApplicationServices.GetService(authType) as IAuthenticator;

                if (authenticator != null)
                {
                    authenticatorsReference.UseAuthenticator(authenticator);
                }
            }

            return app;
        }
    }
}