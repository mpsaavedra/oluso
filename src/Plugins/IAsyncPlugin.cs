namespace Oluso.Plugins;

/// <summary>
/// defines a basic async plugin that returns a value
/// </summary>
/// <typeparam name="TParameter"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public interface IAsyncPlugin<in TParameter, TReturn>: ICommonPlugin
{
    /// <summary>
    /// runs the plugin and returns the execution result
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    Task<TReturn> Run(TParameter parameter);
}

/// <summary>
/// defines the basic async plugin
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IAsyncPlugin<in TParameter>: ICommonPlugin
{
    /// <summary>
    /// runs the middleware
    /// </summary>
    /// <param name="parameter">parameter passed to the plugin</param>
    /// <returns></returns>
    Task Run(TParameter parameter);
}