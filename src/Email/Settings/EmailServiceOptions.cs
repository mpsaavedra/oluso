using Microsoft.Extensions.Configuration;
using Oluso.Email.Extensions;
using Oluso.Email.MailKit.Settings;
using Oluso.Email.NetEmail.Settings;

namespace Oluso.Email.Settings;

/// <summary>
/// Options to be configure in the configuration action
/// </summary>
public class EmailServiceOptions
{
    /// <summary>
    /// <see cref="EmailServiceSettings"/>
    /// </summary>
    public EmailServiceSettings EmailServiceSettings { get; private set; } = new ();

    /// <summary>
    /// load configuration from the appsettings
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public EmailServiceOptions WithNetEmail(IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(EmailServiceSettings))?
            .GetSection(nameof(NetEmailSettings))?
            .Get<NetEmailSettings>();
        if (settings == null)
            throw new ArgumentNullException(nameof(configuration));
        return WithNetEmail(settings!);
    }

    /// <summary>
    /// set the NetEmailSettings options through the action
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public EmailServiceOptions WithNetEmail(Action<NetEmailOptions> options)
    {
        var settings = options!.ToEmailConfigureOrDefault().EmailSettings;
        var config = settings.IsEqualTypeThrow<IEmailSettings, NetEmailSettings>(nameof(settings));
        return WithNetEmail(config);
    }
    
    /// <summary>
    /// set the NetEmailSettings 
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public EmailServiceOptions WithNetEmail(NetEmailSettings settings)
    {
        EmailServiceSettings.NetEmailSettings = settings;
        return this;
    }

    /// <summary>
    /// set MailKit settings loaded from configuration
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public EmailServiceOptions WithMailKit(IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(EmailServiceSettings))?
            .GetSection(nameof(MailKitSettings))?
            .Get<MailKitSettings>();
        if (settings == null)
            throw new ArgumentNullException(nameof(configuration));
        return WithMailKit(settings!);
    }

    /// <summary>
    /// set MailKit options through the action
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public EmailServiceOptions WithMailKit(Action<MailKitOptions> options)
    {
        var settings = options!.ToEmailConfigureOrDefault().EmailSettings;
        var config = settings.IsEqualTypeThrow<IEmailSettings, MailKitSettings>(nameof(settings));
        return WithMailKit(config);
    }

    /// <summary>
    /// set MailKitSettings
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public EmailServiceOptions WithMailKit(MailKitSettings settings)
    {
        EmailServiceSettings.MailKitSettings = settings;
        return this;
    }
}