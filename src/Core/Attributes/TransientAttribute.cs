using System;

namespace Oluso.Attributes;

/// <summary>
/// The marked class should be registered as scoped in the DI container
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute : Attribute
{
    /// <summary>
    /// returns a new <see cref="TransientAttribute"/> instance
    /// </summary>
    public TransientAttribute()
    {
    }
}