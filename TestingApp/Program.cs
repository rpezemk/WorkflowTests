using DawLogicLibrary;
using DawLogicLibrary.DocModels;
using DawLogicLibrary.DocModels.Primitives;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Translators;
using DawWorkflowBase.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleWorkflow sampleWorkflow = new SampleWorkflow(new SampleContext());
            sampleWorkflow.Context.Doc = new Doc();
            sampleWorkflow.SetForkflow();

            var worker = new DawWorkflowBase.Workers.Worker<SampleContext>(sampleWorkflow);
            var serialized = worker.SerializeSteps();
            Notifier.SendWorkflowGraph(serialized);

            worker.RunWorkflow();
            //ChoiceNode<SampleContext> A0 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<SampleContext> A1 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A1") };
            //ChoiceNode<SampleContext> A21 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A21") };
            //ChoiceNode<SampleContext> A22 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A22") };
            //ChoiceNode<SampleContext> A32 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<SampleContext> A31 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<SampleContext> A41 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<SampleContext> A51 = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<SampleContext> AE = new ChoiceNode<SampleContext>() { MyAction = (a) => Console.WriteLine("A0") };

            //Translator<SampleContext, AnotherContext> TranslatorA2B = new Translator<SampleContext, AnotherContext>();

            //ChoiceNode<AnotherContext> B1 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<AnotherContext> B21 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<AnotherContext> B22 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<AnotherContext> B31 = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };
            //ChoiceNode<AnotherContext> BE = new ChoiceNode<AnotherContext>() { MyAction = (a) => Console.WriteLine("A0") };

            //Translator<AnotherContext, SampleContext> TranslatorB2A = new Translator<AnotherContext, SampleContext>();
            //ChoiceNode<SampleContext> TerminatorSC = new ChoiceNode<SampleContext>();

            //SampleContext sampleContext = new SampleContext();
            ////  A0 --> translator<A,B> ------------> B1
            ////  |\                                 /   \
            ////  | \___                            B21  B22     
            ////  |     \                          /   \ /  
            ////  |     A1                       (BE)  B31   
            ////       /  \                             v 
            ////  |  A21  A22 <-- <A,B> rotalsanrt <---/                   
            ////  |  / \  /                    
            ////  |(AE) \/                    
            ////  |     A32                    
            //// A31     |                    
            ////  \----A41     AE                     BE
            ////        |      |                       |
            ////        |      v                       v
            ////        A51--> T <-- <A,B> rotalsanrt /                  
            ////               ^
            ////              (terminator)

            //A0.SetNext(a => a.Doc.GidType == 2001, A1);
            //A1.SetNext(a => a.Doc.Rows.Where(r => r.NB == NB.B).Any(), A21);
            //A1.SetNext(a => a.Doc.Rows.Where(r => r.NB == NB.B).Count() == 0, A22);

            //A21.SetNext(a => true, A32);
            //A22.SetNext(a => true, A32);

            //A31.SetNext(a => true, A41);
            //A0.SetNext(a => true, A31);

            //A32.SetNext(a => true, A41);
            //A41.SetNext(a => true, A51);
            //A51.SetNext(a => true, TerminatorSC);
            //A0.RunStep(sampleContext);

        }
    }
}
