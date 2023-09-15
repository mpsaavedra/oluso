using System.Threading.Tasks;

namespace Oluso.Configuration.Abstractions;

/// <summary>
/// define basic operation in order to publish message into the 
/// Configuration provider and publish configuration data to the
/// registered clients.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// initialize a new connection to the message broker
    /// </summary>
    void Initialize();

    /// <summary>
    /// Publish configuration content
    /// </summary>
    Task Publish(string topic, string message);
}