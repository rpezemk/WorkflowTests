using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Links
{
    public class LinkInstance<InputContext> : ILinkInstance where InputContext : IContext 
    {
        public LinkInstance(Condition<InputContext> condition, Steps.AStep<InputContext> outputStep)
        {
            OutputStep = outputStep;
            Condition = condition;
        }

        public AStep<InputContext> OutputStep { get; set; }
        public Condition<InputContext> Condition {get; set;}

        public ICondition GetCondition()
        {
            return Condition;
        }

        public string GetResult()
        {
            return "dummyres";
        }

        public IStep GetResultStep()
        {
            return OutputStep as IStep;
        }

        public string GetStepName()
        {
            return "step name....";
        }
    }
}
