namespace Oluso.Configuration.Client;

#pragma warning disable CS8618
/// <summary>
/// Configuration parameters to be included in the client, this options
/// define how and where to connect to in the configuration provider.
/// </summary>
public class ConfigurationOptions
{
    /// <summary>
    /// Name or path of the configuration file relative to the configuration provider path.
    /// </summary>
    public string ConfigurationName { get; set; }

    /// <summary>
    /// Determines if loading the file is optional. Defaults to false>.
    /// </summary>
    public bool Optional { get; set; }

    /// <summary>
    /// Determines whether the source will be loaded if the underlying file changes. Defaults to false.
    /// </summary>
    public bool ReloadOnChange { get; set; }

    /// <summary>
    /// The type of <see cref="IConfigurationParser"/> used to parse the remote configuration file.
    /// </summary>
    public IConfigurationParser Parser { get; set; }
}
#pragma warning restore CS8618