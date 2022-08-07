using System;
using System.Runtime.Serialization;

namespace DawWorkflowBase.WCF
{
    [DataContract]
    public class AMessage
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime CreateDT { get; set; } = DateTime.Now;
        [DataMember]
        public DateTime? LastTryDT { get; set; } = null;
        [DataMember]
        public DateTime? FinishedDT { get; set; } = null;
        [DataMember]
        public MessageStatus Status { get; set; } = MessageStatus.Created;
    }
}
