using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Visitors;

namespace DawWorkflowBase.Steps
{
    public class Step<TContext, TResult> : AStep<TContext, TResult> where TContext : IContext
    {

    }
}
