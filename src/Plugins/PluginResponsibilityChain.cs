namespace Oluso.Plugins;

/// <inheritdoc cref="IPluginResponsibilityChain{TParameter,TReturn}"/>
public class PluginResponsibilityChain<TParameter, TReturn> : IPluginResponsibilityChain<TParameter, TReturn>
{
    private readonly List<object> _middlewares = new();
    private Func<TParameter, TReturn>? _finallyFunc = null;

    /// <inheritdoc cref="IPluginResponsibilityChain{TParameter,TReturn}.Chain{TPlugin}"/>
    public PluginResponsibilityChain<TParameter, TReturn> Chain<TPlugin>(TPlugin instance) where TPlugin : IPlugin
    {
        _middlewares.Add(instance);
        return this;
    }

    /// <inheritdoc cref="IPluginResponsibilityChain{TParameter,TReturn}.Execute"/>
    public TReturn Execute(TParameter parameter)
    {
        if (!_middlewares.Any())
            return default!;

        var idx = 0;
        Func<TParameter, TReturn>? func = null;
        func = (param) =>
        {
            var instance = _middlewares[idx];
            var middleware = (IMiddleware<TParameter, TReturn>)instance;

            idx++;
            if (idx == _middlewares.Count())
            {
                func = _finallyFunc ?? ((x) => default!);
            }

            return middleware.Run(param, func);
        };
        
        return func(parameter);
    }

    /// <inheritdoc cref="IPluginResponsibilityChain{TParameter,TReturn}.Finally"/>
    public PluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFunc)
    {
        _finallyFunc = finallyFunc;
        return this;
    }
}