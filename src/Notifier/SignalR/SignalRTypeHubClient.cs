// using Microsoft.AspNetCore.SignalR;
//
// namespace Oluso.Notifier.SignalR;
//
// public class SignalRTypeHubClient : IHubCallerClients<T>, IHubClients<T>
// {
//
// #nullable disable
//     private readonly IHubCallerClients _hubClients;
//
//
// #nullable enable
//     public SignalRTypeHubClient(IHubCallerClients dynamicContext) => this._hubClients = dynamicContext;
//
//     public T All => TypedClientBuilder<T>.Build(this._hubClients.All);
//
//     public T Caller => TypedClientBuilder<T>.Build(this._hubClients.Caller);
//
//     public T Others => TypedClientBuilder<T>.Build(this._hubClients.Others);
//
//     public T AllExcept(IReadOnlyList<string> excludedConnectionIds) => TypedClientBuilder<T>.Build(this._hubClients.AllExcept(excludedConnectionIds));
//
//     public T Client(string connectionId) => TypedClientBuilder<T>.Build(this._hubClients.Client(connectionId));
//
//     public T Group(string groupName) => TypedClientBuilder<T>.Build(this._hubClients.Group(groupName));
//
//     public T GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => TypedClientBuilder<T>.Build(this._hubClients.GroupExcept(groupName, excludedConnectionIds));
//
//     public T Clients(IReadOnlyList<string> connectionIds) => TypedClientBuilder<T>.Build(this._hubClients.Clients(connectionIds));
//
//     public T Groups(IReadOnlyList<string> groupNames) => TypedClientBuilder<T>.Build(this._hubClients.Groups(groupNames));
//
//     public T OthersInGroup(string groupName) => TypedClientBuilder<T>.Build(this._hubClients.OthersInGroup(groupName));
//
//     public T User(string userId) => TypedClientBuilder<T>.Build(this._hubClients.User(userId));
//
//     public T Users(IReadOnlyList<string> userIds) => TypedClientBuilder<T>.Build(this._hubClients.Users(userIds));
// }