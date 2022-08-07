using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Steps
{

    public class ChoiceNode<TContext> : AStep<TContext> where TContext : IContext
    {



        public IStep PassNode;
        public IStep ErrorNode;
        public string Name { get; set; }

        public ChoiceNode() : base()
        {

        }
        public ChoiceNode(StepDef<TContext> stepDef) : base(stepDef)
        {

        }

        public ChoiceNode(Action<TContext> action)
        {
            Action = action;
        }

        public Action<TContext> Action;

        public void AddLink<TResultContext>(Links.LinkDef<TContext, TResultContext> linkDef, Steps.AStep<TResultContext> outStep) where TResultContext : IContext
        {
            Links.LinkInstance<TContext, TResultContext> linkInstance = new Links.LinkInstance<TContext, TResultContext>(linkDef, this, outStep );
            ResultLinks.Add(linkInstance);
        }

        public void AddLink(Links.LinkDef<TContext, TContext> linkDef, Steps.AStep<TContext> outStep) 
        {
            Links.LinkInstance<TContext, TContext> linkInstance = new Links.LinkInstance<TContext, TContext>(linkDef, this, outStep);
            ResultLinks.Add(linkInstance);
        }


    }


}
