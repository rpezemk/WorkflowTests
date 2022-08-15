using Microsoft.AspNet.SignalR;
using Microsoft.AspNet;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR;


namespace SignalRExample
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
    }
}
