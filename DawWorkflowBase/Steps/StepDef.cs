using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Steps
{
    public class StepDef<TContext> where TContext : IContext
    {
        public Action<TContext> Delegate { get; set; }
    }
}
