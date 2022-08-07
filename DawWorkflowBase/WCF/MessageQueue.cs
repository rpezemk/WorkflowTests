using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Extensions;
using DawWorkflowBase.WCF.MessageTypes;

namespace DawWorkflowBase.WCF
{

    public class MessageQueue
    {
        public List<AMessage> Messages { get; set; } = new List<AMessage>();
        public bool Pop(out AMessage aMessage)
        {
            aMessage = new NotAMessage();
            if (Messages == null)
                return false;

            var potMsgs = Messages.OrderBy(m => m.CreateDT).Where(m => m.Status.In(MessageStatus.Created, MessageStatus.Error)).ToList();

            if (potMsgs.Count == 0)
                return false;

            aMessage = potMsgs.Last();

            return true;
        }

        public void Put(AMessage aMessage)
        {
            if (Messages == null)
                Messages = new List<AMessage>();
            Messages.Add(aMessage);
        }
    }
}
