using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Plugins;
using Plugins.Sample.Shared;

namespace Plugins.Sample.Plugin1;

public class TestMiddleware1 : IAsyncMiddleware<TestPayload, TestReturn>
{
    public async Task<TestReturn> Run(TestPayload parameter, Func<TestPayload, Task<TestReturn>> next)
    {
        Console.WriteLine("Test Middleware 1 executed");
        
        // place your in here
        parameter.Counter++;
        
        // you could break the chain in here if is required avoiding to keep executing the chain
        // return new TestReturn()
        // {
        //     Name = parameter.Name,
        //     Counter = parameter.Counter
        // };
        //
        // return the result of the execution of the next middleware in the chain,
        // this is the most common use 
        return await next(parameter);
    }

    public string Name  => "Sample Test Plugin - Plugin1";
    public string ShortName  => "plugin2";
    public string Version  => "0.0.1";
    public Guid PluginId  => Guid.NewGuid();
    public string? Author  => "Michel Perez";
    public string? Description  => "A sample plugin to execute something in as middleware in a chain of responsibility";
    public bool Enable { get; set; } = true;
    public int Level { get; set; } = 1;
    public WebApplication? Configure(WebApplication builder) => builder;

    public IServiceCollection? ConfigureServices(IServiceCollection services) => services;

    public string? EventCode { get; } = EventCodes.MakeItCount.ToString();
}