namespace Oluso.Notifier;

/// <summary>
/// stage in which the process is in
/// </summary>
public enum ProcessStatus : byte
{
    /// <summary>
    /// not available
    /// </summary>
    NotAvailable = 2,
    
    /// <summary>
    /// initializing 
    /// </summary>
    Initializing = 4,

    /// <summary>
    /// processing mapping
    /// </summary>
    PreProcessing = 8,

    /// <summary>
    /// Processing data
    /// </summary>
    Processing = 16,

    /// <summary>
    /// notify that process has finished and show process 
    /// results
    /// </summary>
    Completed = 32
}