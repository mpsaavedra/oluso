using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Extensions;
using Oluso.Plugins;
using Plugins.Sample.Shared;

namespace Plugins.Sample.Plugin2;

public class BannerPlugin : IAsyncPlugin<string>
{
    public string Name => "Banner printer plugin";
    public string ShortName => "banner_plugin";
    public string Version => "0.1.0";
    public Guid PluginId => Constants.BannerPluginGuid;
    public string? Author => "Michel Perez";
    public string? Description => "A simple plugin to display a banner";
    public bool Enable { get; set; } = true;
    public int Level { get; set; } = 0;
    public WebApplication? Configure(WebApplication builder) => builder;

    public IServiceCollection? ConfigureServices(IServiceCollection services) => services;

    public async Task Run(string parameter)
    {
        parameter.ToBanner();
        Console.WriteLine(parameter);
    }
}