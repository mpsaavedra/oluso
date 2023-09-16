namespace Oluso.Notifier;

/// <summary>
/// Message related data
/// </summary>
public record MessageData (DateTime CreationDate, MessageLevel Level, string Data, Exception? Exception)
{
    
}