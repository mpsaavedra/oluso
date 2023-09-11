#nullable enable
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="IApplicationBuilder"/> related extensions
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// returns the application builder <see cref="IHostEnvironment"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostEnvironment? ToEnvironment(this IApplicationBuilder builder) =>
        builder.IsNullOrEmptyThrow(nameof(builder)).ToService<IHostEnvironment>();
    
    /// <summary>
    /// checks if some service is registered as an application service 
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static bool ToExists<TService>(this IApplicationBuilder builder) =>
        builder.IsNullOrEmptyThrow(nameof(builder)).ToService<TService>() != null;
    
    /// <summary>
    /// returns a service of <typeparamref name="TService"/> from the application services container
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService? ToService<TService>(this IApplicationBuilder builder) =>
        builder.IsNullOrEmptyThrow(nameof(builder)).ApplicationServices.GetService<TService>();
}