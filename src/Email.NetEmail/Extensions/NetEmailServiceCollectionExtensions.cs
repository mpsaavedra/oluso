using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oluso.Email.Extensions;
using Oluso.Email.NetEmail;
using Oluso.Email.NetEmail.Settings;
using Oluso.Email.Settings;

// ReSharper disable once CheckNamespace
namespace Oluso.Email.Extensions;

/// <summary>
/// Service collection related extensions
/// </summary>
public static class NetEmailServiceCollectionExtensions
{
    /// <summary>
    /// add net email service to the service collection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddNetMailService(this IServiceCollection services,
        Action<NetEmailOptions> options)
    {
        var settings = options!.ToEmailConfigureOrDefault().EmailSettings as NetEmailSettings;
        var cfg = settings.IsEqualTypeThrow<IEmailSettings, NetEmailSettings>(nameof(settings));
        services.AddNetEmailService(cfg);
        return services;
    }

    /// <summary>
    /// register the netcore email service to the DI
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static IServiceCollection AddNetEmailService(this IServiceCollection services, NetEmailSettings settings)
    {
        if (settings == null)
            throw new ApplicationException($"Settings could not be null");
        if (settings.Enable.HasValue && settings.Enable.Value)
        {
            services.TryAddSingleton<IEmailService>(new NetEmailService(settings));
        }

        return services;
    }
}