using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.Repositories;
using Oluso.Extensions;

namespace Oluso.Data;

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
/// public IAppUnitOrWork : IUnitOfWork{ApplicationDbContext}
/// {
/// }
/// public class AppUnitOfWork : UnitOfWork{ApplicationDbContext}, IAppUnitOfWork
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
public interface IUnitOfWork<out TContext>
    where TContext : DbContext
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
public class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private readonly IServiceProvider _provider;

    /// <inheritdoc cref="IUnitOfWork{TContext}.Context"/>
    public TContext Context { get; }

    /// <summary>
    /// returns a new instance of <see cref="UnitOfWork{TContext}"/>
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
        var executionStrategy = Context.Database.CreateExecutionStrategy();
        if (Context.Database.ProviderName == null || Context.Database.ProviderName.Contains("InMemory"))
        {
            // InMemory does no support transactions
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var result = operation.Invoke();
                if (Context is BaseDbContext context)
                    await context.SaveEntitiesChangesAsync(cancellationToken);
                else
                    await Context.SaveChangesAsync(cancellationToken);
                return await result;
            });
        }
        return await executionStrategy.ExecuteInTransactionAsync<TResult>(async ct =>
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation.Invoke();
                if (Context is BaseDbContext context)
                    await context.SaveEntitiesChangesAsync(cancellationToken);
                else
                    await Context.SaveChangesAsync(cancellationToken);
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
                    if (Context is BaseDbContext context)
                        await context.SaveEntitiesChangesAsync(cancellationToken);
                    else
                        await Context.SaveChangesAsync(cancellationToken);
                });
            }
            else
            {
                executionStrategy.ExecuteInTransaction(async () =>
                {
                    operation.Invoke();
                    if (Context is BaseDbContext context)
                        await context.SaveEntitiesChangesAsync(cancellationToken);
                    else 
                        await Context.SaveChangesAsync(cancellationToken);
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
}