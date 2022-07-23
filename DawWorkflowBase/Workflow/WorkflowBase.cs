using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Lists;
using DawWorkflowBase.Visitors;
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
        public virtual IStep RootStep { get ; set; }
        public List<IStep> SerializedSteps { get; set; }

        public Visitors.ListAllChildStepsVisitor ChildStepsVisitor { get; set; } = new ListAllChildStepsVisitor();

        public List<TwoTuple<Guid, Guid>> Links { get; set; } = new List<TwoTuple<Guid, Guid>>();

        public virtual void SetContext(TContext context)
        {
            Context = context;
        }

        public abstract void SerializeRoot();


        public abstract void SetForkflow();

        public virtual IWorkflowResult RunWorkflow()
        {
            (RootStep as Visitors.IVisitable).Accept(ChildStepsVisitor);
            var list = ChildStepsVisitor.VisitedSteps;
            //ChildStepsVisitor.Visit(RootStep as Visitors.IVisitable);
            return new WorkflowOKResult();
        }
    }
}
