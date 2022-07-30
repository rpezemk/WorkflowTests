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
        public ChoiceNode(Action<TContext> action, string name = "")
        {
            if(action != null)
            {
                Action = action;
            }
            Name = name;
        }

        public Action<TContext> Action;

        //public void AddLink<TResultContext>(Links.Link<TContext> link, Steps.AStep<TResultContext> outStep) where TResultContext : IContext
        //{
        //    Links.LinkInstance<TContext, TContext> linkInstance = new Links.LinkInstance<TContext, TContext>() { InputStep = this, OutputStep = outStep };
        //    ResultLinks.Add(linkInstance);
        //}


        public void AddLink<TResultContext>(Links.Link<TContext, TResultContext> link, Steps.AStep<TResultContext> outStep) where TResultContext : IContext
        {
            Links.LinkInstance<TContext, TResultContext> linkInstance = new Links.LinkInstance<TContext, TResultContext>() { InputStep = this, OutputStep = outStep };
            ResultLinks.Add(linkInstance);
        }

        //public void AddLink<TResultContext>(Condition<TContext> condition, AStep<TResultContext> step) where TResultContext : BaseContext
        //{
        //    BindList.Add(new FlowBind<Condition<TContext>, IStep>() { Arg = condition, Step = step });
        //}


        //public override void RunDecideAndGo(IContext context)
        //{
        //    var context2 = (TContext)context;
        //    Action.Invoke(context2);
        //    bool foundMatch = false;

        //    foreach (var con in BindList)
        //    {
        //        var res = con.Arg.Evaluate((TContext)context);
        //        if (res == true)
        //        {
        //            con.Step.RunDecideAndGo(context);
        //            foundMatch = true;
        //        }
        //    }

        //    if (foundMatch == false)
        //    {
        //        if (PassNode != null)
        //        {
        //            PassNode.RunDecideAndGo(context);
        //        }
        //    }

        //}






    }


}
