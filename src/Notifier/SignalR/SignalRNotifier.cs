using Microsoft.AspNetCore.SignalR;

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
/// <typeparam name="THubInterface">Hub contract interface</typeparam>
/// <typeparam name="THub">Hub client</typeparam>
public class SignalRNotifier<THubInterface, THub> : INotifier
    where THubInterface : class, IBaseNotification
    where THub: Hub<THubInterface>
{
    private readonly IHubContext<THub, THubInterface> _handler;
    private HandlerMessage _message;

#pragma warning disable CS8618
    /// <summary>
    /// returns a new <see cref="SignalRNotifier{THubInterface,THub}"/> instance
    /// </summary>
    /// <param name="handler"></param>
    public SignalRNotifier(IHubContext<THub, THubInterface> handler)
#pragma warning restore CS8618
    {
        _handler = handler;
    }

    /// <summary>
    /// <inheritdoc cref="INotifier.Message"/>
    /// </summary>
    public HandlerMessage Message => _message;

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyUpdateAsync"/>
    /// </summary>
    /// <param name="msg"></param>
    public async Task NotifyUpdateAsync(HandlerMessage msg)
    {
        msg.MessageLevel = MessageLevel.Information;
        _message = msg;
        await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
    }

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyUpdate"/>
    /// </summary>
    /// <param name="msg"></param>
    public void NotifyUpdate(HandlerMessage msg) =>
        Task.Factory.StartNew(async () => await NotifyUpdateAsync(msg));

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyErrorAsync"/>
    /// </summary>
    /// <param name="msg"></param>
    public async Task NotifyErrorAsync(HandlerMessage msg)
    {
        msg.MessageLevel = MessageLevel.Error;
        _message = msg;
        await _handler.Clients.All.OnUpdateStatus(AsHubMessage(msg));
    }

    /// <summary>
    /// <inheritdoc cref="INotifier.NotifyError"/>
    /// </summary>
    /// <param name="msg"></param>
    public void NotifyError(HandlerMessage msg)=>
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