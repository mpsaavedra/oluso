namespace Oluso.Plugins.Services;

/// <summary>
/// define the Chain of responsibility service methods
/// </summary>
public interface IChainOfResponsibilityService
{
    // /// <summary>
    // /// execute a given chain of responsibility loading middleware plugins from the
    // /// <see cref="IPluginService"/>
    // /// </summary>
    // /// <param name="eventCode"></param>
    // /// <param name="parameter"></param>
    // /// <typeparam name="TParameter"></typeparam>
    // /// <typeparam name="TReturn"></typeparam>
    // /// <returns></returns>
    // TReturn ExecuteChain<TParameter, TReturn>(string eventCode, TParameter parameter);

    /// <summary>
    /// Executes a given chain of responsibility loading middleware plugins from the
    /// <see cref="IPluginService"/>. It will only use those plugins that the
    /// <see cref="IMiddlewarePlugin.EventCode"/> match and are <see cref="IPlugin.Enable"/>,
    /// ordered by the dependencies
    /// </summary>
    /// <param name="eventCode">code of event that calls chain</param>
    /// <param name="parameter">parameter type sent to chain</param>
    /// <typeparam name="TParameter">typeof parameter sent to chain</typeparam>
    /// <typeparam name="TReturn">return type of the chain</typeparam>
    /// <returns></returns>
    Task<TReturn> ExecuteAsyncChain<TParameter, TReturn>(string eventCode, TParameter parameter);
}
