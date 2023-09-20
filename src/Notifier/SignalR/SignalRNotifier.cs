using Microsoft.AspNetCore.SignalR;
using Oluso.Extensions;

namespace Oluso.Notifier.SignalR;

/// <summary>
/// <inheritdoc cref="INotifier"/> using the SignalR hub channel.
/// 
/// <code>
/// var hClient = new ServiceHubClient(loggerFactory.CreateLogger{ServiceHubClient}());
/// services.AddSingleton{ServiceHubClient}(hClient);
/// services.AddSingleton{INotifier, SignalRNotifier}();
/// </code>
/// </summary>
/// <typeparam name="THub">Hub client</typeparam>
public class SignalRNotifier<THub> : INotifier
    where THub: Hub, IBaseNotification
{
    private readonly IHubContext<Hub<THub>, THub> _handler;

#pragma warning disable CS8618
    /// <summary>
    /// returns a new <see cref="SignalRNotifier{THub}"/> instance
    /// </summary>
    /// <param name="handler">underlying hub <see cref="IHubContext{THub}"/> used to send messages</param>
    public SignalRNotifier(IHubContext<Hub<THub>, THub> handler)
#pragma warning restore CS8618
    {
        _handler = handler;
    }

    /// <summary>
    /// <inheritdoc cref="INotifier.Message"/>
    /// </summary>
    public HandlerMessage? Message => HandlerMessage.Instance;

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyUpdateAsync"/>
    /// </summary>
    /// <param name="msg"></param>
    public async Task NotifyUpdateAsync(HandlerMessage? msg = null)
    {
        msg ??= HandlerMessage.Instance;
        msg.MessageLevel = MessageLevel.Information;
        msg = HandlerMessage.Instance.FromMessage(msg);
        await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
    }

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyUpdate"/>
    /// </summary>
    /// <param name="msg"></param>
    public void NotifyUpdate(HandlerMessage? msg = null) =>
        Task.Factory.StartNew(async () => await NotifyUpdateAsync(msg));

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyErrorAsync"/>
    /// </summary>
    /// <param name="msg"></param>
    public async Task NotifyErrorAsync(HandlerMessage? msg = null)
    {
        msg ??= HandlerMessage.Instance;
        msg.MessageLevel = MessageLevel.Error;
        msg = HandlerMessage.Instance.FromMessage(msg);
        await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
    }

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyError"/>
    /// </summary>
    /// <param name="msg"></param>
    public void NotifyError(HandlerMessage? msg = null)=>
        Task.Factory.StartNew(async () => await NotifyErrorAsync(msg));
    
    
    private static object AsHubMessage(HandlerMessage message) => new
    {
        Stage = message.Status.ToString(),
        Finish = message.Status == ProcessStatus.Completed,
        Messages = message.AllMessages,
        message.ErrorMessages,
        message.Message,
        message.Data,
    };
}