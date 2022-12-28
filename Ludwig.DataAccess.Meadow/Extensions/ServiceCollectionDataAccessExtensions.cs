using Acidmanic.Utilities.Reflection;
using EnTier.Repositories;
using Ludwig.DataAccess.Contracts.Repositories;
using Ludwig.DataAccess.Meadow.Repositories;
using Ludwig.DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Ludwig.DataAccess.Meadow.Extensions
{
    public static class ServiceCollectionDataAccessExtensions
    {
        private static void AddCustomRepository<TEntity, TId, TAbstract, TCustom>(IServiceCollection services)
            where TCustom : class, TAbstract, ICrudRepository<TEntity, TId>
            where TEntity : class, new()
            where TAbstract : class
        {
            services.AddTransient<TAbstract, TCustom>();

            services.AddTransient<ICrudRepository<TEntity, TId>, TCustom>();
        }


        public static IServiceCollection AddMeadowCustomRepositories(this IServiceCollection services)
        {
            AddCustomRepository<
                AuthorizationRecordDal, long, 
                IAuthorizationRecordRepository,
                AuthorizationRecordRepository>(services);

            
            return services;
        }
    }
}