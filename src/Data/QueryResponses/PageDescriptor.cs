namespace Oluso.Data.QueryResponses;

/// <summary>
/// describes the query result and the result of the pagination of that result.
/// </summary>
public class PageDescriptor
{

    /// <summary>
    /// returns a new <see cref="PageDescriptor"/> instance
    /// </summary>
    /// <param name="pageSize">amount of elements per page</param>
    /// <param name="numberOfPages">total amount of pages from result</param>
    /// <param name="pageBoundaries"><see cref="PageBoundary"/> of every page</param>
    public PageDescriptor(int pageSize, int numberOfPages, PageBoundary[] pageBoundaries)
    {
        PageSize = pageSize;
        NumberOfPages = numberOfPages;
        PageBoundaries = pageBoundaries;
    }
    
    /// <summary>
    /// amount of elements per page
    /// </summary>
    public int PageSize { get; private set; }
    
    /// <summary>
    /// number of pages generated from query result
    /// </summary>
    public int NumberOfPages { get; private set; }
    
    /// <summary>
    /// gets array of <see cref="PageBoundary"/> with the boundaries of avery page extracted
    /// from the query result
    /// </summary>
    public PageBoundary[] PageBoundaries { get; private set; }
}