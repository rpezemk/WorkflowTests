using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Client1
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chatHub").Build();
            connection.StartAsync().Wait();
            connection.InvokeCoreAsync("SendMessage", args: new[] { "Test", "Przema" });
            connection.On("ReceiveMessage", (string userName, string message) =>
            {
                Console.WriteLine(userName + ": " + message);
            });
            Console.ReadLine();
        }
    }
}
