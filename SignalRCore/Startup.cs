using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRCore
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSignalR();
        }
        public void Configure(IApplicationBuilder builder)
        {
            builder.UseRouting();
            builder.UseEndpoints(endpoints => { endpoints.MapHub<ChatHub>("/ChatHub"); });
        }

  //      System.InvalidOperationException
  //HResult = 0x80131509
  //Message=EndpointRoutingMiddleware matches endpoints setup by EndpointMiddleware and so must be added to the request execution pipeline before EndpointMiddleware.Please add EndpointRoutingMiddleware by calling 'IApplicationBuilder.UseRouting' inside the call to 'Configure(...)' in the application startup code.
  //Source= Microsoft.AspNetCore.Routing
  //StackTrace:
  // at Microsoft.AspNetCore.Builder.EndpointRoutingApplicationBuilderExtensions.VerifyEndpointRoutingMiddlewareIsRegistered(IApplicationBuilder app, DefaultEndpointRouteBuilder& endpointRouteBuilder)
  // at Microsoft.AspNetCore.Builder.EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder builder, Action`1 configure)
  // at SignalRCore.Startup.Configure(IApplicationBuilder builder) in C:\Users\Przemek.Zaremba\source\repos\TestWebsockets\SignalRCore\Startup.cs:line 20
  // at System.RuntimeMethodHandle.InvokeMethod(Object target, Object[] arguments, Signature sig, Boolean constructor, Boolean wrapExceptions)
  // at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
  // at Microsoft.AspNetCore.Hosting.ConfigureBuilder.Invoke(Object instance, IApplicationBuilder builder)
  // at Microsoft.AspNetCore.Hosting.ConfigureBuilder.<>c__DisplayClass4_0.<Build>b__0(IApplicationBuilder builder)
  // at Microsoft.AspNetCore.Hosting.ConventionBasedStartup.Configure(IApplicationBuilder app)


    }
}
