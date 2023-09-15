namespace Oluso.Configuration.Abstractions;

/// <summary>
/// define operations executed by a Configuration provider. THe most important
/// methods should be the watch method, that is responsible to observe for Configuration
/// changes and launch the onChange event, and notify to clients using the selected
/// <see cref="IPublisher"/>
/// </summary>
public interface IProvider
{
    /// <summary>
    /// canonical name of provider
    /// </summary>
    string Name { get; }

    /// <summary>
    /// watch for changes in configuration files
    /// </summary>
    Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default);

    /// <summary>
    /// initialize a new connection with the provider
    /// </summary>
    void Initialize();

    /// <summary>
    /// returns the configuration content by his name, it returns the configuration content 
    /// as a byte array so it could be send to the clients
    /// </summary>
    Task<byte[]> GetConfiguration(string name);

    /// <summary>
    /// hashed the configuration content as a string
    /// </summary>
    Task<string> GetHash(string name);

    /// <summary>
    /// List all configuration path existing in the provider
    /// </summary>
    Task<IEnumerable<string>> ListPaths();
}