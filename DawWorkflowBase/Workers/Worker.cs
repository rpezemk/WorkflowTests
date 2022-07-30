using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Workers
{
    /// <summary>
    /// Class for running Workflow
    /// </summary>
    public class Worker<TContext> where TContext : IContext
    {
        public Workflow.AWorkflowBase<TContext> Workflow;
        public Worker(Workflow.AWorkflowBase<TContext> workflow)
        {

        }
    }
}
