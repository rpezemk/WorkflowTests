using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Links;
using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Steps
{ 
    public abstract class AStep<TContext> : StepDef<TContext>,  IStep where TContext: IContext
    {
        public List<FlowBind<Condition<TContext>, IStep>> BindList = new List<FlowBind<Condition<TContext>, IStep>>();
        public Guid Guid { get; set; }
        public AStep()
        {
            ResultLinks = new List<Links.ILinkInstance>();
            Guid = Guid.NewGuid();
        }

        //public abstract void SetHandler();



        private bool done = false;
        public string Name { get  ; set ; }
        public List<Links.ILinkInstance> ResultLinks { get; set; } = new List<ILinkInstance>();
        public TContext InputContext { get; set; }
        
        public void AcceptContext(IContext parentContext)
        {
            InputContext = (TContext)parentContext;
        }



        public virtual void RunDecideAndGo(IContext context)
        {
            throw new NotImplementedException();
        }


        public List<IStep> GetChildren()
        {
            return BindList.Select(bl => bl.Step).ToList();
        }

        public string GetName()
        {
            return Name;
        }

        public List<ILinkInstance> GetLinks()
        {
            return ResultLinks;
        }
    }

}
