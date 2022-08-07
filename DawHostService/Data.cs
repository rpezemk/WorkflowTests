using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DawWorkflowBase.WCF;
using DawWorkflowBase.WCF.MessageTypes;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace DawWCFService
{
    
    public static class Data
    {
        [DataMember]
        public static List<MyMessage> HostMessages { get; set; } = new List<MyMessage>();
        [DataMember]
        public static List<MyMessage> ViewerMessages { get; set; } = new List<MyMessage>();
    }
}