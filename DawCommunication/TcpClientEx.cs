using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DawCommunication
{
    public class TcpClientEx : TcpClient
    {
        public TcpClientEx(string hostname, int port) : base(hostname, port)
        {

        }

        

        public new bool Active()
        {
            return base.Active;
        }
    }
}
