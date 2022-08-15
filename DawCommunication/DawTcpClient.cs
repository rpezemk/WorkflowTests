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
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public DawTcpClient(string serverIP, int port)
        {
            ServerIP = serverIP;
            Port = port;
        }

        public TcpClient client { get; set; }
        public string SendData(string textToSend)
        {
            string messageResult = "";
            client = new TcpClientEx(ServerIP, Port);
            NetworkStream stream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            stream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            messageResult = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            //client.Close();
            return messageResult;
        }

        public string GetData()
        {
            var c = client;
            var len = c.Available;
            NetworkStream reader = c.GetStream();
            byte[] bytes = new byte[len];

            reader.Read(bytes, 0, bytes.Length);
            var res = Encoding.ASCII.GetString(bytes);
            return res;
        }

        public void SendDataAndReceiveAsync(string textToSend, Action<string> action)
        {
            Task task = Task.Run(() => 
            {
                try
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
                catch (ArgumentNullException e)
                {

                }
                catch (SocketException e)
                {

                }
                catch (Exception ex)
                {

                }
            });
        }

    }
}
