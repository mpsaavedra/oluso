namespace Oluso.Helpers;

/// <summary>
/// A set of common regular expressions, commonly used 
/// </summary>
public static class Regexp
{
    /// <summary>
    /// RegExp for date
    /// </summary>
    //ReSharper disable once InconsistentNaming
    public const string ForDate =
        @"^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))) ?((20|21|22|23|[01]\d|\d)(([:.][0-5]\d){1,2}))?$";

    /// <summary>
    /// RegExp for decimal number
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForDecimal = @"^((-?[1-9]+)|[0-9]+)(\.?|\,?)([0-9]*)$";

    // TODO: it does not support subdomains
    /// <summary>
    /// RegExp for email.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    /// <summary>
    /// RegExp for hexadecimal html colors
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForHex = "^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";

    /// <summary>
    /// RegExp for integer number
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForInteger = "^((-?[1-9]+)|[0-9]+)$";

    /// <summary>
    /// RegExp for Tag. (Html/Xml type of tag)
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForTag = @"^<([a-z1-6]+)([^<]+)*(?:>(.*)<\/\1>| *\/>)$";

    /// <summary>
    /// RegExp for time with no seconds
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForTime = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$";

    /// <summary>
    /// RegExp for Url
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForUrl = @"^((https?|ftps?|file):\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";

    /// <summary>
    /// RegExp for regular names
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForName = @"^[a-zA-Z áàäâãÁÀÄÂÃÅéèëêÉÈËÊíìïîÍÌÏÎóòöôõøÓÒÖÔÕúùüûÚÙÜÛçÇħñÑ]*$";

    /// <summary>
    /// RegExp for a regular subdomain name
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForSubdomain = @"^[-._a-zA-Z0-9]*$";

    /// <summary>
    /// RegExp for regular domain name.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForDomain =
        @"(?=^.{1,254}$)(^(?:(?!\d+\.|-)[a-zA-Z0-9_\-]{1,63}(?<!-)\.?)+(?:[a-zA-Z]{2,})$)";

    /// <summary>
    /// RegExp for regular hostnames
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public const string ForHostname =
        @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
}