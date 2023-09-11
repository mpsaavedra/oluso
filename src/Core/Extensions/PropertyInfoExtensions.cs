#nullable  enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oluso.Extensions;

/// <summary>
/// PropertyInfo related extensions
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// gets a list of all attributes of a given type
    /// </summary>
    /// <param name="property"></param>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TAttribute> ToAttributesOfType<TAttribute>(this PropertyInfo property) =>
        property
            .GetCustomAttributes(false)
            .Where(x => x.GetType().IsOfType(typeof(TAttribute)))
            .Cast<TAttribute>();

    /// <summary>
    /// return a given attribute of given type
    /// </summary>
    /// <param name="property"></param>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static TAttribute? ToAttributeOfType<TAttribute>(this PropertyInfo property) where TAttribute : Attribute
    {
        var attrs = property.ToAttributesOfType<TAttribute>();
        var attributes = attrs as TAttribute[] ?? attrs.ToArray();
        return attributes.Any() ? attributes.First() : null;
    }
}