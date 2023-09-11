using System;

namespace Oluso.Exceptions;

/// <summary>
/// throw exception with a message that comparison must be true
/// </summary>
public class BooleanComparisonException : Exception
{
    /// <summary>
    /// returns a new <see cref="BooleanComparisonException"/> instance
    /// </summary>
    public BooleanComparisonException() : base(Messages.ComparisonCouldNotBeFalse())
    {
    }

    /// <summary>
    /// returns a new <see cref="BooleanComparisonException"/> instance
    /// </summary>
    /// <param name="message"></param>
    public BooleanComparisonException(string message) : base(message)
    {
        
    }
}