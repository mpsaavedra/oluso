using System;

namespace Oluso.Attributes;

/// <summary>
/// The marked class should be registered as scoped in the DI container
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute : Attribute
{
    /// <summary>
    /// returns a new <see cref="ScopedAttribute"/> instance
    /// </summary>
    public ScopedAttribute()
    {
    }
}