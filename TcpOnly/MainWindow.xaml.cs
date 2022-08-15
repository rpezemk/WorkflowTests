using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DawCommunication;

namespace TcpOnly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        DawTcpServer dawTcpServer;
        List<DawTcpClient> dawTcpClients = new List<DawTcpClient>();

        public MainWindow()
        {
            InitializeComponent();
        }



        private void ClientSendButton_Click(object sender, RoutedEventArgs e)
        {
            DawTcpClient dawTcpClient = new DawTcpClient(SERVER_IP, PORT_NO);
            dawTcpClient.SendDataAndReceiveAsync(ClientTextBlock.Text, (s) => this.Dispatcher.Invoke(() => ClientResponseTextBlock.Text = s));
            dawTcpClients.Add(dawTcpClient);
        }



        private void ServerSendButton_Click(object sender, RoutedEventArgs e)
        {
            //sending data to clients
            if (dawTcpServer != null)
                dawTcpServer.SendTextData(ServerTextBlock.Text);
            
            //reading data from clients
            foreach(var dawTcpClient in dawTcpClients.Where(c => c.client != null).ToList())
            {
                var c = dawTcpClient;
                var res = c.GetData();
                ServerResponseTextBlock.Text = res;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dawTcpServer = new DawTcpServer(SERVER_IP, PORT_NO);
            dawTcpServer.Start();
        }

        private void CloseServerButton_Click(object sender, RoutedEventArgs e)
        {
            dawTcpServer.Stop();
        }
    }
}
