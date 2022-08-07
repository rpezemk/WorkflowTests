using DawWorkflowBase.Conditions;
using DawWorkflowBase.Steps;
//using DawWorkflowBase.Visitors;
using DawWorkflowBase.Workflow;
using DawWorkflowBase.Links;
using DawWorkflowBase.Creators;
using DawWorkflowBase.Extensions;
using DawWorkflowBase.Context;
using System;

namespace DawLogicLibrary
{
    public class SampleWorkflow : AWorkflowBase<SampleContext>
    {
        public SampleWorkflow(SampleContext context) : base(context)
        {

        }
        public void SetForkflow2()
        {
            var context = new DawLogicLibrary.SampleContext();
            var initStepDef = new StepDef<SampleContext>(_context => { }, "Root");
            var step1Def = new StepDef<SampleContext>(b => { }, "branch 1"); ;
            var root = new ChoiceNode<SampleContext>(initStepDef);
            var condAlwaysTrue = new Condition<SampleContext>(b => true, "Zawsze");
            this.SetContext(context);

            this.SetRoot(root)
                .AppendSteps(
                (condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod1)).AppendSteps(
                    (condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod2)).AppendSteps(
                        (condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod3)).GetNode(out var common))
                    ,(condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod4)).AppendSteps((condAlwaysTrue, common))
                    )
                ,(condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod5)).AppendSteps(
                    (condAlwaysTrue, common))
                );

        }

        
        public override void SetForkflow()
        {
            var context = new DawLogicLibrary.SampleContext();
            var context2 = new DawLogicLibrary.AnotherContext();
            context.Doc.GidType = 2001;

            var condAlwaysTrue = new Condition<SampleContext>(b => true, "Zawsze");
            var condNeverTrue = new Condition<SampleContext>(b => true, "Nigdy");
            var condDoc2001 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2001, "Paragon");
            var condDoc2036 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2036, "Faktura krajowa");
            var condDoc2037 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2037, "Faktura exportowa");
            var condDocIsNetto = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.NB == DocModels.Primitives.NB.N, "Dokument netto");

            var condContext2Test = new Condition<DawLogicLibrary.AnotherContext>(b => true, "Context 2 always true");


            var condZamZewn = new Condition<DawLogicLibrary.SampleContext>(b => true, "Zamówienie wewnętrzne");
            var condWiecejLiniiZamNizDoc = new Condition<DawLogicLibrary.SampleContext>(b => b.Zam.Rows.Count > b.Doc.Rows.Count, "Nadwyżka linii zam");

            //StepDef<DawWorkflowContext>


            var initStepDef = new StepDef<SampleContext>(SteppableMethods.SteppableMethod1, "Root");
            var step1Def = new StepDef<SampleContext>(SteppableMethods.SteppableMethod2, "branch 1"); ;
            var step2Def = new StepDef<SampleContext>(SteppableMethods.SteppableMethod3, "branch 2");
            var step23Def = new StepDef<SampleContext>(b => { }, "branch 23");
            var otherContextStepDef = new StepDef<AnotherContext>(SteppableMethods.SteppableMethod9, "Other Context branch");
            var commonNode1Def = new StepDef<SampleContext>(b => { }, "Common node");

            var link11 = new LinkDef<SampleContext, SampleContext>(condAlwaysTrue, SteppableMethods.ConverterMethod3);
            var link111 = new LinkDef<SampleContext, SampleContext>(condAlwaysTrue, SteppableMethods.ConverterMethod3);
            var link12 = new LinkDef<SampleContext, SampleContext>(condDoc2037, SteppableMethods.ConverterMethod3);
            var link121 = new LinkDef<SampleContext, SampleContext>(condDoc2037, SteppableMethods.ConverterMethod3);

            var root = new ChoiceNode<SampleContext>(initStepDef);
            var child1 = new ChoiceNode<SampleContext>(step1Def);
            var child2 = new ChoiceNode<SampleContext>(step1Def);
            root.AddLink(link11, child1);
            root.AddLink(link12, child2);


            var child23 = new ChoiceNode<SampleContext>(step23Def);
            child2.AddLink(link121, child23);

            var commonStep = new ChoiceNode<SampleContext>(commonNode1Def);
            child1.AddLink(link121, commonStep);
            child23.AddLink(link111, commonStep);
            Creator creator = new Creator();
            var endStep = creator.GenerateStep(typeof(ChoiceNode<>).Name, typeof(SampleContext).Name);
            RootStep = root;
        }


    }

}
