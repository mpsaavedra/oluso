namespace Oluso.Data.Operations;

/// <summary>
/// Update operations
/// </summary>
public interface IUpdateOperation<TKey, TUserKey, TEntity> 
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
{
    /// <summary>
    /// update and entity data int he database. This operation will save changes to the database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}