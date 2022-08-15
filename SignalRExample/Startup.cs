using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

[assembly: OwinStartup(typeof(SignalRExample.Startup))]

namespace SignalRExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }

    }
}
