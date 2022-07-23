using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Visitors;

namespace DawWorkflowBase.Steps
{ 
    public abstract class AStep<TContext, TResult> :  IStep, Visitors.IVisitable where TContext: IContext
    {
        public List<FlowBind<Condition<TContext>, IStep>> BindList = new List<FlowBind<Condition<TContext>, IStep>>();
        public Guid Guid { get; set; }
        public AStep()
        {
            ChildSteps = new List<IStep>();
            Guid = Guid.NewGuid();
        }

        //public abstract void SetHandler();

        public Func<TContext, TResult> Delegate { get; set; }

        private bool done = false;
        public string Name { get  ; set ; }
        public List<IStep> ChildSteps { get; set; }
        public TContext InputContext { get; set; }
        

        public void RegisterChildStep(IStep step)
        {
            if(step != null)
            {
                ChildSteps.Add(step);
            }
        }



        public void AcceptContext(IContext parentContext)
        {
            InputContext = (TContext)parentContext;
        }



        public virtual void RunDecideAndGo(IContext context)
        {
            throw new NotImplementedException();
        }

        public void Accept(AStepVisitor visitor)
        {
            visitor.Visit(this);
        }

        public List<IStep> GetChildren()
        {
            return BindList.Select(bl => bl.Step).ToList();
        }

        public string GetName()
        {
            return Name;
        }
    }

}
