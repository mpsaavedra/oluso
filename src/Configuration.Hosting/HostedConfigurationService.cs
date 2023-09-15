using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Oluso.Configuration.Hosting;

/// <summary>
/// hosted service for the configuration service
/// </summary>
public class HostedConfigurationService : IHostedService, IDisposable
{
    private readonly ILogger<HostedConfigurationService> _logger;
    private readonly IConfigurationService _service;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private Task _executingTask;
    private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

#pragma warning disable CS8618
    /// <summary>
    /// returns a new <see cref="HostedConfigurationService" /> instance
    /// </summary>
    public HostedConfigurationService(ILogger<HostedConfigurationService> logger,
        IHostApplicationLifetime applicationLifetime, IConfigurationService service)
    {
#pragma warning restore CS8618
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// start the hosted service async. it register events. <see cref="OnStarted"/>, <see cref="OnStopping" /> and
    /// <see cref="OnStopped" />
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting configuration service");
        _applicationLifetime.ApplicationStarted.Register(OnStarted);
        _applicationLifetime.ApplicationStopping.Register(OnStopping);
        _applicationLifetime.ApplicationStopped.Register(OnStopped);

        _executingTask = ExecuteAsync(cancellationToken);
        if (_executingTask.IsCompleted)
        {
            return _executingTask;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// stop the hosted service. 
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_executingTask == null)
        {
            return;
        }
        try
        {
            _stoppingCts.Cancel();
        }
        finally
        {
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }

    /// <summary>
    /// Execute the hosted process async. 
    /// </summary>
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //
        try
        {
            await _service.Initialize(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An unhandled exception occurred while attempting to initialize the configuration provider");
            _logger.LogDebug("The application will be terminated");
            await StopAsync(stoppingToken);
            _applicationLifetime.StopApplication();
        }
    }

    /// <summary>
    /// Dispose data
    /// </summary>
    public void Dispose() => _stoppingCts.Cancel();

    /// <summary>
    /// notify service is started
    /// </summary>
    public void OnStarted() => _logger.LogDebug("Configuration service started");

    /// <summary>
    /// notify service is stopping
    /// </summary>
    public void OnStopping() => _logger.LogDebug("Configuration service starting...");

    /// <summary>
    /// notify service is stopped
    /// </summary>
    public void OnStopped() => _logger.LogDebug("Configuration service stopped");
}