using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Plugins;
using Plugins.Sample.Shared;

namespace Plugins.Sample.Plugin2;

public class TestMiddleware2 : IAsyncMiddleware<TestPayload, TestReturn>
{
    // create an application scope so we could call it to get access to any
    // available service
    private IServiceProvider? _provider;
    
    public WebApplication? Configure(WebApplication builder)
    {
        // get access to the service provider
        _provider = builder.Services;
        return builder;
    }

    public IServiceCollection? ConfigureServices(IServiceCollection services) => services;
    
    public async Task<TestReturn> Run(TestPayload parameter, Func<TestPayload, Task<TestReturn>> next)
    {
        Console.WriteLine("Test Middleware 2 executed");
        
        // now you could use the _provider to create and scope and have access to any of the registered
        // services, for example
        // using var scope = _provider.CreateScope();
        // using var repository = scope.ServiceProvider.GetRequiredService<ICountRepository>();
        // repository.Create(new CountdownEvent() { Counter = parameter.Counter });

        parameter.Counter++;
        
        return await Task.FromResult(new TestReturn()
        {
            Counter = parameter.Counter++,
            Name = parameter.Name
        });
    }

    public string Name => "Sample Test Plugin - Plugin2";
    public string ShortName => "plugin2";
    public string Version => "0.0.1";
    public Guid PluginId => Guid.NewGuid();
    public string? Author => "Michel Perez";
    public string? Description => "A sample plugin to execute something in as middleware in a chain of responsibility";
    public bool Enable { get; set; } = true;
    public int Level { get; set; } = 2;

    public string? EventCode  => EventCodes.MakeItCount.ToString();
}