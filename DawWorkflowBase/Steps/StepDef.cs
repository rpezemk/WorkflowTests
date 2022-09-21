using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Steps
{
    public class StepDef<TContext> : IStepDef where TContext : IContext
    {
        public StepDef()
        {
            
        }

        public StepDef(Action<TContext> action, string name)
        {
            StepAction = action;
            Name = name;
        }
        public Action<TContext> StepAction { get; set; }
        public String Name { get; set; }

        public string GetName()
        {
            return Name;
        }

        public Type GetStepType()
        {
            return typeof(TContext);
        }

        public void SetAction(object action)
        {
            StepAction = (Action<TContext>)action;
        }
    }
}
