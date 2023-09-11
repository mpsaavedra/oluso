#nullable enable
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Oluso.Extensions;

/// <summary>
/// Json related extensions
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// serialize object to a json string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string ToSerialize<T>(this T value, Action<JsonSerializerOptions>? action = null) =>
        JsonSerializer
            .Serialize(
                value.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(value))),
                typeof(T),
                action!.ToConfigureOrDefault());

    /// <summary>
    /// deserialize a string into an object
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? ToDeserialize<T>(this string value, Action<JsonSerializerOptions>? action = null) =>
        JsonSerializer
            .Deserialize<T>(
                value.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(value))),
                action!.ToConfigureOrDefault());
}