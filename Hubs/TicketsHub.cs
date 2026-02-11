using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TurneroApi.Hubs;

 [Authorize]
public sealed class TicketsHub : Hub
{
}
