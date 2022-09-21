using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TestingApp
{
    internal static class Notifier
    {


        public class Query
        {
            public Hero GetHero() => new Hero();
        }

        public class Hero
        {
            public string Name => "Luke Skywalker";
        }
        internal static async void SendWorkflowGraph(List<IStep> serialized)
        {
            WebHost
.CreateDefaultBuilder(args)
.ConfigureServices(services =>
   services
       .AddGraphQLServer()
       .AddQueryType<Query>())
.Configure(builder =>
   builder
       .UseRouting()
       .UseEndpoints(e => e.MapGraphQL()))
.Build()
.Run();
            var res = @"mutation {
                            pages {
                                create (
                                    content: "" ## jezdobsze wiec spox ""
                                    description: ""test3""
                                    editor: ""markdown""
                                    isPublished: true
                                    isPrivate: false
                                    locale: ""pl""
                                    path: ""/home/GodModeGen999000""
                                    tags:[""#someTag""]
                                    title: ""PZ_TEST""
                                ) {
                                            responseResult {
                                                succeeded
                                                slug
                                                message
                                            }
                                            page {
                                                content
                                                id
                                            }
                                        }
                                    }
                                }";


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://10.1.2.88:5555/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhcGkiOjEsImdycCI6MSwiaWF0IjoxNjYzNDEwOTMxLCJleHAiOjE3NTgwODM3MzEsImF1ZCI6InVybjp3aWtpLmpzIiwiaXNzIjoidXJuOndpa2kuanMifQ.dSzRx2hqkiV6MDFJCKQ4xkCKPXVZkkERbAYFBI8p86NdC57d-Rk7RlhzXjCgjISKR-YaRRJ7Dxdku-xcGwBKtTpjddZRSl7yGMWeXEUnj1fY55AKqRG_N6ZbpR_QWMx0HMzKamMz_Z_0spObBIUEulswabIUzicp_fEVxqIP98OVI-j67_knFBRut6kmO8c3hlksGWHinivNF5GwBF5A8PC_b7wNwV0rwRfXcEGWy7xDQVFlAEJ_FlpBCbEtdXBQL1CE2hjw7ILbzakmCxiem44uqkg4pZ0Eoz4B7cdNSQY1EZT_Sjry0IgHkYSngR5hXwo1SZO3FBXdFreYqQLQjQ");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8"); 
                client.DefaultRequestHeaders.Add("body", res);
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("body", res)
            });
                var result = client.PostAsync("/graphql", content);
                string resultContent = await result.Result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
            }

            //foreach(var s in serialized)
            //{
            //    res += $@"{s.GetName()}{}";
            //}
        }
    }
}
