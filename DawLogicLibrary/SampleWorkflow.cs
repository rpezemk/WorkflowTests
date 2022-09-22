using DawWorkflowBase.Conditions;
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
            ChoiceNode<SampleContext> A0  = new ChoiceNode<SampleContext>() { Name = "A0", MyAction = (a) => Console.WriteLine("A0") };
            ChoiceNode<SampleContext> A1  = new ChoiceNode<SampleContext>() { Name = "A1", MyAction = (a) => Console.WriteLine("A1") };
            ChoiceNode<SampleContext> A21 = new ChoiceNode<SampleContext>() { Name = "A21", MyAction = (a) => Console.WriteLine("A21") };
            ChoiceNode<SampleContext> A22 = new ChoiceNode<SampleContext>() { Name = "A22", MyAction = (a) => Console.WriteLine("A22") };
            ChoiceNode<SampleContext> A32 = new ChoiceNode<SampleContext>() { Name = "A32", MyAction = (a) => Console.WriteLine("A32") };
            ChoiceNode<SampleContext> A31 = new ChoiceNode<SampleContext>() { Name = "A31", MyAction = (a) => Console.WriteLine("31") };
            ChoiceNode<SampleContext> A41 = new ChoiceNode<SampleContext>() { Name = "A41", MyAction = (a) => Console.WriteLine("41") };
            ChoiceNode<SampleContext> A51 = new ChoiceNode<SampleContext>() { Name = "A51", MyAction = (a) => Console.WriteLine("51") };
            ChoiceNode<SampleContext> AE  = new ChoiceNode<SampleContext>() { Name = "AE", MyAction = (a) => Console.WriteLine("AE") };



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

            A0.SetNext(a => true, "jeśli coś", A1);
            A1.SetNext(a => true, "jeśli nie", A21);
            A1.SetNext(a => true, "jeśli coś tam",  A22);
            
            A21.SetNext(a => true,"jakiś warunek", A32);
            A22.SetNext(a => true, "warunek na 32", A32);

            A31.SetNext(a => true, "w. na 41", A41);
            A0.SetNext(a => true, "jeśli nie na 41", A31);

            A32.SetNext(a => true, "zawsze (?)", A41);
            A41.SetNext(a => true, A51);
            //A51.SetNext(a => true, TerminatorSC);

            RootStep = A0;

        }


    }

}
