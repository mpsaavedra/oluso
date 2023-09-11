#nullable enable
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace Oluso.Extensions;

/// <summary>
/// byte related extensions
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// execute a gzip compression of provided bytes, it validates that
    /// byes are not null
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static byte[] ToCompress(this byte[] bytes)
    {
        bytes.IsNullOrAnyNullThrow(Messages.NullOrAnyNull(nameof(bytes)));
        using var outputStream = new MemoryStream();
        using (var zip = new GZipStream(outputStream, CompressionMode.Compress))
        {
            zip.Write(bytes, 0, bytes.Length);
        }

        return outputStream.ToArray();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static byte[] DeCompress(this byte[] bytes)
    {
        bytes.IsNullOrAnyNullThrow(Messages.NullOrAnyNull(nameof(bytes)));
        using var outputStream = new MemoryStream();
        using var inputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
        {
            gzipStream.CopyTo(outputStream);
        }
        return outputStream.ToArray();
    }
    
    /// <summary>
    /// decompress bytes and serialize them into an object
    /// </summary>
    /// <param name="bytes"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult? DeCompress<TResult>(this byte[] bytes)
    {
        bytes.IsNullOrAnyNullThrow(Messages.NullOrAnyNull(nameof(bytes)));
        using MemoryStream memoryStream = new();
        var decompressed = bytes.DeCompress();
        memoryStream.Write(decompressed, 0, decompressed.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var serializer = new XmlSerializer(typeof(TResult));
        return (TResult)serializer.Deserialize(memoryStream);
    }

    /// <summary>
    /// turn bytes into an object
    /// </summary>
    /// <param name="bytes"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult? AsObject<TResult>(this byte[] bytes)
    {
        bytes.IsNullOrAnyNullThrow(Messages.NullOrAnyNull(nameof(bytes)));
        using MemoryStream memoryStream = new();
        var serializer = new XmlSerializer(typeof(TResult));
        return (TResult)serializer.Deserialize(memoryStream);
    }
}