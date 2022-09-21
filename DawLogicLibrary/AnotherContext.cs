namespace DawLogicLibrary
{
    public class AnotherContext : DawWorkflowBase.Context.IContext
    {
        public DocModels.Doc Zam { get; set; }

        public string GetName()
        {
            return GetType().Name;
        }
    }

}
