using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DawWorkflowBase.Extensions;
using DawWorkflowBase.WCF;
using DawWorkflowBase.WCF.MessageTypes;

namespace DawWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IWorkflowTalkService
    {

        //[OperationContract]
        //string TestFunc();
        //[OperationContract]
        //void SendMessageToWorkflowHost(Message aMessage);

        //[OperationContract]
        //void SendMessageToViewer(Message aMessage);

        [OperationContract]
        List<MyMessage> GetWorkflowHostQueue();
        [OperationContract]
        List<MyMessage> GetViewerQueue();

        [OperationContract]
        void PutHostMessage(MyMessage message);

        [OperationContract]
        void PutViewerMessage(MyMessage message);

        [OperationContract]
        bool RemoveMessageFromViewerQueue(MyMessage message);

        [OperationContract]
        bool RemoveMessageFromHostQueue(MyMessage message);

        [OperationContract]
        void ClearViewerMessages();

        [OperationContract]
        void ClearHostMessages();
    }

    [DataContract]
    public class MyMessage
    {
        private string name;
        private DateTime createDT;
        [DataMember] public string Type { get => name; set => name = value; }
        [DataMember] public DateTime CreateDt { get => createDT; set => createDT = value; }
        [DataMember] public Guid Guid { get; set; }
    }

}
