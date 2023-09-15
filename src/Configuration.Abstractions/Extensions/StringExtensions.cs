namespace Oluso.Configuration.Abstractions.Extensions;

/// <summary>
/// String related extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// check if source is null, empty or whitespace, if it's it throw and exception with provided
    /// message if message es null exception will use a default message. 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool ToNullEmptyOrWhitespaceThrow(this string source, string? message = null)
    {
        if (!string.IsNullOrEmpty(source) && !string.IsNullOrWhiteSpace(source)) 
            return true;
        
        message ??= $"Could not be null, empty or whitespace";
        throw new ApplicationException(message);
    }
}