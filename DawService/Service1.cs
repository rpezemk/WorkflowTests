using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DawService
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Method1().Wait();
            }
        }

        public static async Task Method1()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Task.Delay(1000).Wait();
                }
            });
        }

    }
}
