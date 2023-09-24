using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oluso.Plugins;
using Oluso.Plugins.Services;
using Plugins.Sample.Shared;

namespace Plugins.Sample.Loader;

public class Tester
{
    private readonly IServiceProvider _provider;

    public Tester(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    public static Tester New(IServiceProvider hostProvider) => new(hostProvider);
    public async Task<Tester> Count()
    {
        using var svrScope = _provider.CreateScope();
        var provider = svrScope.ServiceProvider;
        var chain = provider.GetRequiredService<IChainOfResponsibilityService>();
        var result = await chain.ExecuteAsyncChain<TestPayload, TestReturn>(
            EventCodes.MakeItCount.ToString(),
            new TestPayload());
        Console.WriteLine(result.ToString());
        return this;
    }

    public Tester Banner()
    {
        using var svrScope = _provider.CreateScope();
        var provider = svrScope.ServiceProvider;
        var svr = provider.GetRequiredService<IPluginService>();
        var plugin = svr.Get<IAsyncPlugin<string>>(Constants.BannerPluginGuid).Result;
        plugin.Run("Banner").ConfigureAwait(false);
        return this;
    }

    public Tester Message()
    {
        using var svrScope = _provider.CreateScope();
        var provider = svrScope.ServiceProvider;
        var svr = provider.GetRequiredService<IPluginService>();
        var plugin = svr.Get<IAsyncPlugin<string>>(Constants.MessagePluginName).Result;
        plugin.Run("This message is from a plugin").ConfigureAwait(false);
        return this;
    }
}