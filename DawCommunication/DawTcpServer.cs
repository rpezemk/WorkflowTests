﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DawCommunication
{
    public class DawTcpServer
    {
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public bool ClosingPending { get; set; } = false;
        private TcpListenerEx tcpListener;
        private List<DawTcpClient> clients = new List<DawTcpClient>();
        Task task;
        public DawTcpServer(string serverIP, int port)
        {
            ServerIP = serverIP;
            Port = port;
        }

        public bool Start()
        {
            bool serverResult = true;
            try
            {
                IPAddress localAdd = IPAddress.Parse(ServerIP);
                tcpListener = new TcpListenerEx(localAdd, Port);
                task = Task.Run(() => BackgroundTaskMethod(ref tcpListener));
            }
            catch
            {
                serverResult = false;
            }
            return serverResult;
        }

        public void Stop()
        {
            ClosingPending = true;
            task.Wait(1000);
            if (tcpListener == null || !tcpListener.Active)
                return;
            tcpListener.Stop();
        }

        public void SendTextData(string message)
        {
            DawTcpClient[] cliArray = new DawTcpClient[clients.Count];
            clients.CopyTo(cliArray);

            foreach (var c in cliArray)
            {
                if(c != null && c.client != null)
                {
                    NetworkStream nwStream = c.client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                else
                {
                    clients.Remove(c);
                }
            }

        }

        private void BackgroundTaskMethod(ref TcpListenerEx listener)
        {
            if (listener.Active == false)
                listener.Start();

            while (ClosingPending == false)
            {
                try
                {
                    DawTcpClient dawClient = new DawTcpClient(ServerIP, Port) { client = listener.AcceptTcpClient() };
                    NetworkStream nwStream = dawClient.client.GetStream();
                    byte[] buffer = new byte[dawClient.client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(buffer, 0, dawClient.client.ReceiveBufferSize);
                    string dataChanged = Encoding.ASCII.GetString(buffer, 0, bytesRead) + " Confirmed";
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(dataChanged);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    clients.Add(dawClient);
                }
                catch
                {

                }

            }
        }
    }

}
