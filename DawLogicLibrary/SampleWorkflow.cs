﻿using DawWorkflowBase.Conditions;
using DawWorkflowBase.Steps;
//using DawWorkflowBase.Visitors;
using DawWorkflowBase.Workflow;
using DawWorkflowBase.Links;
using DawWorkflowBase.Creators;
using DawWorkflowBase.Extensions;
using DawWorkflowBase.Context;
using System;
using System.Linq;
using DawLogicLibrary.DocModels.Primitives;

namespace DawLogicLibrary
{
    public class SampleWorkflow : AWorkflowBase<SampleContext>
    {
        public SampleWorkflow(SampleContext context) : base(context)
        {

        }
        //public void SetForkflow2()
        //{
        //    var context = new DawLogicLibrary.SampleContext();
        //    var initStepDef = new StepDef<SampleContext>(_context => { }, "Root");
        //    var step1Def = new StepDef<SampleContext>(b => { }, "branch 1"); ;
        //    var root = new ChoiceNode<SampleContext>(initStepDef);
        //    var condAlwaysTrue = new Condition<SampleContext>(b => true, "Zawsze");
        //    this.SetContext(context);

        //    this.SetRoot(root)
        //        .AppendSteps(
        //        (condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod1)).AppendSteps(
        //            (condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod2)).AppendSteps(
        //                (condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod3)).GetNode(out var common))
        //            ,(condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod4)).AppendSteps((condAlwaysTrue, common))
        //            )
        //        ,(condAlwaysTrue, new ChoiceNode<SampleContext>(SteppableMethods.SteppableMethod5)).AppendSteps(
        //            (condAlwaysTrue, common))
        //        );

        //}

        
        public override void SetForkflow()
        {
            ChoiceNode<SampleContext> A0 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };
            ChoiceNode<SampleContext> A1 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A1") };
            ChoiceNode<SampleContext> A21 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A21") };
            ChoiceNode<SampleContext> A22 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A22") };
            ChoiceNode<SampleContext> A32 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A32") };
            ChoiceNode<SampleContext> A31 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("31") };
            ChoiceNode<SampleContext> A41 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("41") };
            ChoiceNode<SampleContext> A51 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("51") };
            ChoiceNode<SampleContext> AE = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("AE") };



            ChoiceNode<AnotherContext> B1 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            ChoiceNode<AnotherContext> B21 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            ChoiceNode<AnotherContext> B22 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            ChoiceNode<AnotherContext> B31 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            ChoiceNode<AnotherContext> BE = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };


            ChoiceNode<SampleContext> TerminatorSC = new ChoiceNode<SampleContext>();

            SampleContext sampleContext = new SampleContext();
            //  A0 --> translator<A,B> ------------> B1
            //  |\                                 /   \
            //  | \___                            B21  B22     
            //  |     \                          /   \ /  
            //  |     A1                       (BE)  B31   
            //       /  \                             v 
            //  |  A21  A22 <-- <A,B> rotalsanrt <---/                   
            //  |  / \  /                    
            //  |(AE) \/                    
            //  |     A32                    
            // A31     |                    
            //  \----A41     AE                     BE
            //        |      |                       |
            //        |      v                       v
            //        A51--> T <-- <A,B> rotalsanrt /                  
            //               ^
            //              (terminator)

            A0.SetNext(a => true, A1);
            A1.SetNext(a => true, A21);
            A1.SetNext(a => true, A22);
            
            A21.SetNext(a => true, A32);
            A22.SetNext(a => true, A32);

            A31.SetNext(a => true, A41);
            A0.SetNext(a => true, A31);

            A32.SetNext(a => true, A41);
            A41.SetNext(a => true, A51);
            //A51.SetNext(a => true, TerminatorSC);

            RootStep = A0;

            //var context = new DawLogicLibrary.SampleContext();
            //var context2 = new DawLogicLibrary.AnotherContext();
            //context.Doc.GidType = 2001;

            //var condAlwaysTrue = new Condition<SampleContext>(b => true, "Zawsze");
            //var condNeverTrue = new Condition<SampleContext>(b => true, "Nigdy");
            //var condDoc2001 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2001, "Jeśli paragon");
            //var condDoc2036 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2036, "Jeśli Faktura krajowa");
            //var condDoc2037 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2037, "Jeśli Faktura exportowa");
            //var condDocIsNetto = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.NB == DocModels.Primitives.NB.N, "Dokument netto");

            //var condContext2Test = new Condition<DawLogicLibrary.AnotherContext>(b => true, "Context 2 always true");


            //var condZamZewn = new Condition<DawLogicLibrary.SampleContext>(b => true, "Zamówienie wewnętrzne");
            //var condWiecejLiniiZamNizDoc = new Condition<DawLogicLibrary.SampleContext>(b => b.Zam.Rows.Count > b.Doc.Rows.Count, "Nadwyżka linii zam");

            ////StepDef<DawWorkflowContext>


            //var initStepDef = new StepDef<SampleContext>(SteppableMethods.SteppableMethod1, "Root");
            //var step1Def = new StepDef<SampleContext>(SteppableMethods.SteppableMethod2, "branch 1"); ;
            //var step2Def = new StepDef<SampleContext>(SteppableMethods.SteppableMethod3, "branch 2");
            //var step23Def = new StepDef<SampleContext>(b => { }, "branch 23");
            //var otherContextStepDef = new StepDef<AnotherContext>(SteppableMethods.SteppableMethod9, "Other Context branch");
            //var commonNode1Def = new StepDef<SampleContext>(b => { }, "Common node");



            ////var root = new ChoiceNode<SampleContext>(initStepDef);
            ////RootStep = root;

            ////var child1 = new ChoiceNode<SampleContext>(step1Def);
            ////var child2 = new ChoiceNode<SampleContext>(step1Def);


            ////root.AddLink(link11, child1);
            ////root.AddLink(link12, child2);


            ////var child23 = new ChoiceNode<SampleContext>(step23Def);
            ////child2.AddLink(link121, child23);

            ////var commonStep = new ChoiceNode<SampleContext>(commonNode1Def);
            ////child1.AddLink(link121, commonStep);
            ////child23.AddLink(link111, commonStep);
            ////Creator creator = new Creator();
            ////var endStep = creator.GenerateStep(typeof(ChoiceNode<>).Name, typeof(SampleContext).Name);
        }


    }

}
