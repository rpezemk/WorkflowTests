using DawWorkflowBase.Attributes;

namespace DawLogicLibrary
{
    public class SampleContext : DawWorkflowBase.Context.IContext
    {
        [Defaultable]
        public int counter;
        [Defaultable]
        public int c;
        public DocModels.Doc Doc { get; set; }
        public DocModels.Doc Zam { get; set; }
    }

}
