using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oluso.Email.Extensions;
using Oluso.Email.MailKit.Settings;
using Oluso.Email.Settings;

namespace Oluso.Email.MailKit.Extensions;

/// <summary>
/// Service collection related extensions
/// </summary>
public static class MailKitServiceCollectionExtensions
{
    /// <summary>
    /// add mailkit email service to the service collection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddMailKit(this IServiceCollection services,
        Action<MailKitOptions> options)
    {
        var settings = options.ToEmailConfigureOrDefault().EmailSettings;
        var config = settings.IsEqualTypeThrow<IEmailSettings, MailKitSettings>(nameof(settings));
        services.AddMailKit(config);
        return services;
    }
    
    
    /// <summary>
    /// register MailKit sender service in the DI
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static IServiceCollection AddMailKit(this IServiceCollection services, MailKitSettings settings)
    {
        if (settings == null)
            throw new ApplicationException($"Settings could not be null");
        if (settings.Enable == true)
        {
            services.TryAddSingleton<IEmailService>(new MailKitService(settings));
        }
        return services;
    }
}