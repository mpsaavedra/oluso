using Configuration.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Client;
using Oluso.Configuration.Client.Extensions;
using Oluso.Configuration.Client.Parsers;
using Oluso.Configuration.Client.Settings;
//
// var loggerFactory = LoggerFactory.Create(builder =>
// {
//     builder.AddConsole();
// });
//
// var localConfiguration = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .Build();
//
// var configuration = new ConfigurationBuilder()
//     .AddConfiguration(localConfiguration)
//     .AddRemoteConfiguration(opts =>
//     {
//         opts.ServiceUri = $"https://localhost:7162/configuration/";
//         opts.AddConfiguration(cfg =>
//         {
//             cfg.ConfigurationName = "Config.json";
//             cfg.ReloadOnChange = true;
//             cfg.Optional = false;
//         });
//         opts.AddConfiguration(cfg =>
//         {
//             cfg.ConfigurationName = "Service1/service.ini";
//             cfg.ReloadOnChange = true;
//             cfg.Optional = false;
//             cfg.Parser = new IniConfigurationParser();
//         });
//         opts.AddRedisSubscriber("localhost:6379");
//         opts.AddLoggerFactory(loggerFactory);
//     })
//     .Build();
//

var services = new ServiceCollection();
var configuration = (IConfigurationRoot)services.AddRemoteConfiguration(opts =>
{
    opts
        .WithServerUri($"https://localhost:7162/configuration/")
        .WithConfigurationFileSetting(
            ConfigurationFileSettings.Create("appsettings.json", false, true))
        .WithConfiguration(
            ConfigurationOptions.Create("Config.json", false, true))
        .WithRedisOptions($"localhost:6379")
        .WithLoggerFactory(
            LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            })
        );
}, false);
services.AddSingleton<ConfigWriter>();
services.Configure<Config>(configuration.GetSection("Config"));

var serviceProvider = services.BuildServiceProvider();
var configWriter = serviceProvider.GetService<ConfigWriter>();

await configWriter?.Write()!;