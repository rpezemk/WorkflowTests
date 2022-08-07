using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DawWorkflowBase.WCF;
using DawWorkflowBase.WCF.MessageTypes;
namespace TestSimple
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendSomeDataButton_Click(object sender, RoutedEventArgs e)
        {
            DawWCFServiceRef.WorkflowTalkServiceClient workflowTalkServiceClient = new DawWCFServiceRef.WorkflowTalkServiceClient();
            workflowTalkServiceClient.ClearHostMessages();
            for(int i = 0; i < 10; i++)
            {
                workflowTalkServiceClient.PutHostMessage(new DawWCFServiceRef.MyMessage());
            }
            workflowTalkServiceClient.Close();
        }

        private void GetMessages_Click(object sender, RoutedEventArgs e)
        {
            DawWCFServiceRef.WorkflowTalkServiceClient workflowTalkServiceClient = new DawWCFServiceRef.WorkflowTalkServiceClient();
            var queue = workflowTalkServiceClient.GetViewerQueue();
            var res = new List<string>();
            foreach(var m in queue)
            {
                res.Add(m.ToString());
            }
            MyTextBlock.Text = string.Join("\n", res);
            workflowTalkServiceClient.ClearViewerMessages();
            workflowTalkServiceClient.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
