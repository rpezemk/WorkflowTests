using DawWorkflowBase.Conditions;
using DawWorkflowBase.Steps;
//using DawWorkflowBase.Visitors;
using DawWorkflowBase.Workflow;
using DawWorkflowBase.Links;
using DawWorkflowBase.Creators;
using DawWorkflowBase.Extensions;
using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowDemo.TestWorkflow
{
    class MyContext : IContext
    {

    }
    class MyWorkflow : AWorkflowBase<DawContext>
    {
        public MyWorkflow(DawContext context) : base(context)
        {

        }
        public static void Test()
        {
            SomeMethod(new[] { (1, 2) });
        }


        public static void SomeMethod(params (int, int)[] ps){}
        public static void SomeMethod2(params (int, int)[] ps) {}

        public void SetForkflow2()
        {
            var context = new TestWorkflow.DawContext();
            var initStepDef = new StepDef<DawContext>(_context => { }, "Root");
            var step1Def = new StepDef<DawContext>(b => { }, "branch 1"); ;
            var root = new ChoiceNode<DawContext>(initStepDef);
            var condAlwaysTrue = new Condition<DawContext>(b => true, "Zawsze");
            this.SetContext(context);

            this.SetRoot(root)
                .AppendSteps(new[] { 
                    (condAlwaysTrue, new ChoiceNode<DawContext>())
                        .AppendSteps(
                            (condAlwaysTrue, new ChoiceNode<DawContext>()), 
                            (condAlwaysTrue, new ChoiceNode<DawContext>()),
                    (condAlwaysTrue, new ChoiceNode<DawContext>()).GetNode(out var ssdf)   
            )});
            
        }
        public override void SetForkflow()
        {

        }
    }

}
