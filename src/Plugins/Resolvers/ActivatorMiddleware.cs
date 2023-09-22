namespace Oluso.Plugins.Resolvers;

/// <summary>
/// a very simple middleware resolver that implements <see cref="IMiddlewareResolver"/> 
/// </summary>
public class ActivatorMiddleware : IMiddlewareResolver
{
    /// <inheritdoc cref="IMiddlewareResolver.Resolve"/>
    public object? Resolve(Type type) =>
        Activator.CreateInstance(type);
}