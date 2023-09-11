using System;
using System.Linq;
using System.Reflection;

namespace Oluso.Extensions;

/// <summary>
/// MethodInfo extensions
/// </summary>
public static class MethodInfoExtensions
{
    /// <summary>
    /// returns the First attribute of the provided type, if none found it returns null
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <typeparam name="TAttr"></typeparam>
    /// <returns></returns>
    public static TAttr GetMethodAttributeOf<TAttr>(this MethodInfo methodInfo)
        where TAttr : Attribute
    {
        return (methodInfo.GetCustomAttributes<TAttr>().Any() ? methodInfo.GetCustomAttributes<TAttr>().First() : null)!;
    }
}