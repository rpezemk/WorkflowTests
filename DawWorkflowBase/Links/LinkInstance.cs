using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Links
{
    public class LinkInstance<InputContext,OutputContext> : ILinkInstance where InputContext : IContext where OutputContext : IContext
    {
        public LinkInstance(Condition<InputContext> condition, Steps.AStep<InputContext,OutputContext> outputStep)
        {
            OutputStep = outputStep;
            Condition = condition;
        }

        public AStep<InputContext,OutputContext> OutputStep { get; set; }
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
