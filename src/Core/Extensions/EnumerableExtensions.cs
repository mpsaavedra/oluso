#nullable enable
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Oluso.Extensions;

/// <summary>
/// Enumerable related extensions
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// returns all elements in the source as a single string separated by separtor
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="separator"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string ToString<T>(this IEnumerable<T> enumerable, string separator = ",") =>
        string.Join(separator, enumerable.IsNullOrEmptyThrow(nameof(enumerable)).ToArray());

    /// <summary>
    /// returns and empty enumerable if source is null
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T>? enumerable) =>
        enumerable ?? Enumerable.Empty<T>();

    /// <summary>
    /// returns a ReadOnlyCollection from provided enumerable source
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> enumerable) =>
        !(enumerable is ReadOnlyCollection<T> collection)
            ? new List<T>(enumerable).AsReadOnly()
            : new List<T>(collection).AsReadOnly();
}