namespace Oluso.Data.QueryResponses;

/// <summary>
/// result of a given query, that include some effective information about the pagination
/// of the result.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class QueryResult<TEntity>
{

    /// <summary>
    /// returns a new <see cref="QueryResult{TEntity}"/> instance
    /// </summary>
    /// <param name="descriptor"></param>
    /// <param name="currentPage"></param>
    /// <param name="results"></param>
    public QueryResult(PageDescriptor descriptor, int currentPage, IEnumerable<TEntity> results)
    {
        Descriptor = descriptor;
        CurrentPage = currentPage;
        Results = results;
    }
    
    /// <summary>
    /// <see cref="PageDescriptor"/> with overall information about the query result pagination
    /// </summary>
    public PageDescriptor Descriptor { get; private set; }
    
    /// <summary>
    /// current page
    /// </summary>
    public int CurrentPage { get; private set; }
    
    /// <summary>
    /// results of the query contained in this page
    /// </summary>
    public IEnumerable<TEntity> Results { get; private set; }
}