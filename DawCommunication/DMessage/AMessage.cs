using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DawCommunication
{

    [Serializable]
    public abstract class AMessage<T> : TxtMessage
    {
        public AMessage() { }
        public AMessage(string type, string info)
        {
            Type = typeof(T).GetType().Name;
            Info = info;
            CreateDT = DateTime.Now;
        }
        public T TObject { get; set; }
        public abstract TxtMessage GetTextMessage();
        public abstract T GetObject();

    }

}
