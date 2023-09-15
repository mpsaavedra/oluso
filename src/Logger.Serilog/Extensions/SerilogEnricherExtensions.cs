using Oluso.Logger.Abstractions.Extensions;
using Oluso.Logger.Serilog.Enrichers;
using Serilog;
using Serilog.Configuration;

namespace Oluso.Logger.Serilog;

/// <summary>
/// add different Enrichers
/// </summary>
public static class SerilogEnricherExtensions
{
    /// <summary>
    /// add <see cref="ThreadEnricher"/>, to log id and thread name
    /// </summary>
    /// <param name="enrichementConfiguration"></param>
    /// <returns></returns>
    public static LoggerConfiguration WithThread(this LoggerEnrichmentConfiguration enrichementConfiguration) =>
        enrichementConfiguration.IsNullOrEmptyThrow(nameof(enrichementConfiguration)).With<ThreadEnricher>();

    /// <summary>
    /// add <see cref="MachineNameEnricher"/> to the log with the name of the machine
    /// </summary>
    /// <param name="enrichmentConfiguration"></param>
    /// <returns></returns>
    public static LoggerConfiguration WithMachineName(this LoggerEnrichmentConfiguration enrichmentConfiguration) =>
        enrichmentConfiguration.IsNullOrEmptyThrow(nameof(enrichmentConfiguration)).With<MachineNameEnricher>();

    /// <summary>
    /// add <see cref="ProcessEnricher"/> to the log with the id and process name
    /// </summary>
    /// <param name="enrichmentConfiguration"></param>
    /// <returns></returns>
    public static LoggerConfiguration WithProcess(this LoggerEnrichmentConfiguration enrichmentConfiguration) =>
        enrichmentConfiguration.IsNullOrEmptyThrow(nameof(enrichmentConfiguration)).With<ProcessEnricher>();

    /// <summary>
    /// add <see cref="TitleEnricher"/> to the log
    /// </summary>
    /// <param name="enrichmentConfiguration"></param>
    /// <returns></returns>
    public static LoggerConfiguration WithTitle(this LoggerEnrichmentConfiguration enrichmentConfiguration) =>
        enrichmentConfiguration.IsNullOrEmptyThrow(nameof(enrichmentConfiguration)).With<TitleEnricher>();
}