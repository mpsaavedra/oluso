using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Oluso.Helpers;

/// <summary>
/// An embedded resource manager helper to load string resources
/// </summary>
public class EmbeddedResourceManager
{
    /// <summary>
    /// returns the string content of the resource in the assembly of the provided type
    /// if no type is provided it read it from current assembly. if resource was not found
    /// it returns null.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static async Task<string?> ReadStringAsync(string resourceName, Type? type = null)
    {
        type ??= typeof(EmbeddedResourceManager);
        var assembly = type.Assembly;
        var resourceStream = assembly?.GetManifestResourceStream(resourceName);
        if (resourceStream == null)
            return null;
        using var reader = new StreamReader(resourceStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// returns the string content of the resource in the assembly of the provided type
    /// if no type is provided it read it from current assembly. if resource was not found
    /// it returns null.
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string? ReadString(string resourceName, Type? type = null) =>
        ReadStringAsync(resourceName, type).Result;
}