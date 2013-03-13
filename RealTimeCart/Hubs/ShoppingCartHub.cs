using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace RealTimeCart.Hubs
{
    [HubName("cart")]
    public class ShoppingCartHub : Hub
    {}
}