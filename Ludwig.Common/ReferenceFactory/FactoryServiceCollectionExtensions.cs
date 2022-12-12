using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.Common.ReferenceFactory
{
    public static class FactoryServiceCollectionExtensions
    {
        public static IServiceCollection AddReferencedFactory<TAbstraction, TImplementation>(this IServiceCollection services) where TAbstraction : class where TImplementation : class, TAbstraction
        {
            var abstractionType = typeof(TAbstraction);

            var genericFactory = typeof(IReferencedFactory<,>);

            var factoryInterface = abstractionType.GetInterfaces()?
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericFactory);

            if (factoryInterface == null)
            {
                throw new Exception($"{abstractionType.Name} MUST to implement IReferencedFactory<TRef,TArg>");
            }

            services.AddSingleton<TAbstraction, TImplementation>();

            services.AddSingleton(factoryInterface, typeof(TImplementation));

            return services;
        }


        public static IServiceCollection AddReferencedFactory<TReference, TArg, TImplementation>
            (this IServiceCollection services)
        {
            return AddReferencedFactory(services, typeof(TReference), typeof(TArg), typeof(TImplementation));
        }

        public static IServiceCollection AddReferencedFactory(this IServiceCollection services, Type referenceType,
            Type argType, Type implementationType)
        {
            
            var genericType = typeof(IReferencedFactory<,>);

            var factoryAbstraction = genericType.MakeGenericType(referenceType, argType);

            services.AddSingleton(factoryAbstraction, implementationType);
            
            return services;
        }
    }
}