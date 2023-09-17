using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;

namespace Oluso.Configuration.Client;

#pragma warning disable CS8618
/// <summary>
/// represent the remote configuration file as an <see cref="IConfigurationSource"/>
/// </summary>
public class RemoteConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// Configuration service endpoint.
    /// </summary>
    public string ConfigurationServiceUri { get; set; }

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
    /// The <see cref="System.Net.Http.HttpMessageHandler"/> for the <see cref="HttpClient"/>.
    /// </summary>
    public HttpMessageHandler HttpMessageHandler { get; set; }

    /// <summary>
    /// The timeout for the <see cref="HttpClient"/> request to the configuration server.
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// The type of <see cref="IConfigurationParser"/> used to parse the remote configuration file.
    /// </summary>
    public IConfigurationParser? Parser { get; set; }

    /// <summary>
    /// Delegate to create the type of <see cref="ISubscriber"/> used to subscribe to published configuration messages.
    /// </summary>
    public Func<ISubscriber> CreateSubscriber { get; set; }

    /// <summary>
    /// The type used to configure the logging system and create instances of <see cref="ILogger"/>
    /// </summary>
    public ILoggerFactory LoggerFactory { get; set; }

    /// <inheritdoc/>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        //return new RemoteConfigurationProvider(this);
        return new RemoteConfigurationProvider(this);
    }
}
#pragma warning restore CS8618