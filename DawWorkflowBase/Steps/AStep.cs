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
    public abstract class AStep<TContext> : IStep where TContext: IContext
    {
        public Guid Guid = Guid.NewGuid();
        public bool IsEndPoint { get; set; } = false;
        public AStep()
        {

        }
        public AStep(StepDef<TContext> stepDef) 
        {
            StepDef = stepDef;
        }
        public StepDef<TContext> StepDef { get; set; }  
        public List<FlowBind<Condition<TContext>, IStep>> BindList = new List<FlowBind<Condition<TContext>, IStep>>();

        //public AStep()
        //{
        //    ResultLinks = new List<Links.ILinkInstance>();
        //    Guid = Guid.NewGuid();
        //}

        //public abstract void SetHandler();



        private bool done = false;


        public string Name { get  ; set ; }
        public List<Links.ILinkInstance> ResultLinks { get; set; } = new List<ILinkInstance>();
        public TContext StepContext { get; set; }
        
        public void AcceptContext(IContext parentContext)
        {
            StepContext = (TContext)parentContext;
        }

        public string GetName()
        {
            return Name;
        }

        public List<ILinkInstance> GetLinks()
        {
            return ResultLinks;
        }

        public bool CheckIfEndPoint()
        {
            return IsEndPoint;
        }

        public void SetEndPoint(bool isEndPoint)
        {
            IsEndPoint = isEndPoint;
        }

        public void RunStep(IContext context)
        {
            StepDef.StepAction((TContext)context);
        }

        public IContext GetContext()
        {
            return StepContext;
        }

        public override string ToString()
        {
            return $"Step {Name}, {Guid}";
        }
    }

}
