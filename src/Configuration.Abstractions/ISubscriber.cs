using System;

namespace Oluso.Configuration.Abstractions;

/// <summary>
/// basic structure to define a subscriber that connects
/// to a configuration provider
/// </summary>
public interface ISubscriber
{
    /// <summary>
    /// Canonical name of the provider
    /// </summary>
    string Name { get; }

    /// <summary>
    /// initialize the connection with the configuration provider
    /// </summary>
    void Initialize();

    /// <summary>
    /// Subscribe to the configuration service provider
    /// </summary>
    void Subscribe(string topic, Action<string> handler);
}