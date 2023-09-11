using System;

namespace Oluso.Exceptions;

/// <summary>
/// Throw an exception with message Operation had fail by default or any specified
/// </summary>
public class OperationHadFailException : Exception
{
    /// <summary>
    /// returns a new <see cref="OperationHadFailException"/> instance
    /// </summary>
    public OperationHadFailException() : base(Messages.HadFail())
    {
    }

    /// <summary>
    /// returns a new <see cref="OperationHadFailException"/> instance
    /// </summary>
    /// <param name="message"></param>
    public OperationHadFailException(string message) : base(message)
    {
        
    }
}