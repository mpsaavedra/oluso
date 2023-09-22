using System.Reflection;
using Oluso.Plugins.Resolvers;

namespace Oluso.Plugins;

/// <summary>
/// base implementation of the middleware flow handler
/// </summary>
/// <typeparam name="TMiddleware"></typeparam>
public abstract class BaseMiddlewareFlow<TMiddleware>
{
    private static readonly TypeInfo _middlewareTypeInfo = typeof(TMiddleware).GetTypeInfo();
    
    /// <summary>
    /// get the middleware types
    /// </summary>
#pragma warning disable CS8618
    protected IList<Type> MiddlewareTypes { get; private set; } = new List<Type>();
#pragma warning restore CS8618

    /// <summary>
    /// get the <see cref="IMiddlewareResolver"/> used to resolve the middleware
    /// types
    /// </summary>
    protected IMiddlewareResolver MiddlewareResolver { get; private set; }

    internal BaseMiddlewareFlow(IMiddlewareResolver resolver) =>
        MiddlewareResolver = resolver ??
                             throw new ArgumentNullException(nameof(resolver),
                                 "you must specify a valid IMiddleareResolver");

    /// <summary>
    /// Adds a new middleware to the list, middlewares will be executed in the order that
    /// they are loaded
    /// </summary>
    /// <param name="middlewareType"></param>
    protected void AddMiddleware(Type middlewareType)
    {
        if (middlewareType == null) throw new ArgumentNullException(nameof(middlewareType));

        var isValid = _middlewareTypeInfo.IsAssignableFrom(middlewareType.GetTypeInfo());
        if (isValid)
            throw new ArgumentException($"The middleware type must implement {typeof(TMiddleware)}");
        
        MiddlewareTypes.Add(middlewareType);
    }
}