namespace Oluso.Configuration.Abstractions.Events;

/// <summary>
/// event to send between hosting and client
/// </summary>
public class ConfigurationEventMessage
{
    /// <summary>
    /// topic to publish to
    /// </summary>
    public string Topic { get; set; } = "";
    
    /// <summary>
    /// message content to publish
    /// </summary>
    public string Message { get; set; } = "";
}