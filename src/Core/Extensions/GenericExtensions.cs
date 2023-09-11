#nullable enable
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Oluso.Extensions;

/// <summary>
/// generic extensions
/// </summary>
public static class GenericExtensions
{
    /// <summary>
    /// returns the default value of provided type
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static object? ToDefaultValue<T>(this T value) =>
        value!.GetType().ToDefaultValue();
    
    /// <summary>
    /// convert into bytes current value
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static byte[] ToBytes<T>(this T value) where T : class
    {
        using var memoryStream = new MemoryStream();
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(memoryStream, value);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// returns a dictionary with properties information
    /// </summary>
    /// <param name="input"></param>
    /// <param name="includeNullOrEmptyProperties"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IDictionary? ToDictionaryPropertyInfo<T>(this T input, bool includeNullOrEmptyProperties = false)
    {
        input.IsNullOrAnyNull(Messages.NullOrEmpty(nameof(input)));
        return input!.GetType().GetProperties()
            .Where(property =>
            {
                var name = property.Name;
                var value = property.GetValue(input, null);
                if (!includeNullOrEmptyProperties)
                {
                    return value is bool ||
                           (value is string && !string.IsNullOrWhiteSpace(value?.ToString())) ||
                           (value is null && !value!.Equals(value.ToDefaultValue()));
                }
                return true;
            })
            .ToDictionary(
                property => property.Name.ToCapitalize(),
                property => property.GetValue(input, null)?.ToString());
    }
}