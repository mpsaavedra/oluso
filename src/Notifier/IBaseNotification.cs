namespace Oluso.Notifier;

/// <summary>
/// base for notifications, this should be the parent interface
/// in the SignalR usage case.
/// </summary>
public interface IBaseNotification
{
    /// <summary>
    /// Send an update status message
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task OnUpdateStatus(object data);
}