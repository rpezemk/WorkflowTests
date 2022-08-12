using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace SignalRCore
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CreateWebHostBuilder(args).Build().Run();
            Console.ReadLine();
            Console.ReadLine();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

    }
}
