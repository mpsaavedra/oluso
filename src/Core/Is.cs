using System;

namespace Oluso;

/// <summary>
/// Helper class to execute validation of many different situations
/// </summary>
public static class Is
{
    /// <summary>
    /// throw an <see cref="ApplicationException"/> with provided message if func is true if no
    /// errors found or func is false it returns the original source value.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func">validation function</param>
    /// <param name="message">message to display</param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource Throw<TSource>(this TSource source, Func<bool> func, string message)
    {
        if(source.Errors(func))
            Insist.Throw<ApplicationException>(message);
        return source;
    }

    /// <summary>
    /// check for possible errors in the source, errors could be:<br/>
    /// <li>source to be null</li>
    /// <li>func to be null</li>
    /// <li>result of the validation function return true</li>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func">validation function to execute</param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static bool Errors<TSource>(this TSource source, Func<bool> func) =>
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        source == null || func == null || func();
}