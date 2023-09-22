namespace Oluso.Plugins;

/// <summary>
/// implements the <see cref="IAsyncPluginResponsibilityChain{TParameter,TReturn}"/> interface
/// with the chain processes and all that
/// </summary>
/// <typeparam name="TParameter"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public class AsyncPluginResponsibilityChain<TParameter, TReturn> : IAsyncPluginResponsibilityChain<TParameter, TReturn>
{
    private List<object> _middlewares ;
    private Func<TParameter, Task<TReturn>>? _finallyFunc = null;

    /// <summary>
    /// returns a new instance of <see cref="AsyncResponsibilityChain{TParameter,TReturn}"/>
    /// </summary>
    public AsyncPluginResponsibilityChain()
    {
        _middlewares = new List<object>();
    }

    /// <summary>
    /// Add a new middleware to the middleware chain
    /// </summary>
    public AsyncPluginResponsibilityChain<TParameter, TReturn> Chain(object instance)
    {
        _middlewares.Add(instance);
        return this;
    }

    /// <summary>
    /// execute the async middleware chain
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public async Task<TReturn> Execute(TParameter parameter)
    {
        if (_middlewares.Count == 0)
        {
            return default(TReturn)!;
        }

        // order using the Level property
        var ordered = _middlewares.OrderBy(x => (x as IPlugin).Level).ToList();
        int index = 0;
        Func<TParameter, Task<TReturn>> func = null!;
        func = (param) =>
        {
            // var instance = _middlewares[index];
            var instance = ordered[index];
            var middleware = (IAsyncMiddleware<TParameter, TReturn>)instance;

            index++;
            // if (index == _middlewares.Count)
            if (index == ordered.Count())
            {
                func = _finallyFunc ?? ((p) =>
                    Task.FromResult(default(TReturn))!);
            }

            return middleware.Run(param, func);
        };

        return await func(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes some function with the chain end it's execution
    /// </summary>
    public AsyncPluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, Task<TReturn>> finallyFunc)
    {
        _finallyFunc = finallyFunc;
        return this;
    }
}
