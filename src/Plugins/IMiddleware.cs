namespace Oluso.Plugins;

/// <summary>
/// defines a middleware that returns result of execution
/// </summary>
/// <typeparam name="TParameter"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public interface IMiddleware<TParameter, TReturn>
{
    /// <summary>
    /// execute the middleware and return the result of the execution
    /// </summary>
    /// <param name="parameter">passed parameters</param>
    /// <param name="next">next middleware to execute</param>
    /// <returns></returns>
    TReturn Run(TParameter parameter, Func<TParameter, TReturn> next);
}

/// <summary>
/// defines a middleware that does not return any value
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IMiddleware<TParameter>
{
    /// <summary>
    /// execute the middleware but it does not return any value
    /// </summary>
    /// <param name="parameter">passed parameters</param>
    /// <param name="next">next middleware to execute</param>
    void Run(TParameter parameter, Action<TParameter> next);
}