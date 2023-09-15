using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Configuration.Abstractions;

namespace Oluso.Configuration.Hosting.Extensions;

/// <summary>
/// Route endpoint extensions to register the configuration endpoints
/// </summary>
public static class ConfigurationRouteEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the route to access the configuration files in the system
    /// </summary>
    public static IEndpointConventionBuilder MapConfigurationService(this IEndpointRouteBuilder endpoints,
        string pattern = "/configuration")
    {
        if (endpoints == null)
        {
            throw new ArgumentNullException(nameof(endpoints));
        }

        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        var conventionBuilders = new List<IEndpointConventionBuilder>();

        var listConfigurationBuilder = endpoints.RegisterListRoute(pattern);
        conventionBuilders.Add(listConfigurationBuilder);

        var fileConfigurationBuilder = endpoints.RegisterFileRoute(pattern);
        conventionBuilders.Add(fileConfigurationBuilder);

        return new CompositeEndpointConventionBuilder(conventionBuilders);
    }

    private static IEndpointConventionBuilder RegisterListRoute(this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
    {
        var provider = endpointRouteBuilder.ServiceProvider.GetService<IProvider>();

        return endpointRouteBuilder.MapGet(pattern, async context =>
        {
            List<string> files = new();

            if (provider != null)
                files = (await provider.ListPaths()).ToList();

            context.Response.OnStarting(async () =>
            {
                await JsonSerializer.SerializeAsync(context.Response.Body, files);
            });

            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.Body.FlushAsync();
        });
    }

    private static IEndpointConventionBuilder RegisterFileRoute(this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
    {
        var provider = endpointRouteBuilder.ServiceProvider.GetService<IProvider>();

        return endpointRouteBuilder.MapGet(pattern + "/{name}", async context =>
        {
            var name = context.GetRouteValue("name")?.ToString();
            name = WebUtility.UrlDecode(name);

            byte[] bytes = new byte[] { };

            if (provider != null && !string.IsNullOrEmpty(name))
                bytes = await provider.GetConfiguration(name);

            if (bytes == null || bytes.Count() == 0)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var fileContent = Encoding.UTF8.GetString(bytes);

            await context.Response.WriteAsync(fileContent);
            await context.Response.Body.FlushAsync();
        });
    }

    private class CompositeEndpointConventionBuilder : IEndpointConventionBuilder
    {
        private readonly List<IEndpointConventionBuilder> _endpointConventionBuilders;

        public CompositeEndpointConventionBuilder(List<IEndpointConventionBuilder> endpointConventionBuilders)
        {
            _endpointConventionBuilders = endpointConventionBuilders;
        }

        public void Add(Action<EndpointBuilder> convention)
        {
            foreach (var endpointConventionBuilder in _endpointConventionBuilders)
            {
                endpointConventionBuilder.Add(convention);
            }
        }
    }
}