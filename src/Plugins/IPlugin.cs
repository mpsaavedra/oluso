using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Oluso.Plugins;

/// <summary>
/// Basic plugin information, fields must be defined at development time
/// with the idea t keep it as unique as possible to avoid data crashes
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// plugin's fullname
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// plugin's shortname
    /// </summary>
    string ShortName { get; }
    
    /// <summary>
    /// plugin's version
    /// </summary>
    string Version { get; }
    
    /// <summary>
    /// unique id of the plugin
    /// </summary>
    Guid PluginId { get; }
    
    /// <summary>
    /// plugin's author 
    /// </summary>
    string? Author { get; }
    
    /// <summary>
    /// plugin's description
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// if true the plugin is enable to be use
    /// </summary>
    bool Enable { get; set; }

    /// <summary>
    /// set the level in execution 0 is first and growth up, this level
    /// should be carefully keep because data processing is important when 
    /// accessing to formatted data. so 0 is the first executed plugin.
    /// </summary>
    int Level { get; set; }

    /// <summary>
    /// Configure plugin IApplicationBuilder if required. This method could be used to call initialization
    /// processes.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    WebApplication? Configure(WebApplication builder);

    /// <summary>
    /// Configure plugin services if required. This method could be used to call initialization
    /// processes.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    IServiceCollection? ConfigureServices(IServiceCollection services);
}