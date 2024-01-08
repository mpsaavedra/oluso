using Microsoft.AspNetCore.SignalR;
using Oluso.Notifier;
using Oluso.Notifier.SignalR;

namespace Notifier.Hubs;

public class ChatNotifier : SignalRNotifier<ChatHub>
{
    public ChatNotifier(IHubContext<Hub<ChatHub>, ChatHub> handler) : base(handler)
    {
    }
}