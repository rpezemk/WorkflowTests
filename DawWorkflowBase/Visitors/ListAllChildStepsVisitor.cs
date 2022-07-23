using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;

namespace DawWorkflowBase.Visitors
{
    public class ListAllChildStepsVisitor : AStepVisitor
    {
        public override void Visit(IVisitable element)
        {
            var children = element.GetChildren();
            foreach (var ch in children)
            {
                VisitedSteps.Add(ch);
            }

            foreach (var ch in children)
            {
                (ch as IVisitable).Accept(this);
            }
        }
    }




    public abstract class AStepVisitor
    {
        public List<Steps.IStep> VisitedSteps { get; set; } = new List<IStep>();
        public abstract void Visit(IVisitable step);
    }



    public interface IVisitable
    {
        void Accept(Visitors.AStepVisitor visitor);
        List<IStep> GetChildren();
    }

}
