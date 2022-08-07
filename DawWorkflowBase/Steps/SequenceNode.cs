using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DawWorkflowBase.Steps
{
    public class SequenceNode<TContext> : AStep<TContext> where TContext : IContext
    {
        //private ListArgT obj;
        //List<FlowBind<Condition<TContext>, IStep>> BindList = new List<FlowBind<Condition<TContext>, IStep>>();

        public IStep PassNode;
        public string Name { get; set; }



        public SequenceNode(StepDef<TContext> stepDef) : base(stepDef)
        {
            
        }

        public new Func<TContext, TContext> Delegate;

        public void AddChildNode<TResultContext>(Condition<TContext> condition, AStep<TResultContext> step) where TResultContext : IContext
        {
            BindList.Add(new FlowBind<Condition<TContext>, IStep>() { Arg = condition, Step = step });
        }


        //public override void RunDecideAndGo(IContext context)
        //{
        //    var context2 = (TContext)context;
        //    var obj = Delegate.Invoke(context2);
        //    var sel = BindList.Where(b => b.Arg.Equals(obj));
        //    bool foundMatch = false;

        //    if (sel.Count() > 0)
        //    {
        //        var firstSel = sel.First();
        //        if (obj.Equals(firstSel.Arg) && !foundMatch)
        //        {
        //            firstSel.Step.RunDecideAndGo(context);
        //            foundMatch = true;
        //        }
        //    }

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



        public void AddPassNode(AStep<TContext> node)
        {
            PassNode = node;
        }



    }


}
