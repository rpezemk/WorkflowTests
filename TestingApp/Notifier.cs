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

            ReplacePage(resMermaid);

        }



        internal static async void ReplacePage(string content)
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

            var nextId = 77777;// pages.Select(jt => Convert.ToInt32(jt["id"])).LastOrDefault();

            var queryCreate = @"mutation {
                            pages {
                                create (
                                    content: """ + exactContent + @" ""
                                    description: ""test3""
                                    editor: ""markdown""
                                    isPublished: true
                                    isPrivate: false
                                    locale: ""pl""
                                    path: ""/home/" + godmodePrefix  + nextId.ToString() + @"""
                                    tags:[""#someTag""]
                                    title: """ + godmodePrefix + nextId.ToString() + @"""
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
