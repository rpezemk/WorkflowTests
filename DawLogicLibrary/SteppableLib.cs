using DawWorkflowBase.Steps;
using DawWorkflowBase.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawLogicLibrary
{

    public class SteppableLib
    {
        //public ChoiceNode<SampleContext> A0 = new ChoiceNode<SampleContext>() { Name = "A0" };
        //public ChoiceNode<SampleContext> A1  = new ChoiceNode<SampleContext>() { Name = "A1" };
        //public ChoiceNode<SampleContext> A21 = new ChoiceNode<SampleContext>() { Name = "A21" };
        //public ChoiceNode<SampleContext> A22 = new ChoiceNode<SampleContext>() { Name = "A22" };
        //public ChoiceNode<SampleContext> A32 = new ChoiceNode<SampleContext>() { Name = "A32" };
        //public ChoiceNode<SampleContext> A31 = new ChoiceNode<SampleContext>() { Name = "A31" };
        //public ChoiceNode<SampleContext> A41 = new ChoiceNode<SampleContext>() { Name = "A41" };
        //public ChoiceNode<SampleContext> A51 = new ChoiceNode<SampleContext>() { Name = "A51" };
        //public ChoiceNode<SampleContext> AE = new ChoiceNode<SampleContext>() { Name = "AE" };

        //public Translator<SampleContext, AnotherContext> TranslatorA2B = new Translator<SampleContext, AnotherContext>();

        //public ChoiceNode<AnotherContext> B1  = new ChoiceNode<AnotherContext>();
        //public ChoiceNode<AnotherContext> B21 = new ChoiceNode<AnotherContext>();
        //public ChoiceNode<AnotherContext> B22 = new ChoiceNode<AnotherContext>();
        //public ChoiceNode<AnotherContext> B31 = new ChoiceNode<AnotherContext>();
        //public ChoiceNode<AnotherContext> BE  = new ChoiceNode<AnotherContext>();

        //public Translator<AnotherContext, SampleContext> TranslatorB2A = new Translator<AnotherContext, SampleContext>();

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
        public ChoiceNode<SampleContext> TerminatorSC = new ChoiceNode<SampleContext>();



        public void Init()
        {
            SampleContext sampleContext = new SampleContext();
            ChoiceNode<SampleContext> A0 = new ChoiceNode<SampleContext>() { Name = "A0" };
            ChoiceNode<SampleContext> A1 = new ChoiceNode<SampleContext>() { Name = "A1" };
            ChoiceNode<SampleContext> A21 = new ChoiceNode<SampleContext>() { Name = "A21" };
            ChoiceNode<SampleContext> A22 = new ChoiceNode<SampleContext>() { Name = "A22" };
            ChoiceNode<SampleContext> A32 = new ChoiceNode<SampleContext>() { Name = "A32" };
            ChoiceNode<SampleContext> A31 = new ChoiceNode<SampleContext>() { Name = "A31" };
            ChoiceNode<SampleContext> A41 = new ChoiceNode<SampleContext>() { Name = "A41" };
            ChoiceNode<SampleContext> A51 = new ChoiceNode<SampleContext>() { Name = "A51" };
            ChoiceNode<SampleContext> AE = new ChoiceNode<SampleContext>() { Name = "AE" };


            A0.SetNext(a => a.Doc.GidType == 2001, A1);
            A1.SetNext(a => a.Doc.Rows.Where(r => r.NB == DocModels.Primitives.NB.B).Any(), A21);
            A1.SetNext(a => a.Doc.Rows.Where(r => r.NB == DocModels.Primitives.NB.B).Count() == 0, A22);


            A21.SetNext(a => true, A32);
            A22.SetNext(a => true, A32);

            A31.SetNext(a => true, A41);
            A32.SetNext(a => true, A41);
            A41.SetNext(a => true, A51);
            A51.SetNext(a => true, TerminatorSC);

            A0.RunStep(sampleContext);

            //B1.SetNext(b => b.Zam.Rows.Where(r => r.NB == DocModels.Primitives.NB.B).Any(), B21);
            //B1.SetNext(b => b.Zam.Rows.Where(r => r.NB == DocModels.Primitives.NB.B).Count() == 0, B22);
            
            //B21.SetNext(b => true, B31);
            //B22.SetNext(b => true, B31);

            //B31.AddLink(b => true, A51, TranslatorB2A);




        }


    }
}
