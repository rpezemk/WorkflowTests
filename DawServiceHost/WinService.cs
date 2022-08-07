using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DawLogicLibrary;
using DawWorkflowBase;
using DawWorkflowBase.WCF;
using DawWorkflowBase.WCF.MessageTypes;

namespace DawServiceHost
{
    public partial class WinService : ServiceBase
    {
        AutoResetEvent wait;
        DawWCFServiceRef.WorkflowTalkServiceClient client;
        public bool CancellationPending;
        public WinService()
        {
            InitializeComponent();
            wait = new AutoResetEvent(false);
        }

        protected override void OnStart(string[] args)
        {
            Log.WriteLogEntry("Service started");
            backgroundWorker1.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            Log.WriteLogEntry("Service ended");
            if (client.State != System.ServiceModel.CommunicationState.Closed)
                client.Close();
            CancellationPending = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteLogEntry("bkgnd worker started");
            client = new DawWCFServiceRef.WorkflowTalkServiceClient();
            Log.WriteLogEntry("WorkflowTalkServiceClient instance created");
            while (true)
            {
                Log.WriteLogEntry("iteration");
                ProcessQueue();
                wait.WaitOne(1000);
            } 
        }

        public string SomeMethod(string arg)
        { 
            return "Test response";
        }

        private void ProcessQueue()
        {
            Log.WriteLogEntry("ProcessQueue"); 
            var queue = client.GetWorkflowHostQueue();
            Log.WriteLogEntry(queue.GetType().Name.ToString());
            foreach(var m in queue)
            {
                client.PutViewerMessage(m);
                client.RemoveMessageFromHostQueue(m);
                Log.WriteLogEntry("message sent to viewer queue");
            }
            client.ClearHostMessages();

            Log.WriteLogEntry("ProcessQueue end");
        }
    }
}
