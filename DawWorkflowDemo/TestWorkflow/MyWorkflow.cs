using DawWorkflowBase.Conditions;
using DawWorkflowBase.Steps;
//using DawWorkflowBase.Visitors;
using DawWorkflowBase.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Links;

namespace DawWorkflowDemo.TestWorkflow
{
    class MyWorkflow : AWorkflowBase<DawWorkflowContext>
    {
        public override void SerializeRoot()
        {
            throw new NotImplementedException();
        }

        public override void SetForkflow()
        {
            var context = new TestWorkflow.DawWorkflowContext();
            var context2 = new TestWorkflow.SomeOtherContext();
            context.Doc.GidType = 2001;
            context.Zam.Rodzaj = 4;

            var condAlwaysTrue = new Condition<DawWorkflowContext>(b => true, "Zawsze");
            var condNeverTrue = new Condition<DawWorkflowContext>(b => true, "Nigdy");
            var condDoc2001 = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.GidType == 2001, "Paragon");
            var condDoc2036 = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.GidType == 2036, "Faktura krajowa");
            var condDoc2037 = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.GidType == 2037, "Faktura exportowa");
            var condDocIsNetto = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.NB == DocModels.Primitives.NB.N, "Dokument netto");

            var condContext2Test = new Condition<TestWorkflow.SomeOtherContext>(b => true, "Context 2 always true");


            var condZamZewn = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Zam.Rodzaj == 4, "Zamówienie wewnętrzne");
            var condWiecejLiniiZamNizDoc = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Zam.Rows.Count > b.Doc.Rows.Count, "Nadwyżka linii zam");

            var rootStep = new ChoiceNode<DawWorkflowContext>( _context => { }, "Root");
            var branch1 = new ChoiceNode<DawWorkflowContext>(b => { }, "branch 1");;
            var branch2 = new ChoiceNode<DawWorkflowContext>(b => { }, "branch 2");
            var branchWithOtherContext = new ChoiceNode<SomeOtherContext>(b => { }, "Other Context branch");
            var branchWithOtherContext = new ChoiceNode<SomeOtherContext>(b => { }, "Other Context branch");
            
            

            var commonNode1 = new ChoiceNode<DawWorkflowContext>(b => { }, "Common node");


            var link11  = new Link<DawWorkflowContext>(condAlwaysTrue, branch1);
            var link12  = new Link<DawWorkflowContext, SomeOtherContext>(condDoc2037, branchWithOtherContext);

            //rootStep.AddLink(link11);
            //rootStep.AddLink(link12);

            var link121 = new Link<SomeOtherContext, DawWorkflowContext>(condContext2Test, commonNode1);
            var link21 = new Link<DawWorkflowContext>(condAlwaysTrue, commonNode1);


            //commonNode1.AddLink()
            //rootStep.AddLink(condDoc2036.OR(condDoc2037), branch2);
            //branch1.AddLink(condAlwaysTrue, commonNode1);
            //branch2.AddLink(condAlwaysTrue, commonNode1);

            RootStep = rootStep;
        }
    }

}
