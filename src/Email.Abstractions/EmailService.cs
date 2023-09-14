using Oluso.Email.Extensions;
using Oluso.Email.Settings;

namespace Oluso.Email;

/// <summary>
/// Email service functionalities
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send and email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> SendEmailAsync(Models.Email email);
}

/// <inheritdoc cref="IEmailService"/>
public abstract class EmailService : IEmailService
{
    /// <summary>
    /// returns a new <see cref="EmailService"/> instance
    /// </summary>
    /// <param name="settings"></param>
    public EmailService(IEmailSettings settings) => EmailSettings = settings;
    
    /// <summary>
    /// get/set the service settings
    /// </summary>
    public IEmailSettings EmailSettings { get; set; }
    
    /// <inheritdoc cref="IEmailService.SendEmailAsync"/>
    public abstract Task<bool> SendEmailAsync(Models.Email email);
    
    /// <summary>
    /// setup provider checking the provider type
    /// </summary>
    protected void SetupServer()
    {
        var provider = EmailSettings.Provider.ToEmailProvider();
        if (provider == EmailProvider.Gmail)
        {
            EmailSettings.Host = EmailProvider.Gmail.Host;
            EmailSettings.Port = EmailProvider.Gmail.Port;
            EmailSettings.UseSsl = true;
            EmailSettings.UseCredentials = true;
            return;
        }
        if (provider == EmailProvider.Hotmail)
        {
            EmailSettings.Host = EmailProvider.Hotmail.Host;
            EmailSettings.Port = EmailProvider.Hotmail.Port;
            EmailSettings.UseSsl = true;
            EmailSettings.UseCredentials = true;
            return;
        }
        if (provider == EmailProvider.Office365)
        {
            EmailSettings.Host = EmailProvider.Office365.Host;
            EmailSettings.Port = EmailProvider.Office365.Port;
            EmailSettings.UseSsl = true;
            EmailSettings.UseCredentials = true;
            return;
        }
        if (provider == EmailProvider.Aws)
        {
            EmailSettings.Host = EmailProvider.Aws.Host.ToParseHost(EmailSettings.Host!);
            EmailSettings.Port = EmailProvider.Aws.Port;
            EmailSettings.UseSsl = true;
            EmailSettings.UseCredentials = true;
        }
    }
}