using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.AutoMapping;
using Oluso.Data.Repositories;

namespace Oluso.Data.Extensions;

/// <summary>
/// ServiceCollection related extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// scan for <see cref="IRepository{TKey,TUserKey,TEntity,TContext}"/> implementation to register
    /// in the DI container
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
        
        return services;
    }
    
    /// <summary>
    /// scan for  <see cref="IUnitOfWork{TContext}"/> implementation to register in the DI container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, params Type[] types)
    {
        services.Scan(scan =>
            scan
                .FromAssembliesOf(types)
                .AddClasses(cls => cls.AssignableToAny(typeof(IUnitOfWork<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        
        return services;
    }

    /// <summary>
    /// register the automapper and load all Dto classes that inherit from <see cref="IMapFrom{T}<>"/>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IServiceCollection AddAutoMapping(this IServiceCollection services, params Type[] types)
    {
        types = types.Append(typeof(Oluso.Data.AutoMapping.AutoMapping)).ToArray();
        return services.AddAutoMapper(types);
    }
}