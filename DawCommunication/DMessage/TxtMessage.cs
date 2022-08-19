using System;
namespace DawCommunication
{
    [Serializable]
    public class TxtMessage
    {
        public string Type { get; set; }
        public string Info { get; set; }
        public string Data { get; set; }
        public DateTime CreateDT { get; set; } = DateTime.Now;
        public DateTime SendDT { get; set; }
        public DateTime ReceiveDT { get; set; }
        public DateTime FininshedDT { get; set; }
        public string ClientName { get; set; } = "";
        public string GetAsXmlString()
        {
            Serializer serializer = new Serializer();
            var xmlText = serializer.Serialize(this);
            return xmlText;
        }

        public T GetInnerInstance<T>() where T : class, new()
        {
            var serializer = new Serializer();
            serializer.Deserialize<T>(Data, out var instance);
            return instance;
        }

        public ObjMessage<T> GetObjMessage<T>() where T : class, new()
        {
            var serializer = new Serializer();
            serializer.Deserialize<T>(Data, out var instance);
            return new ObjMessage<T>() { TObject = instance };
        }
    }

}
