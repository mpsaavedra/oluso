using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Internal;
using Oluso.Extensions;

namespace Oluso.Notifier.SignalR;

/// <summary>
/// Base class for the SignalR Hub, this class implements the <see cref="IBaseNotification"/>
/// interface that has the method <see cref="IBaseNotification.OnUpdateStatus"/> that's on
/// charge of receive an object with the data to send to the clients to notify any update.
/// </summary>
/// <typeparam name="THub">
///     Hub interface declaring the methods to be used to send/receive messages to/from clients
/// </typeparam>
public class SignalRHub<THub> : Hub<THub>, IBaseNotification 
    where THub : class, IBaseNotification
{
    /// <inheritdoc cref="IBaseNotification.OnUpdateStatus"/>
    public async Task OnUpdateStatus(object? data = null)
    {
        data ??= HandlerMessage.Instance.Message;
        await (this as Hub).Clients.All.SendAsync("OnUpdateStatus", data);
    }

    /// <summary>
    /// get/set the clients from an strong typed <see cref="Hub{T}"/> where
    /// T is of type THub. This are the clients to be used to send/receive messages
    /// to/from connected clients.
    /// </summary>
    public IHubCallerClients HubClients => (this as Hub).Clients;
    
    public Hub Hub => (this as Hub);
}