using DawWorkflowBase.Conditions;
using DawWorkflowBase.Steps;
//using DawWorkflowBase.Visitors;
using DawWorkflowBase.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            context.Doc.GidType = 2001;
            context.Zam.Rodzaj = 4;

            var condAlwaysTrue = new Condition<DawWorkflowContext>(b => true, "Zawsze");
            var condNeverTrue = new Condition<DawWorkflowContext>(b => true, "Nigdy");
            var condDoc2001 = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.GidType == 2001, "Paragon");
            var condDoc2036 = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.GidType == 2036, "Faktura krajowa");
            var condDoc2037 = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.GidType == 2037, "Faktura exportowa");
            var condDocIsNetto = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Doc.NB == DocModels.Primitives.NB.N, "Dokument netto");

            var condZamZewn = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Zam.Rodzaj == 4, "Zamówienie wewnętrzne");
            var condWiecejLiniiZamNizDoc = new Condition<TestWorkflow.DawWorkflowContext>(b => b.Zam.Rows.Count > b.Doc.Rows.Count, "Nadwyżka linii zam");



            //var a0 = condDoc2001.Evaluate(context);
            //var a1 = condDoc2001.NOT().Evaluate(context);

            var rootStep = new ChoiceNode<DawWorkflowContext, int>("Root");
            rootStep.Delegate = _context =>
            {
                if (_context == null)
                    _context = new TestWorkflow.DawWorkflowContext();
                _context.Doc = SomeApi.API.GetDoc("13/03/ABC");
                _context.Doc.GidType = 2001;
                _context.Zam.GidType = 2001;
                return 0;
            };

            var branch1 = new ChoiceNode<DawWorkflowContext, int>("branch 1");

            branch1.Delegate = b =>
            {
                if (b.Doc.NB == DocModels.Primitives.NB.N)
                    return 0;
                return 1;
            };

            var branch2 = new ChoiceNode<DawWorkflowContext, int>("branch 2");
            branch2.Delegate = b =>
            {
                if (b.Doc.NB == DocModels.Primitives.NB.N)
                    return 0;
                return 1;
            };

            var commonNode1 = new ChoiceNode<DawWorkflowContext, int>("Common node");
            commonNode1.Delegate = b =>
            {
                b.Doc.GidType = 9999;
                b.Zam.GidType = 9999;
                if (b.Doc.NB == DocModels.Primitives.NB.N)
                    return 0;
                return 1;
            };



            rootStep.AddChildNode(condDoc2001, branch1);
            rootStep.AddChildNode(condDoc2036.OR(condDoc2037), branch2);
            branch1.AddChildNode(condAlwaysTrue, commonNode1);
            branch2.AddChildNode(condAlwaysTrue, commonNode1);
            rootStep.AddPassNode(commonNode1);

            RootStep = rootStep;
        }
    }

}
