using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oluso.HealthCheck.Checkers;
using Oluso.HealthCheck.Settings;

namespace Oluso.HealthCheck.Extensions;

/// <summary>
/// Health check service collection related extensions
/// </summary>
public static class HealthCheckServiceCollectionExtensions
{
    /// <summary>
    /// add memory health check service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddHealthCheckService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection("HealthCheck")
            .Get<HealthCheckSettings>();
        
        services.AddHostedService<StartupHostedService>();
        services.AddSingleton<StartupHostedServiceHealthCheck>();

        var healthCheck = services.AddHealthChecks();
        healthCheck
            .AddCheck<StartupHostedServiceHealthCheck>(
                MemoryHealthCheckSettings.DefaultMemoryHealthCheckFullName,
                HealthStatus.Degraded, new string[] { "ready" });
        
        if (settings.IsEnabled)
        {
            if(settings.MemoryHealthCheckSettings?.Enabled == true)
            {
                services.AddMemoryHealthCheckService(healthCheck, settings.MemoryHealthCheckSettings!);
            }
        }
        
        services.Configure<HealthCheckPublisherOptions>(opts =>
        {
            opts.Delay = TimeSpan.FromSeconds(2.0);
            opts.Predicate = check => check.Tags.Contains("ready");
        });
        
        return services;
    }

    /// <summary>
    /// add health check service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddHealthCheckService(
        this IServiceCollection services,
        IHealthChecksBuilder builder,
        Action<HealthCheckOptions> options)
    {
        var settings = options!.ToHealthCheckConfigureOrDefault();
        if (settings.IsHealthCheckEnabled)
        {
            services.AddMemoryHealthCheckService(builder, settings.MemoryHealthCheckOptions);
        }
        return services;
    }

    /// <summary>
    /// add the memory health check service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="memoryHealthCheckSettings"></param>
    /// <returns></returns>
    public static IServiceCollection AddMemoryHealthCheckService(
        this IServiceCollection services,
        IHealthChecksBuilder builder,
        MemoryHealthCheckSettings memoryHealthCheckSettings)
    {
        builder.AddMemoryHealthCheck(memoryHealthCheckSettings.MemoryHealthCheckName!);
        return services;
    }

    /// <summary>
    /// add the memory health check service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddMemoryHealthCheckService(
        this IServiceCollection services,
        IHealthChecksBuilder builder,
        Action<MemoryHealthCheckOptions> options)
    {
        var settings = options!.ToHealthCheckConfigureOrDefault();
        if (settings.Settings.Enabled == true)
        {
            services.AddMemoryHealthCheckService(builder, settings.Settings);
        }
        return services;
    }
}