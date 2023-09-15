using Oluso.Email.Settings;

namespace Oluso.Email.NetEmail.Settings;

/// <summary>
/// Netcore email settings
/// </summary>
public class NetEmailSettings : EmailSettings
{
    private bool _isExchange = false; // not implement for now

    /// <summary>
    /// get/set true if is exchange server
    /// </summary>
    public bool? IsExchange
    {
        get => _isExchange;
        set
        {
            if (value.HasValue)
                _isExchange = value.Value;
        }
    }
}