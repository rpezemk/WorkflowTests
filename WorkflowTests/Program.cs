using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTests
{
    class Program
    {
        static void Main(string[] args) 
        {
            DocWorkflowHost host = new DocWorkflowHost();
            host.RegisterWorkflow<DocProcessWorkflow<Doc>, Doc>();
        }

    }
}
