namespace Oluso.Plugins;

/// <summary>
/// Defines a middleware plugins information. middleware execution is order sensitive, this means that
/// the executed order could affect the middleware result. in a common middleware the execution order
/// depend of the loading order, in this plugin system where plugins are automatically loaded, loading
/// could be difficult
/// </summary>
public interface IMiddlewarePlugin : IPlugin
{
    /// <summary>
    /// this code is mainly set as a constant name in the function in which the plugins will be
    /// used are refer and used by operations. if no EventCode specified this plugin could be used
    /// by other middleware plugins
    /// </summary>
    string? EventCode { get; }
}