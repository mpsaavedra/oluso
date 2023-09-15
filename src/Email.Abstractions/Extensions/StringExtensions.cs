using System.Text;
using System.Text.RegularExpressions;
using Oluso.Email.Settings;

namespace Oluso.Email.Extensions;

/// <summary>
/// string related extensions
/// </summary>
public static class StringExtensions
{
    // TODO: it does not support subdomains
    /// <summary>
    /// RegExp for email.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    /// <summary>
    /// Validate if source is a valida email address
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsEmail(this string source) => Regex.IsMatch(source, ForEmail);
    
    /// <summary>
    /// replace all coincidences of the given original value with
    /// the provided value
    /// </summary>
    /// <param name="val">string to replace values from</param>
    /// <param name="original">value to be replace</param>
    /// <param name="replacement">value to replace with</param>
    /// <returns></returns>
    public static string ReplaceAll(this string val, string original, string replacement)
    {
        StringBuilder sb = new StringBuilder(val);
        sb.Replace(original, replacement);

        return sb.ToString();
    }

    /// <summary>
    /// returns a valid <see cref="EmailProvider"/> from his name. Available providers are
    /// <li>Gmail</li>
    /// <li>Hotmail</li>
    /// <li>Office365</li>
    /// <li>Aws</li>
    /// <li>Custom</li>
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static EmailProvider ToEmailProvider(this string? provider) =>
        provider switch
        {
            "Gmail" => EmailProvider.Gmail,
            "Hotmail" => EmailProvider.Hotmail,
            "Office365" => EmailProvider.Aws,
            _ => EmailProvider.Custom
        };

    /// <summary>
    /// parse host name 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="host"></param>
    /// <returns></returns>
    public static string ToParseHost(this string source, string host) =>
        string.Format(source, host);
}