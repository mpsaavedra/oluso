namespace Oluso.Plugins.Services;

/// <see cref="IChainOfResponsibilityService"/>
public class ChainOfResponsibilityService : IChainOfResponsibilityService
{
    private readonly IPluginService _pluginService;

    /// <summary>
    /// returns a new instance of <see cref="ChainOfResponsibilityService"/>
    /// </summary>
    public ChainOfResponsibilityService(IPluginService pluginService)
    {
        _pluginService = pluginService;
    }

    /// <inheritdoc cref="IChainOfResponsibilityService.ExecuteChain{TParameter, TReturn}"/>
    public TReturn ExecuteChain<TParameter, TReturn>(string eventCode, TParameter parameter)
    {
        var middleware = Task.Run(() => _pluginService.MiddlewareChain(eventCode)).Result;
        var chain = new PluginResponsibilityChain<TParameter, TReturn>();
    
        foreach (var mPlugin in middleware)
        {
            chain.Chain(mPlugin);
        }
    
        return chain.Execute(parameter);
    }

    /// <inheritdoc cref="IChainOfResponsibilityService.ExecuteAsyncChain{TParameter, TReturn}"/>
    public async Task<TReturn> ExecuteAsyncChain<TParameter, TReturn>(string eventCode, TParameter parameter)
    {
        var middleware = await _pluginService.MiddlewareChain(eventCode);
        var chain = new AsyncPluginResponsibilityChain<TParameter, TReturn>();

        foreach (var mPlugin in middleware)
        {
            chain.Chain(mPlugin);
        }

        return await chain.Execute(parameter);
    }
}
