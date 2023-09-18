using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oluso.HealthCheck.Checkers;

namespace Oluso.HealthCheck;

/// <summary>
/// Startup hosted service 
/// </summary>
public class StartupHostedService : IHostedService, IDisposable
{
    private readonly int _delaySeconds = 5;
    private readonly ILogger<StartupHostedService> _logger;
    private readonly StartupHostedServiceHealthCheck _startupHostedServiceHealthCheck;

    /// <summary>
    /// return a new <see cref="StartupHostedService"/> instance
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="startupHostedServiceHealthCheck"></param>
    public StartupHostedService(ILogger<StartupHostedService> logger,
        StartupHostedServiceHealthCheck startupHostedServiceHealthCheck)
    {
        _logger = logger;
        _startupHostedServiceHealthCheck = startupHostedServiceHealthCheck;
    }
    
    /// <summary>
    /// start the task
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Startup Background Service is starting...");
        Task.Run((Func<Task>)(async () =>
        {
            await Task.Delay(_delaySeconds * 1000, cancellationToken);
            _startupHostedServiceHealthCheck.StartupTaskCompleted = true;
            _logger.LogDebug("Startup Background Service has started");
        }), cancellationToken);
        return Task.CompletedTask;
    }

    /// <summary>
    /// stop the service 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Startup Background Service is stopping");
        return Task.CompletedTask;
    }

    /// <summary>
    /// dispose used resources
    /// </summary>
    public void Dispose()
    {
    }
}