using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Lists;
using DawWorkflowBase.WorkflowResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Workflow
{
    public abstract class AWorkflowBase<TContext>  where TContext: IContext // : IWorkflow<TContext>
    {
        public virtual TContext Context { get ; set; }
        public virtual AStep<TContext> RootStep { get ; set; }
        public List<IStep> SerializedSteps { get; set; }

        public virtual void SetContext(TContext context)
        {
            Context = context;
        }

        public abstract void SerializeRoot();


        public abstract void SetForkflow();


    }
}
