using System;

namespace Oluso.Enums;

/// <summary>
/// The kinds of accesses
/// </summary>
[Flags]
public enum AccessTypes : byte
{
    /// <summary>
    /// Associated with the private keyword.
    /// </summary>
    Private = 2,
		
    /// <summary>
    /// Associated with the protected keyword.
    /// </summary>
    Protected = 4,
		
    /// <summary>
    /// Associated with the internal keyword.
    /// </summary>
    Internal = 8,
		
    /// <summary>
    /// Associated with the public keyword.
    /// </summary>
    Public = 16,
    
    /// <summary>
    /// </summary>
    PrivatePublic = Private | Public
}
