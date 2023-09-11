#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Oluso.Helpers;

/// <summary>
/// Enum related utilities
/// </summary>
public class Enums
{
    /// <summary>
    /// amount of members in the enum
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static int ToCount<TEnum>() where TEnum : Enum =>
        Enum.GetValues(typeof(TEnum)).Length;

    /// <summary>
    /// returns an enumerable list of the name of the enum members
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static IEnumerable<string> ToValues<TEnum>() where TEnum : struct, Enum =>
        Enum.GetValues(typeof(TEnum)).OfType<Enum>().Select(x => x.ToString());

    /// <summary>
    ///  returns a list of a tuple with the value, name and if the member is selected by default
    /// <b>This function is only valid on int indexed Enums</b>
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="selected"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static IEnumerable<(int Value, string Text, bool Selected)> ToList<TEnum>(Type? resourceType = null,
        TEnum? selected = null) where TEnum : struct, Enum
    {
        var enumType = typeof(TEnum);
        ResourceManager? resource = null;

        if (resourceType != null)
        {
            resource = new ResourceManager(resourceType);
        }

        return Enum.GetValues(enumType).OfType<Enum>().Select(value =>
        (
            Convert.ToInt32(value, CultureInfo.CurrentCulture),
            resource?.GetString(value.ToString(), CultureInfo.CurrentCulture) ?? value.ToString(),
            selected != null && Convert.ToInt32(value, CultureInfo.CurrentCulture) == Convert.ToInt32(selected, CultureInfo.CurrentCulture)
        ));
    }
}