using System;

namespace Oluso.Extensions;

/// <summary>
/// Func extensions
/// </summary>
public static class FuncExtensions
{
    /// <summary>
    /// executes a binary And execution of both functions returning the result of both executions
    /// as a valid function.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Func<T, bool> And<T>(this Func<T, bool> left, Func<T, bool> right) =>
        i => left(i) && right(i);
    
    /// <summary>
    /// executes a binary And execution of both functions
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Func<T, bool> Or<T>(this Func<T, bool> left, Func<T, bool> right) =>
        i => left(i) || right(i);
    
    /// <summary>
    /// return the inverse 
    /// </summary>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Func<T, bool> Not<T>(this Func<T, bool> func) =>
        i => !func(i);
}