using System;
using System.Linq;
using System.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Ludwig.Common.ReferenceFactory
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFactory<TRef, TArg>(this IApplicationBuilder app)
        {
            return UseFactory(app, typeof(TRef), typeof(TArg));
        }


        public static IApplicationBuilder UseFactory(this IApplicationBuilder app, Type referenceType,
            Type argumentType)
        {
            app.ApplicationServices.UseFactory(referenceType, argumentType);

            return app;
        }

        public static IServiceProvider UseFactory<TRef, TArg>(this IServiceProvider provider,
            Type referenceType, Type argumentType)
        {
            return UseFactory(provider, typeof(TRef), typeof(TArg));
        }

        public static IServiceProvider UseFactory(this IServiceProvider provider, Type referenceType, Type argumentType)
        {

            FindReference(provider, referenceType, argumentType);

            return provider;
        }
        
        private static ITypeReference FindReference(this IServiceProvider provider, Type referenceType, Type argumentType)
        {

            var factoryAbstraction = typeof(IReferencedFactory<,>)
                .MakeGenericType(referenceType, argumentType);

            var registeredFactory = provider.GetService(factoryAbstraction);

            if (registeredFactory == null)
            {
                throw new Exception($"You MUST register a factory that implements " +
                                    $"IReferencedFactory<{referenceType.Name},{argumentType.Name}>.");
            }
            // It definitely is
            if (registeredFactory is ITypeReference reference)
            {

                return reference;
            }

            return null;
        }


        private static void AddApplicationPart(Assembly assembly, ITypeReference reference)
        {
            var types = assembly.GetAvailableTypes()
                .Where(IsReferencedFactory);
            
            reference.AvailableImplementations.AddRange(types);
        }

        private static bool IsReferencedFactory(Type type)
        {

            var generic = typeof(IReferencedFactory<,>);

            var interfaces = type.GetInterfaces();

            foreach (var i in interfaces)
            {
                if (i.GetGenericTypeDefinition() == generic)
                {
                    return true;
                }
            }

            return false;
        }
    }
}