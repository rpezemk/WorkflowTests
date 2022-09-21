using DawWorkflowBase.Context;

//using static DawWorkflowBase.Conditions.Expression;

namespace DawWorkflowBase.Conditions
{
    public interface ICondition
    {
        bool Evaluate(IContext context);
    }

}
