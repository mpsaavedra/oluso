#nullable enable
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oluso.Extensions;

/// <summary>
/// Enum related extensions
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// returns the enumeration value
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int ToInt(this Enum name) =>
        (int)(IConvertible)name;

    /// <summary>
    /// returns the enumeration value
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static byte ToByte(this Enum name) =>
        (byte)(IConvertible)name;

    /// <summary>
    /// returns the enumeration value
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToValue<T>(this Enum name) =>
        (T)(IConvertible)name;
    
    /// <summary>
    /// return the value of <see cref="DescriptionAttribute"/> of an enum
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string? GetDescription(this Enum val) =>
        val.GetAttributeOfType<DescriptionAttribute>()?.Description ?? val?.ToString();

    /// <summary>
    /// returns values of the <see cref="DisplayAttribute"/> of an enumeration
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static (string? name, string? description) ToDisplay(this Enum name)
    {
        var attribute = name.GetAttributeOfType<DisplayAttribute>();
        return attribute == null ? (name?.ToString(), name?.ToString()) : (attribute.Name, attribute.Description);
    }

    /// <summary>
    /// returns an attribute of a given enum or the default value.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static TAttribute? GetAttributeOfType<TAttribute>(this Enum value)
    {
        var val = value.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(value)));
        var type = val.GetType();
        var memberInfo = type.GetMember(val.ToString()).FirstOrDefault(x => x.DeclaringType == type);
        if (memberInfo != null)
        {
            var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(TAttribute), false);
            return attribute is TAttribute attribute1 ? attribute1 : default;
        }

        return default;
    }

    private static TAttr[]? GetAttributesOfType<TAttr>(this Enum val) where TAttr : Attribute
    {
        var field = val.GetType().GetField(val.ToString());
        if (field == null)
        {
            return null;
        }

        return (TAttr[])field.GetCustomAttributes(typeof(TAttr), false);
    }
}