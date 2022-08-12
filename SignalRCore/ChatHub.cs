using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalRCore
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string userName, string message) 
        { 
            return Clients.All.SendAsync("ReceiveMessage", userName.ToUpper(), message.ToUpper()); 
        }
    }
}
