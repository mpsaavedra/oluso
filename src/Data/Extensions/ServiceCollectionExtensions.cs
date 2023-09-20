using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.Repositories;

namespace Oluso.Data.Extensions;

/// <summary>
/// ServiceCollection related extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// scan for <see cref="IRepository{TKey,TUserKey,TEntity,TContext}"/> and <see cref="IUnitOfWork{TContext}"/>
    /// implementation to register in the DI container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services, params Type[] types)
    {
        services.Scan(scan => 
            scan
                .FromAssembliesOf(types)
                .AddClasses(cls => cls.AssignableTo(typeof(IRepository<,,,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        services.Scan(scan =>
            scan
                .FromAssembliesOf(types)
                .AddClasses(cls => cls.AssignableToAny(typeof(IUnitOfWork<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        
        return services;
    }
}