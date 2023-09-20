using Microsoft.AspNetCore.SignalR;
using Oluso.Notifier;
using Oluso.Notifier.SignalR;

namespace Notifier.Hubs;

public interface IChatHub : IBaseNotification
{
    Task SendMessage(string user, string message);
}

public class ChatHub: SignalRHub<IChatHub>
{
    public async Task SendMessage(string user, string message)
    {
        await (this as Hub).Clients.All.SendAsync("ReceiveMessage", user, message)!;
    }
}