namespace Oluso.Plugins;

/// <summary>
/// defines the chain of responsibility 
/// </summary>
/// <typeparam name="TParameter"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public interface IPluginResponsibilityChain<TParameter, TReturn>
{
    /// <summary>
    /// register a new plugin into the change
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    PluginResponsibilityChain<TParameter, TReturn> Chain<TPlugin>(TPlugin instance) where TPlugin : IPlugin;

    /// <summary>
    /// executes the responsibility chain
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    TReturn Execute(TParameter parameter);

    /// <summary>
    /// this function is called after the chain has finished, even if there was an error
    /// </summary>
    /// <param name="finallyFunc"></param>
    /// <returns></returns>
    PluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFunc);
}