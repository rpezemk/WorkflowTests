using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Links
{
    public class LinkInstance<InputContext> where InputContext : IContext 
    {
        public LinkInstance(Condition<InputContext> condition, Steps.AStep<InputContext> outputStep)
        {
            OutputStep = outputStep;
            Condition = condition;
        }

        //public LinkInstance(LinkDef<InputContext> linkDef, Steps.AStep<InputContext> inputStep, Steps.AStep<ResultContext> outputStep)
        //{
        //    InputStep = inputStep;
        //    OutputStep = outputStep;
        //    LinkDef = linkDef;
        //}

        public AStep<InputContext> OutputStep { get; set; }
        public Condition<InputContext> Condition {get; set;}

        public ICondition GetCondition()
        {
            return Condition;
        }



        public IStep GetResultStep()
        {
            return OutputStep as IStep;
        }
    }


}
