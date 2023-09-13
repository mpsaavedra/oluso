using Microsoft.EntityFrameworkCore;
using Oluso.Data.Operations;

namespace Oluso.Data.Repositories;

/// <summary>
/// A command repository, this repository is meant to be used for operations that modify data
/// in the database
/// </summary>
public interface ICommandRepository<TKey, TUserKey, TEntity, TContext> : 
    ICreateOperation<TKey, TUserKey, TEntity>,
    IUpdateOperation<TKey, TUserKey, TEntity>,
    IDeleteOperation<TKey, TUserKey, TEntity> 
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    
    /// <summary>
    /// Unit of work used to operate this Repository
    /// </summary>
    IUnitOfWork<TContext>? UnitOfWork { get; }
    
    /// <summary>
    /// returns the DbContext
    /// </summary>
    TContext Context { get; }

    /// <summary>
    /// change the entity status to provided value
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool>? ChangeStatusAsync(TKey id, bool state, CancellationToken cancellationToken = default);
}