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
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace TestingApp
{
    public class Response<T>
    {
        public T Data { get; set; }

        public void ThrowErrors()
        {
  
        }
    }
    internal static class Notifier
    {
        private static IGraphQLClient _client;

        private static string mdText = @"## jakiś przykład";
//   moj jakiś tekst plus poniżej przykłady diagramu z netu
//   * przykład 1
//   ```mermaid
//     graph TD;
//         A-->B;
//         A-->C;
//         B-->D;
//         C-->D;
//   ```
     
     
//   * przykład 2
//   ```mermaid
//   sequenceDiagram
//       participant dotcom
//       participant iframe
//       participant viewscreen
//       dotcom->>iframe: loads html w/ iframe url
//       iframe->>viewscreen: request template
//       viewscreen->>iframe: html & javascript
//       iframe->>dotcom: iframe ready
//       dotcom->>iframe: set mermaid data on iframe
//       iframe->>iframe: render mermaid
//   ```
//";


        internal static async void SendWorkflowGraph(List<IStep> serialized)
        {

            var query = @"
                mutation {
                            pages {
                                create (
                                    content: """ + @"* przykład 1 ```mermaid   graph TD;       A-->B;       A-->C;       B-->D;       C-->D; ```" + @" ""
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


            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://10.1.2.88:5555/graphql");
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.Headers["Authorization"] = @"Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhcGkiOjEsImdycCI6MSwiaWF0IjoxNjYzNDEwOTMxLCJleHAiOjE3NTgwODM3MzEsImF1ZCI6InVybjp3aWtpLmpzIiwiaXNzIjoidXJuOndpa2kuanMifQ.dSzRx2hqkiV6MDFJCKQ4xkCKPXVZkkERbAYFBI8p86NdC57d-Rk7RlhzXjCgjISKR-YaRRJ7Dxdku-xcGwBKtTpjddZRSl7yGMWeXEUnj1fY55AKqRG_N6ZbpR_QWMx0HMzKamMz_Z_0spObBIUEulswabIUzicp_fEVxqIP98OVI-j67_knFBRut6kmO8c3hlksGWHinivNF5GwBF5A8PC_b7wNwV0rwRfXcEGWy7xDQVFlAEJ_FlpBCbEtdXBQL1CE2hjw7ILbzakmCxiem44uqkg4pZ0Eoz4B7cdNSQY1EZT_Sjry0IgHkYSngR5hXwo1SZO3FBXdFreYqQLQjQ";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    query = query,
                    //operationName = "mutation",
                    variables = new List<string>()
                });
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var json = streamReader.ReadToEnd();
                //var data = (JObject)JsonConvert.DeserializeObject(json);
                //if (data["errors"]["faultCode"].Value<int>() != 0)
                //    throw new Exception($"Odpowiedź z błędem: {data["errors"]["faultString"].Value<string>()}");
                //else
                //{
                //    foreach (var result in data["Results"])
                //    {
                        
                //    }
                //}
            }

            return;
        }
    }
}
