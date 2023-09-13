using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Oluso.Data.Operations;
using Oluso.Data.Specifications;

namespace Oluso.Data.Repositories;

/// <summary>
/// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}"/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUserKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TContext"></typeparam>
public class QueryRepository<TKey, TUserKey, TEntity, TContext> :
    IQueryRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// returns a new <see cref="QueryRepository{TKey,TUserKey,TEntity,TContext}"/> instance
    /// </summary>
    /// <param name="context"></param>
    public QueryRepository(TContext context)
    {
        Insist.MustBe.NotNull(context);
        Context = context;
        Query = Context.Set<TEntity>();
    }
    
    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Query"/>
    /// </summary>
    public IQueryable<TEntity> Query { get; }

    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Context"/>
    /// </summary>
    public TContext Context { get; }

    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.GetEntity{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DbSet<T> GetEntity<T>() where T : class => Context.Set<T>();

    /// <summary>
    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},System.Threading.CancellationToken)"/> 
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await Task.Run(() => Query.AsEnumerable().Where(predicate.Compile()).AsQueryable(), cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(Oluso.Data.Specifications.Specification{TEntity},System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IQueryable<TEntity>> FindAsync(Specification<TEntity> specification,
        CancellationToken cancellationToken = default) =>
        await Task.Run(() => Query.AsEnumerable().Where(specification).AsQueryable(), cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Count"/>
    /// </summary>
    /// <returns></returns>
    public int Count(bool softDeleted = false) => 
        softDeleted 
            ? Query.Count() 
            : Query.Count(x => x.Status == true);

    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.CountAsync"/>
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        predicate == null
            ? await Query.CountAsync(cancellationToken: cancellationToken)
            : await Query.CountAsync(predicate, cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Any"/>
    /// </summary>
    /// <returns></returns>
    public bool Any(bool softDeleted = false) => softDeleted ? Query.Any() : Query.Any(x => x.Status == true);

    /// <summary>
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.AnyAsync"/>
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        predicate == null
            ? await Query.AnyAsync(cancellationToken: cancellationToken)
            : await Query.AnyAsync(predicate, cancellationToken);
}