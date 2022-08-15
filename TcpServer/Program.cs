using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args)
        {

            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            Console.WriteLine("Listening...");


            Task task = Task.Run(() => NewMethod(ref listener));
            listener.Stop();
            Console.ReadLine();
        }

        private static void NewMethod(ref TcpListener listener)
        {
            listener.Start();
            while (true)
            {
                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataChanged = Encoding.ASCII.GetString(buffer, 0, bytesRead) + " Confirmed";
                Console.WriteLine("Received : " + dataChanged);

                //---write back the text to the client---
                Console.WriteLine("Sending back : " + dataChanged);
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(dataChanged);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                client.Close();
            }
            listener.Stop();

        }
    }
}
