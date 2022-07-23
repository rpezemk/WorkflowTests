namespace DawWorkflowBase.Steps
{
    public class FlowBind<TArg, TStep> where TStep : IStep
    {
        public TArg Arg { get; set; }
        public DawWorkflowBase.Steps.IStep Step { get; set; }
    }


}
