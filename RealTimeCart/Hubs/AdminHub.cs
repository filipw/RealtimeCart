using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace RealTimeCart.Hubs
{
    [HubName("admin")]
    public class AdminHub : Hub
    { }
}