using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Attributes;

namespace DawLogicLibrary
{
    [DawWorkflowBase.Attributes.SteppableLibrary]
    public static class SteppableMethods
    {
        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod1(SampleContext context)
        {

        }

        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod2(SampleContext context)
        {
            
        }

        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod3(SampleContext context)
        {

        }

        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod4(SampleContext context)
        {

        }

        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod5(SampleContext context)
        {

        }

        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod7(SampleContext context)
        {

        }

        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod8(SampleContext context)
        {

        }


        [DawWorkflowBase.Attributes.SteppableMethod]
        public static void SteppableMethod9(AnotherContext context)
        {

        }


        [DawWorkflowBase.Attributes.Translator]
        public static AnotherContext ConverterMethod1(SampleContext context)
        {
            return new AnotherContext();
        }


        [DawWorkflowBase.Attributes.Translator]
        public static SampleContext ConverterMethod2(AnotherContext context)
        {
            return new SampleContext();
        }

        internal static SampleContext ConverterMethod3(SampleContext arg)
        {
            throw new NotImplementedException();
        }


    }

}
