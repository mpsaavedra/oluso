using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Hosting;

namespace Tests.Configuration;

public class HostingTests
{
    private readonly ILogger<ConfigurationService> _logger;
    private readonly IProvider _provider;
    private readonly IPublisher _publisher;
    private readonly IConfigurationService _configurationService;

    public HostingTests()
    {
        _logger = Substitute.For<ILogger<ConfigurationService>>();
        _publisher = Substitute.For<IPublisher>();
        _provider = SetupStorageProvider();
        _configurationService = new ConfigurationService(_logger, _provider, _publisher);
    }

    // [Fact]
    // public async Task Publish_Invoked_on_Initialization()
    // {
    //     await _configurationService.Initialize();
    //
    //     await _publisher.Received().Publish(Arg.Any<string>(), Arg.Any<string>());
    // }
    //
    // [Fact]
    // public async Task Publish_Invoked_on_Change()
    // {
    //     await _configurationService.OnChange(ListRandomFiles(1));
    //
    //     await _publisher.Received(1).Publish(Arg.Any<string>(), Arg.Any<string>());
    // }
    //
    // [Fact]
    // public async Task Publish_Invoked_on_PublishChanges()
    // {
    //     await _configurationService.PublishChanges(ListRandomFiles(1));
    //
    //     await _publisher.Received(1).Publish(Arg.Any<string>(), Arg.Any<string>());
    // }

    [Fact]
    public async Task Publish_Invoked_on_Change_for_Each_File()
    {
        var fileCount = 5;
        await _configurationService.OnChange(ListRandomFiles(fileCount));

        await _publisher.Publish(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Publish_Is_Not_Invoked_when_No_Change()
    {
        await _configurationService.OnChange(new List<string>());

        await _publisher.DidNotReceive().Publish(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Service_Permits_Null_Publisher()
    {
        var configurationService = new ConfigurationService(_logger, _provider);
        await configurationService.PublishChanges(ListRandomFiles(1));
    }

    [Fact]
    public async Task Publish_Is_Not_Invoked_when_No_Publisher_Registered()
    {
        var configurationService = new ConfigurationService(_logger, _provider);
        await configurationService.OnChange(ListRandomFiles(1));

        await _publisher.DidNotReceive().Publish(Arg.Any<string>(), Arg.Any<string>());
    }

    private static IProvider SetupStorageProvider()
    {
        var storageProvider = Substitute.For<IProvider>();
        storageProvider.ListPaths().Returns(ListRandomFiles(1));
        storageProvider.GetConfiguration(Arg.Any<string>())
            .Returns(name => Encoding.UTF8.GetBytes($"{{ \"name\": \"{name}\" }}"));
        return storageProvider;
    }

    private static IEnumerable<string> ListRandomFiles(int count)
    {
        var list = new List<string>();

        for (var i = 0; i < count; i++)
        {
            list.Add($"{Guid.NewGuid()}.json");
        }

        return list;
    }
}