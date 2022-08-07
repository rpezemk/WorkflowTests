using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Links
{
    public interface ILinkInstance
    {

        Conditions.ICondition GetCondition();
        IStep GetInputStep();
        IStep GetResultStep();
        ILinkDef GetLinkDef();
    }


}
