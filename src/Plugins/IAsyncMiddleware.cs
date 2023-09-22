namespace Oluso.Plugins;


/// <summary>
/// defines a basic async middleware that returns a value
/// </summary>
/// <typeparam name="TParameter"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public interface IAsyncMiddleware<TParameter, TReturn>: IMiddlewarePlugin
{
    /// <summary>
    /// runs the middleware and returns the execution result
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    Task<TReturn> Run(TParameter parameter, Func<TParameter, Task<TReturn>> next);
}

/// <summary>
/// defines the basic async middleware
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IAsyncMiddleware<TParameter>: IMiddlewarePlugin
{
    /// <summary>
    /// runs the middleware
    /// </summary>
    /// <param name="parameter">parameter passed to the middleware</param>
    /// <param name="next">next middleware to execute</param>
    /// <returns></returns>
    Task Run(TParameter parameter, Func<TParameter, Task> next);
}