using System;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Attributes;

namespace Oluso.Extensions;

/// <summary>
/// auto register services etensions
/// </summary>
public static class ServicesAutoRegisterExtensions
{
    /// <summary>
    /// register all services of the provided type in the DI. Services must include a decorator that
    /// describes the scope. <see cref="ScopedAttribute"/>, <see cref="SingletonAttribute"/>
    /// and <see cref="TransientAttribute"/>
    /// </summary>
    /// <param name="services">services collection DI</param>
    /// <param name="types">Will scan for types from the assemblies of each System.Type in types</param>
    /// <typeparam name="TService">type of service to register</typeparam>
    public static IServiceCollection RegisterServices<TService>(this IServiceCollection services,
        params Type[] types)
    {
        // scoped services
        services.Scan(scan => scan
            .FromAssembliesOf(types)
            .AddClasses(cls => 
                cls.WithAttribute<ScopedAttribute>().AssignableToAny(typeof(TService))
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services.Scan(scan => scan
            .FromAssembliesOf(types)
            .AddClasses(cls => 
                cls.WithAttribute<TransientAttribute>().AssignableToAny(typeof(TService))
            )
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        services.Scan(scan => scan
            .FromAssembliesOf(types)
            .AddClasses(cls => 
                cls.WithAttribute<SingletonAttribute>().AssignableToAny(typeof(TService))
            )
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );

        return services;
    }
}