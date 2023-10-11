using System;

namespace Oluso.Enums;

/// <summary>
/// The kind of member
/// </summary>
[Flags]
public enum MemberType : byte
{
    /// <summary>
    /// Both by field and property
    /// </summary>
    Both = Field | Property,

    /// <summary>
    /// A C# field.
    /// </summary>
    Field = 4,
		
    /// <summary>
    /// A C# property.
    /// </summary>
    Property = 8
}