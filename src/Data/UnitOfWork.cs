using System.Text;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Authentication;
using Oluso.Data.Repositories;
using Oluso.Extensions;

namespace Oluso.Data;

/// <inheritdoc cref="Oluso.Data.IUnitOfWork{TContext,TUserKey}"/>
public interface IUnitOfWork<out TContext>: IUnitOfWork<TContext, string>
    where TContext : DbContext
{
}

/// <summary>
/// <p>
/// Define the Unit of Work that handle transaction over database operations. This pattern is focused on changes in
/// objects. As Martin Fowler said, this unit keeps a list of changed objects in a transaction context. Also manages
/// the write operation and deal with the concurrency problems. We can look at this as a context, session, or object
/// which follows all the changes on data models during a transaction.
/// <br/>
/// In this particular implementation it requires to define the UnitOfWork implementation because the
/// <typeparamref name="TContext"/> types is mandatory., by defining a dummy declaration makes  invocation using the
/// dependency resolver easier.
/// <br/>
/// If you use the Repository pattern implemented is possible for you to obtain repositories from this unit of work
/// <br/>
/// <b>Note:</b> You must define only one by application, defining two it will make no sense.
/// </p>
/// <code>
///  // implemented as dummy Unit of Work to make easier the dependency injection, if you use the
///  // AddRepositories extension registration will be automatic.
/// public IAppUnitOrWork : IUnitOfWork{ApplicationDbContext, string}
/// {
/// }
/// public class AppUnitOfWork : UnitOfWork{ApplicationDbContext, string}, IAppUnitOfWork
/// {
/// }
/// pubic class UoWTest
/// {
///     private IAppUnitOfWork _unitOfWork;
///     // simple usage
///     public async Task{int} CreateUser(User user)
///     {
///         return _unitOfWork.ExecuteAsync(async ()=>
///         {
///             var userEntry = await _context.Users.AddAsync(user);
///             _unitOfWork.SaveEntitiesChangesAsync(); // this will update the audit information
///             return userEntry.Entity.Id;
///         });
///     }
/// }
/// </code>
/// </summary>
/// <typeparam name="TContext">Application DbContext</typeparam>
/// <typeparam name="TUserKey"></typeparam>
public interface IUnitOfWork<out TContext, TUserKey>
    where TContext : DbContext
    where TUserKey: class
{
    /// <summary>
    /// Database context
    /// </summary>
    TContext Context { get; }
    
    /// <summary>
    /// Execute an operation in resilient environment with a return value
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="verifySucceeded"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation,
        Task<bool>? verifySucceeded = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// execute an operation in a resilient environment with no return value
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="verifySucceeded"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ExecuteAsync(Action operation, Func<bool>? verifySucceeded = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Save tracked changes of entities. If the entities inherit from <see cref="IBusinessEntity"/>
    /// it will update all audit information
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveEntitiesChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// returns a repository from the DI container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? Repository<T>() where T : class;
    
    /// <summary>
    /// returns a given service from de DI container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? Service<T>() where T: class;
}

/// <inheritdoc cref="IUnitOfWork{TContext}"/>
public class UnitOfWork<TContext> : UnitOfWork<TContext, string>, IUnitOfWork<TContext>
    where TContext : DbContext
{
    /// <summary>
    /// returns a new <see cref="UnitOfWork{TContext}"/> instance
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="context"></param>
    public UnitOfWork(IServiceProvider provider, TContext context) : base(provider, context)
    {
    }
}


/// <inheritdoc cref="IUnitOfWork{TContext}"/>
public class UnitOfWork<TContext, TUserKey> : IUnitOfWork<TContext, TUserKey>
    where TContext : DbContext
    where TUserKey : class
{
    private IExecutionStrategy? _executionStrategy = null;
    private readonly IServiceProvider _provider;

    /// <summary>
    /// User Id user to seed data
    /// </summary>
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Guid UserSeedId = Guid.Parse("2E9D5237-4453-43C6-B6E3-F31207E25241");

    /// <summary>
    /// event to invoke when entities where added
    /// </summary>
    public event BaseDbContext.DatabaseOperationEventHandler? OnEntitiesAdded;

    /// <summary>
    /// event to invoke when entities where modified
    /// </summary>
    public event BaseDbContext.DatabaseOperationEventHandler? OnEntitiesUpdated;

    /// <summary>
    /// event to invoke when entities where deleted
    /// </summary>
    public event BaseDbContext.DatabaseOperationEventHandler? OnEntitiesDeleted;

    /// <inheritdoc cref="IUnitOfWork{TContext, TUserKey}.Context"/>
    public TContext Context { get; }

    /// <summary>
    /// returns a new instance of <see cref="UnitOfWork{TContext, TUserKey}"/>
    /// </summary>
    public UnitOfWork(IServiceProvider provider, TContext context)
    {
        Context = context.IsNullOrEmptyThrow($"{nameof(context)} could not be null or empty");
        _provider = provider;
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.ExecuteAsync{TResult}"/>
    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation,
        Task<bool>? verifySucceeded = null,
        CancellationToken cancellationToken = default)
    {
        verifySucceeded ??= new Task<bool>(() => true);

        if (_executionStrategy is not null)
        {
            var result = await operation.Invoke();
            await SaveEntitiesChangesAsync(cancellationToken);
            return result;
        }

        _executionStrategy ??= Context.Database.CreateExecutionStrategy();
        if (Context.Database.ProviderName == null || Context.Database.ProviderName.Contains("InMemory"))
        {

            return await _executionStrategy.ExecuteAsync(async () =>
            {
                // InMemory does no support transactions
                var result = await operation.Invoke();
                await SaveEntitiesChangesAsync(cancellationToken);
                return result;
            });
        }

        if (Context.Database.CurrentTransaction is not null)
        {
            var result = await operation.Invoke();
            await SaveEntitiesChangesAsync(cancellationToken);
            return result;
        }

        return await _executionStrategy.ExecuteInTransactionAsync<TResult>(async ct =>
        {
            if (Context.Database.CurrentTransaction is not null)
            {
                var result = await operation.Invoke();
                await SaveEntitiesChangesAsync(cancellationToken);
                return result;
            }
            await using var transaction = await Context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation.Invoke();
                await SaveEntitiesChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                Insist.RegisterException(e, e.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }, async ct => await Task.FromResult(true), cancellationToken);
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.ExecuteAsync(Action, Func{bool}?, CancellationToken)"/>
    public async Task ExecuteAsync(Action operation, Func<bool>? verifySucceeded = null,
        CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            verifySucceeded ??= (() => false);
            var executionStrategy = Context.Database.CreateExecutionStrategy();
            if (Context.Database.ProviderName == null || Context.Database.ProviderName.Contains("InMemory"))
            {
                // InMemory does no support transactions
                executionStrategy.Execute(async () =>
                {
                    operation.Invoke();
                    await SaveEntitiesChangesAsync(cancellationToken);
                });
            }
            else
            {

                Task.Run(async () =>
                {
                    if (Context.Database.CurrentTransaction is not null)
                    {
                        operation.Invoke();
                        await SaveEntitiesChangesAsync(cancellationToken);
                    }
                }, cancellationToken);

                executionStrategy.ExecuteInTransaction(async () =>
                {

                    await Task.Run(async () =>
                    {
                        if (Context.Database.CurrentTransaction is not null)
                        {
                            operation.Invoke();
                            await SaveEntitiesChangesAsync(cancellationToken);
                        }
                    }, cancellationToken);
                    
                    await using var transaction = await Context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        operation.Invoke();
                        await SaveEntitiesChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                    }
                    catch (Exception e)
                    {
                        Insist.RegisterException(e, e.Message);
                        await transaction.RollbackAsync(cancellationToken);
                        throw;
                    }
                }, verifySucceeded);
            }
        }, cancellationToken);
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.Repository{T}"/>
    public T? Repository<T>() where T : class =>
        _provider.GetRequiredService(typeof(T)) as T;

    /// <inheritdoc cref="IUnitOfWork{TContext}.Service{T}"/>
    public T? Service<T>() where T : class =>
        _provider.GetRequiredService(typeof(T)) as T;

    /// <summary>
    /// Throw an exception with a more detailed information about the error
    /// </summary>
    /// <param name="exception"></param>
    private void ThrowSave(Exception exception)
    {
        Task.Run(async () => await ThrowSaveAsync(exception)).ConfigureAwait(false);
    }

    /// <summary>
    /// Throw an exception with a more detailed information about the error
    /// </summary>
    /// <param name="exception"></param>
    private async Task ThrowSaveAsync(Exception exception)
    {
        var sb = new StringBuilder();

        switch (exception)
        {
            case DbUpdateConcurrencyException dbUpdateConcurrencyException
                when exception is DbUpdateConcurrencyException:

                sb.AppendLine(dbUpdateConcurrencyException.ToFullMessage());

                if (!Is.NullOrEmpty(dbUpdateConcurrencyException) &&
                    !Is.NullOrEmpty(dbUpdateConcurrencyException.Entries))
                {
                    foreach (var eve in dbUpdateConcurrencyException.Entries)
                    {
                        sb.Append("Entity of type ")
                            .Append(eve.Entity.GetType().Name)
                            .Append(" in state ")
                            .Append(eve.State)
                            .AppendLine(" could not be updated");
                    }
                }

                Insist.Throw<DbUpdateConcurrencyException>(sb.ToString());
                break;

            case DbUpdateException dbUpdateException when exception is DbUpdateException:

                sb.AppendLine(dbUpdateException.ToFullMessage());

                if (!Is.NullOrEmpty(dbUpdateException) && !Is.NullOrEmpty(dbUpdateException.Entries))
                {
                    foreach (var eve in dbUpdateException.Entries)
                    {
                        sb.Append("Entity of type ")
                            .Append(eve.Entity.GetType().Name)
                            .Append(" in state ")
                            .Append(eve.State)
                            .AppendLine(" could not be updated");
                    }

                }

                Insist.Throw<DbUpdateException>(sb.ToString());
                break;

            default:

                sb.AppendLine(exception.ToFullMessage());

                if (!Is.NullOrEmpty(exception) && !Is.NullOrEmpty(exception.Data))
                {
                    foreach (var eve in exception.Data)
                    {
                        sb.Append("Entity of type ")
                            .AppendLine(eve?.GetType().Name);
                    }
                }

                Insist.Throw<Exception>(sb.ToString());
                break;
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Saves data into the database and updates the auditable information of affected entities.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public virtual async Task<int> SaveEntitiesChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = Context.ChangeTracker.Entries().Where(x => x.Entity is IBusinessEntity);
        {
            var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
            var added = entityEntries.Where(e => e.State == EntityState.Added);
            var modified = entityEntries.Where(e => e.State == EntityState.Modified);
            var deleted = entityEntries.Where(e => e.State == EntityState.Deleted);

            if (added.Any()) ProcessAddedEntities(added);
            if (modified.Any()) ProcessModifiedEntities(modified);
            if (deleted.Any()) ProcessDeletedEntities(deleted);
        }


        var result = await Context.SaveChangesAsync(cancellationToken);
        return result;
    }

    private void ProcessAddedEntities(IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            var userId = GetUserId(entry);
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.GetType().GetProperty("CreatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                    entry.Entity.GetType().GetProperty("CreatedBy")?.SetValue(entry.Entity, userId);
                    entry.Entity.GetType().GetProperty("RowVersion")?.SetValue(entry.Entity, Guid.NewGuid().ToString());
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Modified:
                default:
                    throw new ApplicationException($"Entry {entry} is not Added state");
            }
        }

        OnEntitiesAdded?.Invoke(this, DatabaseOperationEventArgs.New(entityEntries));
    }

    private void ProcessModifiedEntities(IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            var userId = GetUserId(entry);
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.GetType().GetProperty("UpdatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                    entry.Entity.GetType().GetProperty("UpdatedBy")?.SetValue(entry.Entity, userId);
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Modified state");
            }
        }

        OnEntitiesUpdated?.Invoke(this, DatabaseOperationEventArgs.New(entityEntries));
    }

    private void ProcessDeletedEntities(IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            var userId = GetUserId(entry);
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.Entity.GetType().GetProperty("DeletedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                    entry.Entity.GetType().GetProperty("DeletedBy")?.SetValue(entry.Entity, userId);
                    entry.Entity.GetType().GetProperty("State")?.SetValue(entry.Entity, false);
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Modified:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Deleted state");
            }
        }

        OnEntitiesDeleted?.Invoke(this, DatabaseOperationEventArgs.New(entityEntries));
    }

    private TUserKey GetUserId(EntityEntry entry, IAuthenticatedUser<TUserKey>? authenticatedUser = null)
    {
        var userId = authenticatedUser?.UserId;

        if (userId == null || string.IsNullOrWhiteSpace(userId.ToString()))
        {
            userId = UserSeedId.ToString() as TUserKey;
        }

        return userId!;
    }
}