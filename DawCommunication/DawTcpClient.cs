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
    public class DawTcpClient
    {
        public string Name { get; set; }
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public TcpClient client { get; set; }

        public DawTcpClient(string serverIP, int port, string name)
        {
            ServerIP = serverIP;
            Port = port;
            Name = name;
        }

        public void Close()
        {
            if (client != null)
            {
                client.Close();
            }
        }

        public void Open()
        {
            client = new TcpClient(ServerIP, Port);
        }


        public void SendDataAndReceiveAsync(string textToSend, Action<string> action)
        {
            if (string.IsNullOrEmpty(textToSend))
                return;
            Task task = Task.Run
                (
                    () =>
                    {
                        string messageResult = "";
                        client = new TcpClient(ServerIP, Port);
                        NetworkStream stream = client.GetStream();
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                        stream.Write(bytesToSend, 0, bytesToSend.Length);
                        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                        int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                        messageResult = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                        if (action != null)
                            action.Invoke(messageResult);
                    }
                );

        }

        public void SendTxtMessage(TxtMessage txtMessage, Action<string> action)
        {
            var xmlData = txtMessage.GetAsXmlString();
            SendDataAndReceiveAsync(xmlData, action);
        }



        public string GetData()
        {
            if (client == null)
                return string.Empty;
            if (client.Connected == false)
                return string.Empty;

            var c = client;
            var len = c.Available;
            NetworkStream reader = c.GetStream();
            byte[] bytes = new byte[len];

            reader.Read(bytes, 0, bytes.Length);
            var res = Encoding.ASCII.GetString(bytes);
            return res;
        }



    }
}
