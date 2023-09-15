using Microsoft.Extensions.DependencyInjection;
using Oluso.Email.MailKit.Extensions;
using Oluso.Email.Settings;

namespace Oluso.Email.Extensions;

/// <summary>
/// Service collection related extensions
/// </summary>
public static class EmailServiceCollectionExtensions
{
    /// <summary>
    /// add specified email service in the service collection DI
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailService(this IServiceCollection services,
        Action<EmailServiceOptions> options) =>
        services.AddEmailService(options!.ToEmailConfigureOrDefault().EmailServiceSettings);
    
    /// <summary>
    /// Add specified email service in the di container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static IServiceCollection AddEmailService(this IServiceCollection services, EmailServiceSettings settings)
    {
        if(settings == null)
            throw new ApplicationException(Messages.NullOrEmpty(nameof(settings)));
        if (settings.NetEmailSettings?.Enable == true)
        {
            services.AddNetEmailService(settings.NetEmailSettings);
        }

        if (settings.MailKitSettings?.Enable == true)
        {
            services.AddMailKit(settings.MailKitSettings);
        }

        return services;
    }
}