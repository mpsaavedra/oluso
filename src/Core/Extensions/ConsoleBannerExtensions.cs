using System;
using Oluso.Helpers;

namespace Oluso.Extensions;

/// <summary>
/// ConsoleBanner related extensions
/// </summary>
public static class ConsoleBannerExtensions
{
    /// <summary>
    /// write text as banner in the console
    /// </summary>
    /// <param name="text"></param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    public static void ToBanner(this string text, ConsoleColor? foregroundColor = null, 
        ConsoleColor? backgroundColor = null) =>
        ConsoleBanner.Write(text, foregroundColor, backgroundColor);
}