using DawWorkflowBase.Conditions;
using DawWorkflowBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawLogicLibrary
{
    [Conditions]
    public static class Conditions
    {
        public static Condition<SampleContext> condSCAlwaysTrue = new Condition<SampleContext>(b => true, "Zawsze");
        public static Condition<SampleContext> condSCNeverTrue = new Condition<SampleContext>(b => true, "Nigdy");
        public static Condition<SampleContext> condSCDoc2001 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2001, "Paragon");
        public static Condition<SampleContext> condSCDoc2036 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2036, "Faktura krajowa");
        public static Condition<SampleContext> condSCDoc2037 = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.GidType == 2037, "Faktura exportowa");
        public static Condition<SampleContext> condSCDocIsNetto = new Condition<DawLogicLibrary.SampleContext>(b => b.Doc.NB == DocModels.Primitives.NB.N, "Dokument netto");
    }
}
