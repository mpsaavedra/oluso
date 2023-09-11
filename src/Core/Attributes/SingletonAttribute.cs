using System;

namespace Oluso.Attributes;

/// <summary>
/// The marked class should be registered as singleton in the DI container
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute : Attribute
{
    /// <summary>
    /// <see cref="SingletonAttribute"/>
    /// </summary>
    public SingletonAttribute()
    {
    }
}