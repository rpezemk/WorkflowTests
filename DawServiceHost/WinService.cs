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
        localhost.WorkflowTalkService service;
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
            //if (service.State != System.ServiceModel.CommunicationState.Closed)
            //    service.Close();
            CancellationPending = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteLogEntry("bkgnd worker started");
            service = new localhost.WorkflowTalkService();
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
            var queue = service.GetWorkflowHostQueue();
            Log.WriteLogEntry(queue.GetType().Name.ToString());
            foreach(var m in queue)
            {
                service.PutViewerMessage(m);
                service.RemoveMessageFromHostQueue(new localhost.MyMessage(), out var r1, out var r2);
                Log.WriteLogEntry("message sent to viewer queue");
            }
            service.ClearHostMessages();

            Log.WriteLogEntry("ProcessQueue end");
        }
    }
}
