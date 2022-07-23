using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DawWorkflowBase.Steps
{
    public class SequenceNode<TContext, ListArgT> : Step<TContext, ListArgT> where TContext : IContext
    {
        private ListArgT obj;
        //List<FlowBind<Condition<TContext>, IStep>> BindList = new List<FlowBind<Condition<TContext>, IStep>>();

        public IStep PassNode;
        public string Name { get; set; }
        public SequenceNode(string name = "")
        {
            Name = name;
        }

        public new Func<TContext, ListArgT> Delegate;

        public void AddChildNode<TResultContext>(Condition<TContext> condition, Step<TResultContext, ListArgT> step) where TResultContext : BaseContext
        {
            BindList.Add(new FlowBind<Condition<TContext>, IStep>() { Arg = condition, Step = step });
        }


        public override void RunDecideAndGo(IContext context)
        {
            var context2 = (TContext)context;
            obj = Delegate.Invoke(context2);
            var sel = BindList.Where(b => b.Arg.Equals(obj));
            bool foundMatch = false;

            if (sel.Count() > 0)
            {
                var firstSel = sel.First();
                if (obj.Equals(firstSel.Arg) && !foundMatch)
                {
                    firstSel.Step.RunDecideAndGo(context);
                    foundMatch = true;
                }
            }

            foreach (var con in BindList)
            {
                var res = con.Arg.Evaluate((TContext)context);
                if (res == true)
                {
                    con.Step.RunDecideAndGo(context);
                    foundMatch = true;
                }
            }

            if (foundMatch == false)
            {
                if (PassNode != null)
                {
                    PassNode.RunDecideAndGo(context);
                }
            }

        }



        public void AddPassNode(Step<TContext, ListArgT> node)
        {
            PassNode = node;
        }



    }


}
