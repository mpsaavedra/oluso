#nullable enable
using Oluso.Exceptions;

namespace Oluso.Extensions;

/// <summary>
/// Boolean related extensions
/// </summary>
public static class BooleanExtensions
{
    /// <summary>
    /// check if source is true if not throws and exception 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool ToTrue(this bool source, string? message = null)
    {
        if(!source)
        {
            message ??= Messages.ComparisonCouldNotBeFalse();
            Insist.Throw<BooleanComparisonException>(message);
        }
        return source;
    }

    /// <summary>
    /// check if source is false if not throws and exception
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool ToFalse(this bool source, string? message = null) =>
        !source.ToTrue(Messages.ComparisonCouldNotBeTrue());

    /// <summary>
    /// checks if some value is false. This method is some helper to be used in operations that require validations
    /// and it works as a circuit breaker.
    /// <code>
    ///  // validate some result
    ///  var result = MyFunction();
    ///  if(!result.HadFail("Sorry this function fail"))
    ///  {
    ///     // do something if operation had not fail
    ///  }
    ///
    ///  // or as a validation result notice the negation of the validation
    ///  if((!string.IsNullOrWhiteSpace(username))).HadFail("Sorry username could not be null or whitespace"))
    ///  {
    ///  
    ///  }   
    /// </code>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool HadFail(this bool source, string? message = null)
    {
        if (!source)
        {
            message ??= Messages.HadFail();
            Insist.Throw<OperationHadFailException>(message);
        }
        return source;
    }
}