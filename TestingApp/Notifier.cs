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
    
    class GraphLine
    {
        public string Parent = "";
        public string Arrow = "";
        public string Child = "";
        public string ParentContext = "";
        public string ChildContext = "";
    }


    internal static class Notifier
    {




        internal static void NotifySerializedWithContextGroups(List<IStep> inputStepsSeries)
        {
            var serializedSteps = inputStepsSeries;
            List<GraphLine> graphLines = new List<GraphLine>();


            List<string> typeNames = inputStepsSeries.Select(s => s.GetContextTypeName()).Distinct().ToList();

            var resMermaid = "```mermaid\n\rflowchart TB\n\r";
            var termCounter = 1;
            var firstStep = inputStepsSeries.FirstOrDefault();

            var thisName0 = firstStep.GetName();
            var thisLinks0 = firstStep.GetLinks();



            var firstTypeName = typeNames[0];

            foreach (var step in serializedSteps)
            {
                var thisName = step.GetName();
                var thisLinks = step.GetLinks();

                foreach (var ch in thisLinks)
                {
                    var chName = ch.GetResultStep().GetName();
                    var condName = ch.GetCondition().GetName();
                    var arrow = "";
                    if (string.IsNullOrEmpty(chName))
                    {
                        chName = "term" + termCounter.ToString();
                        termCounter++;
                    }

                    arrow = string.IsNullOrEmpty(condName) ? " --> " : $" -- {condName} --> ";
                    GraphLine graphLine = new GraphLine() { Parent = thisName, Child = chName, Arrow = arrow, ParentContext = step.GetContextTypeName(), ChildContext = ch.GetResultStep().GetContextTypeName() };
                    graphLines.Add(graphLine);

                }
            }
            


            foreach(GraphLine gl in graphLines.Where(gl => gl.ParentContext != gl.ChildContext))
            {
                resMermaid += $"{gl.Parent}{gl.Arrow}{gl.Child} \n";
            }

            foreach(var typeName in typeNames)
            {
                resMermaid += $" subgraph {typeName}\n\r";

                foreach(var gl in graphLines.Where(gl => gl.ParentContext == gl.ChildContext && gl.ParentContext == typeName))
                {
                    resMermaid += $"{gl.Parent}{gl.Arrow}{gl.Child} \n";
                }
                resMermaid += $" end\n\r";
            }





            resMermaid += "```\r\n";
            Console.WriteLine(resMermaid);

            ReplaceOrCreatePage(resMermaid);

        }

        internal static void NotifySerialized(List<IStep> serializedSteps)
        {
            var resMermaid = "```mermaid\n\rflowchart TB\n\r";
            var termCounter = 1;
            foreach(var step in serializedSteps)
            {
                var thisName = step.GetName();
                var thisLinks = step.GetLinks();

                foreach(var ch in thisLinks)
                {
                    var chName = ch.GetResultStep().GetName();
                    var condName = ch.GetCondition().GetName();
                    var arrow = "";
                    if (string.IsNullOrEmpty(chName))
                    {
                        chName = "term" + termCounter.ToString();
                        termCounter++;
                    }

                    arrow = string.IsNullOrEmpty(condName) ? " --> " : $" -- {condName} --> ";
                    resMermaid += $"{thisName}{arrow}{chName} \n";
                }
            }

            resMermaid += "```\r\n";
            Console.WriteLine(resMermaid);

            ReplaceOrCreatePage(resMermaid);

        }



        internal static async void ReplaceOrCreatePage(string content)
        {
            var exactContent = content.Replace("\n", @"\n").Replace("\r", @"\r").Replace("(", @"&#40").Replace(")", @"&#41");

            var queryGet = @"query { pages { list { id title } } }";
            if (!SendQGraphQuery($"http://10.1.2.88:5555/graphql", queryGet, out var outData1))
                return;

            var godmodePrefix = "TestStruktury";
            var pages = outData1["data"]["pages"]["list"].ToList();
            var idsToDel = pages.Where(jt => Convert.ToString(jt["title"]).StartsWith(godmodePrefix)).Select(jt => Convert.ToString(jt["id"])).ToList();
            

            foreach(var id in idsToDel)
            {
                var queryDel = @"mutation { pages { delete (id: " + id + "){responseResult {succeeded errorCode slug message}} } } ";
                SendQGraphQuery($"http://10.1.2.88:5555/graphql", queryDel, out var outData2);
            }
            var now = DateTime.Now;
            var suffix = now.ToString("_yyMMdd_hhmm") ;// pages.Select(jt => Convert.ToInt32(jt["id"])).LastOrDefault();
            var queryCreate = @"mutation {
                            pages {
                                create (
                                    content: """ + exactContent + @" ""
                                    description: ""test3""
                                    editor: ""markdown""
                                    isPublished: true
                                    isPrivate: false
                                    locale: ""pl""
                                    path: ""/GodMode/" + godmodePrefix + @"""
                                    tags:[""#someTag""]
                                    title: """ + godmodePrefix + suffix + @"""
                                ) {responseResult { succeeded slug message }
                                   page { content id } } }
                                }";


            Console.WriteLine(queryCreate);
            if (!SendQGraphQuery($"http://10.1.2.88:5555/graphql", queryCreate, out var keyValuePairs))
                return;

            foreach(var kvp in keyValuePairs)
            {
                Console.WriteLine($"{kvp.Key}{kvp.Value.ToString()}");
            }

        }

        public static bool SendQGraphQuery(string url, string query, out JObject resultData)
        {
            resultData = null;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.Headers["Authorization"] = @"Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhcGkiOjEsImdycCI6MSwiaWF0IjoxNjYzNDEwOTMxLCJleHAiOjE3NTgwODM3MzEsImF1ZCI6InVybjp3aWtpLmpzIiwiaXNzIjoidXJuOndpa2kuanMifQ.dSzRx2hqkiV6MDFJCKQ4xkCKPXVZkkERbAYFBI8p86NdC57d-Rk7RlhzXjCgjISKR-YaRRJ7Dxdku-xcGwBKtTpjddZRSl7yGMWeXEUnj1fY55AKqRG_N6ZbpR_QWMx0HMzKamMz_Z_0spObBIUEulswabIUzicp_fEVxqIP98OVI-j67_knFBRut6kmO8c3hlksGWHinivNF5GwBF5A8PC_b7wNwV0rwRfXcEGWy7xDQVFlAEJ_FlpBCbEtdXBQL1CE2hjw7ILbzakmCxiem44uqkg4pZ0Eoz4B7cdNSQY1EZT_Sjry0IgHkYSngR5hXwo1SZO3FBXdFreYqQLQjQ";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json1 = new JavaScriptSerializer().Serialize(new
                {
                    query = query,
                    variables = new List<string>()
                });
                streamWriter.Write(json1);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string json2 = "";


            bool isOK = true;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                try
                {
                    json2 = streamReader.ReadToEnd();
                }
                catch
                {
                    isOK = false;
                }
            }
            resultData = (JObject)JsonConvert.DeserializeObject(json2);
            return isOK;
        }
    }
}
