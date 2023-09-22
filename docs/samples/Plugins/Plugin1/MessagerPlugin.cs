using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Plugins;
using Plugins.Sample.Shared;

namespace Plugins.Sample.Plugin1;

public class MessagePlugin : IAsyncPlugin<string>
{
    // create an application scope so we could call it to get access to any
    // available service
    private IServiceProvider? _provider;
    
    public string Name => "Message Plugin";
    public string ShortName => Constants.MessagePluginName;
    public string Version => "0.1.0";
    public Guid PluginId => Constants.BannerPluginGuid;
    public string? Author => "";
    public string? Description => "";
    public bool Enable { get; set; } = true;
    public int Level { get; set; } = 0;
    public WebApplication? Configure(WebApplication builder)
    {
        // get access to the service provider
        _provider = builder.Services;
        return builder;
    }

    public IServiceCollection? ConfigureServices(IServiceCollection services) => services;
    
    
    public async Task Run(string parameter)
    {
        // now you could use the _provider to create and scope and have access to any of the registered
        // services, for example
        // using var scope = _provider.CreateScope();
        // using var repository = scope.ServiceProvider.GetRequiredService<ICountRepository>();
        // repository.Create(new CountdownEvent() { Counter = parameter.Counter });
        Console.WriteLine($"Message from MessagePlugin: {parameter}");
    }
}