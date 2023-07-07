using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //string username = Context.User.GetCurrentUserName(); //TODO: why do we get null here???
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetCurrentUserName());//TODO: why do we get null here???
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetCurrentUserName());//TODO: why do we get null here???
            await base.OnDisconnectedAsync(exception);
        }
    }
}