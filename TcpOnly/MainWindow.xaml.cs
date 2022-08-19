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
        DawTcpClient dawTcpClient;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dawTcpServer = new DawTcpServer(SERVER_IP, PORT_NO);
            dawTcpServer.Start();
            dawTcpClient = new DawTcpClient(SERVER_IP, PORT_NO, "TestName");
        }



        private void ClientSendButton_Click(object sender, RoutedEventArgs e)
        {
            TestClass testObject = new TestClass() { Val1 = ClientTextBlock.Text, Val2 = 34, Val3 = System.DateTime.Now };
            ObjMessage<TestClass> objMessage = new ObjMessage<TestClass>() { TObject = testObject };
            TxtMessage txtMessage = objMessage.GetTextMessage();
            txtMessage.ClientName = "TestName";
            dawTcpClient.SendTxtMessage(txtMessage, (s) => this.Dispatcher.Invoke(() => ClientResponseTextBlock.Text = s));
        }



        private void ServerSendButton_Click(object sender, RoutedEventArgs e)
        {
            //sending data to clients
            if (dawTcpServer != null)
                dawTcpServer.SendTextData(ServerTextBlock.Text);

            //reading data from clients
            
            if (dawTcpClient.client != null)
            {
                var c = dawTcpClient;
                var res = c.GetData();
                ServerResponseTextBlock.Text = res;
            }
        }

        private void CloseServerButton_Click(object sender, RoutedEventArgs e)
        {
            //dawTcpServer.Stop();
        }

        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            ObjMessage<TestClass> objMessage = new ObjMessage<TestClass>("hello world");
            var res = objMessage.GetTextMessage().GetAsXmlString();
            InputMessage.Text = objMessage.GetTextMessage().Data;
            ResultXMLTextBlock.Text = res;
        }

        private void DeserializeButton_Click(object sender, RoutedEventArgs e)
        {
            var input = ResultXMLTextBlock.Text;
            TxtMessageBox.Text = input;
            Serializer serializer = new Serializer();
            serializer.Deserialize<TxtMessage>(input, out var txtMessage);
            var objMessage = txtMessage.GetObjMessage<TestClass>();
        }
    }
}
