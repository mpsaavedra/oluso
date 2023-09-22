namespace Oluso.Plugins;

/// <summary>
/// define the async responsibility chain plugin needed methods
/// </summary>
public interface IAsyncPluginResponsibilityChain<TParameter, TReturn>
{
    /// <summary>
    /// register a new plugin instance into the chain
    /// </summary>
    AsyncPluginResponsibilityChain<TParameter, TReturn> Chain(object instance);

    /// <summary>
    /// start the chain execution
    /// </summary>
    Task<TReturn> Execute(TParameter parameter);

    /// <summary>
    /// method to execute often the chain is executed, it could be used to do some dispose or
    /// clean up process
    /// </summary>
    AsyncPluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, Task<TReturn>> finallyFunc);

}