using System.Security.Cryptography;
using System.Text;

namespace Oluso.Configuration.Abstractions.Helpers;

/// <summary>
/// Utility for hashing content
/// </summary>
public static class Hasher
{
    /// <summary>
    /// creates a simple has string from the content bytes, this
    /// function is mostly used to send configuration content over 
    /// the selected publisher
    /// </summary>
    public static string CreateHash(byte[] bytes)
    {
        using var hash = SHA1.Create();
        var hashBytes = hash.ComputeHash(bytes);

        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}