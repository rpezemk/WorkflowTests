using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Conditions;
using DawWorkflowBase.WorkflowResult;
using DawWorkflowBase.Creators;
using DawWorkflowDemo.TestWorkflow;
using DawWorkflowBase.Workers;

namespace DawWorkflowDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            var context = new TestWorkflow.DawContext();

            MyWorkflow myWorkflow = new MyWorkflow(context);
            myWorkflow.SetContext(context);
            myWorkflow.SetForkflow();

            var worker = new DawWorkflowBase.Workers.Worker<DawWorkflowDemo.TestWorkflow.DawContext>(myWorkflow);
            //creator.GenerateStep(typeof(SequenceNode<DawWorkflowContext>), "Generated step");
            
            worker.RunWorkflow();

            //Console.ReadLine();
        }

        private static void MyAction(IContext c)
        {
            var context = (DawContext)c;
            context.Doc = new DocModels.Doc();
        }
    }
}
