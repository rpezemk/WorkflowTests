using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Links
{
    public interface ILink
    {
        Conditions.ICondition GetCondition();
    }

    public interface ILinkInstance
    {
        bool CheckCondition();
        Conditions.ICondition GetCondition();
        IStep GetInputStep();
        IStep GetResultStep();
    }

    public class LinkInstance<InputContext, ResultContext> : ILinkInstance where InputContext : IContext where ResultContext : IContext
    {
        public AStep<InputContext> InputStep { get; set; }
        public AStep<ResultContext> OutputStep { get; set; }
        public bool CheckCondition()
        {
            throw new NotImplementedException();
        }

        public ICondition GetCondition()
        {
            throw new NotImplementedException();
        }

        public IStep GetInputStep()
        {
            throw new NotImplementedException();
        }

        public IStep GetResultStep()
        {
            throw new NotImplementedException();
        }
    }

    public class Link<TContext> : ILink where TContext : IContext
    {
        public Link(Conditions.Condition<TContext> condition, Steps.AStep<TContext> step)
        {
            Context = step.InputContext;
            Condition = condition;
            Step = step;
        }

        public TContext Context { get; set; }
        public Conditions.Condition<TContext> Condition { get; set; }
        public Steps.AStep<TContext> Step { get; set; }
        public ICondition GetCondition()
        {
            return Condition;
        }

        public void Run()
        {
            if (Context != null && Condition != null && Step != null)
            {
                Step.RunDecideAndGo(Context);
            }
        }
    }




    public class Link<InputContext, ResultContext> : ILink where InputContext : IContext where ResultContext : IContext
    {


        public Link(Conditions.Condition<InputContext> condition, Steps.AStep<ResultContext> step)
        {
            //InContext = condition.;
            OutContext = step.InputContext;
        }

        public InputContext InContext { get; set; }
        public ResultContext OutContext { get; set; }
        public Conditions.Condition<InputContext> Condition { get; set; }
        public Steps.AStep<ResultContext> Step { get; set; }


        public ICondition GetCondition()
        {
            return Condition;
        }

    }


}
