using DawWorkflowBase.WCF;
using DawWorkflowBase.WCF.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DawWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WorkflowTalkService : IWorkflowTalkService
    {
        public List<MyMessage> GetViewerQueue()
        {
            Log.WriteLogEntry("GetViewerQueue");
            return Data.ViewerMessages;
        }

        public List<MyMessage> GetWorkflowHostQueue()
        {
            Log.WriteLogEntry("GetWorkflowHostQueue");
            return Data.HostMessages;
        }

        public bool RemoveMessageFromHostQueue(MyMessage message)
        {
            Log.WriteLogEntry("RemoveMessageFromHostQueue");
            var contains = Data.HostMessages.Where(m => m.Guid == message.Guid).Any();
            if (contains)
            {
                Data.HostMessages.Remove(message);
            }
            return contains;
        }

        public bool RemoveMessageFromViewerQueue(MyMessage message)
        {
            Log.WriteLogEntry("RemoveMessageFromViewerQueue");
            var contains = Data.ViewerMessages.Where(m => m.Guid == message.Guid).Any();
            if (contains)
            {
                Data.HostMessages.Remove(message);
            }
            return contains;
        }

        public void PutHostMessage(MyMessage message)
        {
            Log.WriteLogEntry("PutHostMessage");
            Data.HostMessages.Add(message);
        }
         
        public void PutViewerMessage(MyMessage message)
        {
            Log.WriteLogEntry("PutViewerMessage");
            Data.ViewerMessages.Add(message);
        }

        public void ClearViewerMessages()
        {
            Log.WriteLogEntry("ClearViewerMessages");
            Data.ViewerMessages.Clear();
        }

        public void ClearHostMessages()
        {
            Log.WriteLogEntry("ClearHostMessages");
            Data.HostMessages.Clear();
        }
    }


}
