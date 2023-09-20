namespace Oluso.Notifier;

/// <summary>
/// define the required methods that must be implemented in the specific
/// notification mechanism. The OnUpdateStatus method is called when an
/// update status is required. This method is implemented in the
/// specific notifier implementation
/// </summary>
public interface IBaseNotification
{
    /// <summary>
    /// Send an update status message using the Notifier implementation,
    /// if data is null it will get the data to be send from the
    /// <see cref="HandlerMessage"/> singleton class that holds all
    /// system messages.
    /// </summary>
    /// <param name="data">data to send through notifier</param>
    /// <returns></returns>
    Task OnUpdateStatus(object? data = null);
}