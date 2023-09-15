using Oluso.Email.MailKit.Settings;
using Oluso.Email.NetEmail.Settings;

namespace Oluso.Email.Settings;

#pragma warning disable CS8618

/// <summary>
/// Email service configurations
/// </summary>
public class EmailServiceSettings
{
    /// <summary>
    /// <see cref="NetEmailSettings"/>
    /// </summary>
    public NetEmailSettings NetEmailSettings { get; set; }
    
    /// <summary>
    /// <see cref="MailKitSettings"/>
    /// </summary>
    public MailKitSettings MailKitSettings { get; set; }
}

#pragma warning restore CS8618