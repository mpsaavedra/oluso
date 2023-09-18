using Oluso.Email.Settings;

namespace Oluso.Email.MailKit.Settings;

/// <summary>
/// MailKit configuration options
/// </summary>
public class MailKitOptions : EmailOptions
{
    /// <summary>
    /// returns a new <see cref="MailKitOptions"/> instance
    /// </summary>
    public MailKitOptions() : base()
    {
        EmailSettings = new MailKitSettings();
    }
}