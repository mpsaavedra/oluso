namespace Oluso.Configuration.Client;

/// <summary>
/// Structure for a configuration parser
/// </summary>
public interface IConfigurationParser
{
    /// <summary>
    /// receive and stream with the configuration data and
    /// parse it into a <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    IDictionary<string, string> Parse(Stream input);
}