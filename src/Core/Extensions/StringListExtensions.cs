using System.Collections.Generic;
using System.Text;

namespace Oluso.Extensions;

/// <summary>
/// List of strings related extensions
/// </summary>
public static class StringListExtensions
{
    /// <summary>
    /// returns a the elements in the list a plain string separated by the specified separator
    /// </summary>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <param name="carrierReturn"></param>
    /// <returns></returns>
    public static string ToPlainString(this List<string> source, string separator = ", ", bool carrierReturn = false)
    {
        var result = new StringBuilder();
        foreach (var line in source)
        {
            var carrier = carrierReturn ? "\n" : "";
            result.Append($"{line}{carrier}{separator}");
        }
        return result.ToString().Trim();
    }
}