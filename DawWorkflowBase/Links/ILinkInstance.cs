using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Links
{
    public interface ILinkInstance
    {

        Conditions.ICondition GetCondition();
        string GetStepName();
        string GetResult();
        IStep GetResultStep();
    }


}
