using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Oluso.Data.Extensions;
using Oluso.Data.Operations;
using Oluso.Data.QueryResponses;
using Oluso.Data.Specifications;

namespace Oluso.Data.Repositories;

/// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}"/>
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
    
    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Query"/>
    public IQueryable<TEntity> Query { get; }

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Context"/>
    public TContext Context { get; }

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.GetEntity{T}"/>
    public DbSet<T> GetEntity<T>() where T : class => Context.Set<T>();

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},System.Threading.CancellationToken)"/> 
    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default) =>
        predicate != null
        ? await Task.Run(() => Query.AsEnumerable().Where(predicate.Compile()).AsQueryable(), cancellationToken)
        : await Task.Run(() => Query.AsEnumerable().AsQueryable(), cancellationToken) ;

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(Oluso.Data.Specifications.Specification{TEntity},System.Threading.CancellationToken)"/>
    public async Task<IQueryable<TEntity>> FindAsync(Specification<TEntity>? specification,
        CancellationToken cancellationToken = default) =>
        specification != null
        ? await Task.Run(() => Query.AsEnumerable().Where(specification).AsQueryable(), cancellationToken)
        : await Task.Run(() => Query.AsEnumerable().AsQueryable(), cancellationToken);

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindPaginatedAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},int,int,System.Threading.CancellationToken)"/>
    public async Task<QueryResult<TEntity>> FindPaginatedAsync(Expression<Func<TEntity, bool>>? predicate,
        int pageIndex = 0, int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        Insist.MustBe.True<ApplicationException>(pageIndex > -1, () => Messages.MsgGreaterThan("Page index", 0));
        Insist.MustBe.True<ApplicationException>(pageSize > 0, () => Messages.MsgGreaterThan("Page size", 0));

        var filteredItems = 
            predicate != null 
            ? Context.Set<TEntity>().Where(predicate).ToList()
            : Context.Set<TEntity>().ToList();
        var pageDescriptor = filteredItems.ToPageDescriptor(pageSize);
        var pageBoundaries = pageDescriptor.PageBoundaries[pageIndex];
        var from = pageBoundaries.FirstItemIndex;
        var to = pageBoundaries.LastItemIndex;
        var take = to - from + 1;
        return await Task.FromResult(new QueryResult<TEntity>(pageDescriptor, pageIndex,
            filteredItems.Skip(from).Take(take)));
    }

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindPaginatedAsync(Oluso.Data.Specifications.Specification{TEntity},int,int,System.Threading.CancellationToken)"/>
    public async Task<QueryResult<TEntity>> FindPaginatedAsync(Specification<TEntity>? specification, int pageIndex = 0,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        Insist.MustBe.True<ApplicationException>(pageIndex > -1, () => Messages.MsgGreaterThan("Page index", 0));
        Insist.MustBe.True<ApplicationException>(pageSize > 0, () => Messages.MsgGreaterThan("Page size", 0));

        var filteredItems = specification != null 
            ? Context.Set<TEntity>().Where(specification).ToList()
            : Context.Set<TEntity>().ToList();
        var pageDescriptor = filteredItems.ToPageDescriptor(pageSize);
        var pageBoundaries = pageDescriptor.PageBoundaries[pageIndex];
        var from = pageBoundaries.FirstItemIndex;
        var to = pageBoundaries.LastItemIndex;
        var take = to - from + 1;
        return await Task.FromResult(new QueryResult<TEntity>(pageDescriptor, pageIndex,
            filteredItems.Skip(from).Take(take)));
    }

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Count"/>
    public int Count(bool softDeleted = false) => 
        softDeleted 
            ? Query.Count() 
            : Query.Count(x => x.Status == true);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.CountAsync"/>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        predicate == null
            ? await Query.CountAsync(cancellationToken: cancellationToken)
            : await Query.CountAsync(predicate, cancellationToken);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Any"/>
    public bool Any(bool softDeleted = false) => softDeleted ? Query.Any() : Query.Any(x => x.Status == true);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.AnyAsync"/>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        predicate == null
            ? await Query.AnyAsync(cancellationToken: cancellationToken)
            : await Query.AnyAsync(predicate, cancellationToken);
}