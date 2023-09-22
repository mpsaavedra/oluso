namespace Oluso.Plugins.Resolvers;

/// <summary>
/// used to create instances of middlewares, you can implement this
/// interface for your preferred dependency injection container
/// </summary>
public interface IMiddlewareResolver
{
    /// <summary>
    /// creates an instance of the given type, if could not create an instance it returns null
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    object? Resolve(Type type);
}