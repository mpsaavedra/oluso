namespace Oluso.Notifier;

/// <summary>
/// System notifier
/// </summary>
public interface INotifier
{
    /// <summary>
    /// <see cref="HandlerMessage"/> instance with message to send
    /// </summary>
    HandlerMessage? Message { get; }

    /// <summary>
    /// send an async <see cref="HandlerMessage"/> update message
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    Task NotifyUpdateAsync(HandlerMessage? msg = null);

    /// <summary>
    /// send a sync <see cref="HandlerMessage"/> update message
    /// </summary>
    /// <param name="msg"></param>
    void NotifyUpdate(HandlerMessage? msg = null);
        
    /// <summary>
    /// send an async <see cref="HandlerMessage"/> error message
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    Task NotifyErrorAsync(HandlerMessage? msg = null);

    /// <summary>
    /// send a sync <see cref="HandlerMessage"/> error message
    /// </summary>
    /// <param name="msg"></param>
    void NotifyError(HandlerMessage? msg = null);
}