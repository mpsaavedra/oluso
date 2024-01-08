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

    /// <inheritdoc cref="INotifier.Message"/>
    public HandlerMessage? Message => HandlerMessage.Instance;

    /// <inheritdoc cref="INotifier.NotifyUpdateAsync(Oluso.Notifier.HandlerMessage?)"/>
    public async Task NotifyUpdateAsync(HandlerMessage? msg = null)
    {
        msg ??= HandlerMessage.Instance;
        msg.MessageLevel = MessageLevel.Information;
        msg = HandlerMessage.Instance.FromMessage(msg);
        await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
    }

    /// <inheritdoc cref="INotifier.NotifyUpdateAsync(string)"/>
    public Task NotifyUpdateAsync(string msg) =>
        NotifyUpdateAsync(HandlerMessage.NewInformationMessage(msg));

    /// <inheritdoc cref="INotifier.NotifyUpdate(Oluso.Notifier.HandlerMessage?)"/>
    public void NotifyUpdate(HandlerMessage? msg = null) =>
        Task.Factory.StartNew(async () => await NotifyUpdateAsync(msg));

    /// <inheritdoc cref="INotifier.NotifyUpdate(object)"/>
    public void NotifyUpdate(object data) =>
        NotifyUpdate(HandlerMessage.NewDataMessage(data));

    /// <inheritdoc cref="INotifier.NotifyErrorAsync(Oluso.Notifier.HandlerMessage?)"/>
    public async Task NotifyErrorAsync(HandlerMessage? msg = null)
    {
        msg ??= HandlerMessage.Instance;
        msg.MessageLevel = MessageLevel.Error;
        msg = HandlerMessage.Instance.FromMessage(msg);
        await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
    }

    /// <inheritdoc cref="INotifier.NotifyErrorAsync(string)"/>
    public Task NotifyErrorAsync(string msg) =>
        NotifyErrorAsync(HandlerMessage.NewErrorMessage(msg));

    /// <inheritdoc cref="INotifier.NotifyError(Oluso.Notifier.HandlerMessage?)"/>
    public void NotifyError(HandlerMessage? msg = null)=>
        Task.Factory.StartNew(async () => await NotifyErrorAsync(msg));

    /// <inheritdoc cref="INotifier.NotifyError(string)"/>
    public void NotifyError(string msg) =>
        NotifyError(HandlerMessage.NewErrorMessage(msg));


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