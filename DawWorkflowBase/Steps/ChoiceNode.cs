﻿using DawWorkflowBase.Conditions;
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

        public ChoiceNode(Action<TContext> action)
        {
            MyAction = action;
        }



        public void SetNext(Condition<TContext> condition, Steps.AStep<TContext> outStep) 
        {
            Links.LinkInstance<TContext> linkInstance = new Links.LinkInstance<TContext>(condition, outStep);
            ResultLinks.Add(linkInstance);
        }

        public void SetNext(Func<TContext, bool> func, Steps.AStep<TContext> outStep)
        {
            var condition = new Condition<TContext>(func, "autogenerated");
            Links.LinkInstance<TContext> linkInstance = new Links.LinkInstance<TContext>(condition, outStep);
            ResultLinks.Add(linkInstance);
        }

    }
}
