using System.Collections;
using Oluso.Data.QueryResponses;

namespace Oluso.Data.Extensions;

/// <summary>
/// IList related extensions
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// returns a <see cref="PageDescriptor"/> from provided list data.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static PageDescriptor ToPageDescriptor(this IList list, int pageSize = 50)
    {
        var actualPageSize = pageSize;

        if (actualPageSize <= 0)
        {
            actualPageSize = list.Count;
        }

        var pagesCount = (int)Math.Round(Math.Max(1, Math.Ceiling((float)list.Count / actualPageSize)));

        return new PageDescriptor(
            actualPageSize, pagesCount,
            Enumerable.Range(0, pagesCount)
                .Select( x => new PageBoundary(
                    x * actualPageSize,
                    Math.Min((x * actualPageSize) + (actualPageSize -1), list.Count - 1)
                    )).ToArray());
    }
}