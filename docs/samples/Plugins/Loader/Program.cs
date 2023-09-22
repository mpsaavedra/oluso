using Microsoft.AspNetCore.Builder;
using Oluso.Plugins;
using Oluso.Plugins.Extensions;
using Plugins.Sample.Loader;
using Plugins.Sample.Shared;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .AddPluginsService(
        Path.Combine(System.Environment.CurrentDirectory, "..", "..", "..", "..", "Builded"),
        new Type[]
        {
            // include the  Plugins package types that you use in your plugins
            // this will add them to the Application context and make available
            // to load the plugins correctly
            // typeof(IPlugin),
            typeof(IAsyncPlugin<>),
            typeof(IMiddlewarePlugin),
            // typeof(IAsyncMiddleware<,>),
            
            // share types used as parameters and results
            typeof(TestPayload),
            typeof(TestReturn)
        });

var app = builder.Build();
app.ConfigurePlugins();
Tester
    .New(app.Services)
    .Count().Result
    .Banner()
    .Message();  