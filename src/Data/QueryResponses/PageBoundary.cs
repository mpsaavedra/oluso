namespace Oluso.Data.QueryResponses;

/// <summary>
/// Query result page boundaries, that represents a zero index based result,
/// that holds the page data, like first item index, last item index, and
/// page index. Which is the basic information of a page in a paginated
/// result.
/// </summary>
public class PageBoundary
{
    /// <summary>
    /// returns a new <see cref="PageBoundary"/> instance
    /// </summary>
    /// <param name="firstItemIndex"></param>
    /// <param name="lastItemIndex"></param>
    public PageBoundary(int firstItemIndex, int lastItemIndex)
    {
        FirstItemIndex = firstItemIndex;
        LastItemIndex = lastItemIndex;
    }
    
    /// <summary>
    /// first item in page content Id
    /// </summary>
    public int FirstItemIndex { get; private set; }
    
    /// <summary>
    /// last item in page content Id
    /// </summary>
    public int LastItemIndex { get; private set; }
}