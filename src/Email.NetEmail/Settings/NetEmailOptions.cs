using Oluso.Email.Settings;

namespace Oluso.Email.NetEmail.Settings;

/// <summary>
/// <inheritdoc cref="EmailOptions"/> specifically for netcore email
/// </summary>
public class NetEmailOptions : EmailOptions
{
    /// <summary>
    /// returns a new <see cref="NetEmailOptions"/> instance
    /// </summary>
    public NetEmailOptions()
    {
        EmailSettings = new NetEmailSettings();
    }
}