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
    public abstract class AStep<BaseContext,OutputContext> : IStep where BaseContext: IContext where OutputContext: IContext
    {
        public Guid Guid = Guid.NewGuid();
        public bool IsEndPoint { get; set; } = false;
        public AStep()
        {

        }

        private bool done = false;


        public Action<BaseContext> MyAction;

        public string Name { get  ; set ; }
        public List<Links.LinkInstance<BaseContext, OutputContext>> ResultLinks { get; set; } = new List<LinkInstance<BaseContext,OutputContext>>();
        



        public BaseContext StepContext { get; set; }
        
        public void AcceptContext(IContext parentContext)
        {
            StepContext = (BaseContext)parentContext;
        }

        public string GetName()
        {
            return Name;
        }



        public bool CheckIfEndPoint()
        {
            return IsEndPoint;
        }

        public void SetEndPoint(bool isEndPoint)
        {
            IsEndPoint = isEndPoint;
        }



        public IContext GetContext()
        {
            return StepContext;
        }

        public override string ToString()
        {
            return $"Step {Name}, {Guid}";
        }

        List<ILinkInstance> IStep.GetLinks()
        {
            var test = ResultLinks.Select(l => l as ILinkInstance).ToList();
            return test;
        }



        public void RunStep(IContext context)
        {
            if (MyAction == null)
                return;

            MyAction.Invoke((BaseContext)context);
        }
    }

}
