using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Links
{
    public class LinkInstance<InputContext, ResultContext> : ILinkInstance where InputContext : IContext where ResultContext : IContext
    {
        public LinkInstance(LinkDef<InputContext, ResultContext> linkDef, Steps.AStep<InputContext> inputStep, Steps.AStep<ResultContext> outputStep)
        {
            InputStep = inputStep;
            OutputStep = outputStep;
            LinkDef = linkDef;
        }

        //public LinkInstance(LinkDef<InputContext> linkDef, Steps.AStep<InputContext> inputStep, Steps.AStep<ResultContext> outputStep)
        //{
        //    InputStep = inputStep;
        //    OutputStep = outputStep;
        //    LinkDef = linkDef;
        //}

        public AStep<InputContext> InputStep { get; set; }
        public AStep<ResultContext> OutputStep { get; set; }
        public LinkDef<InputContext, ResultContext> LinkDef {get; set;}

        public ICondition GetCondition()
        {
            return LinkDef.Condition;
        }

        public IStep GetInputStep()
        {
            return InputStep;
        }

        public ILinkDef GetLinkDef()
        {
            return LinkDef;
        }

        public IStep GetResultStep()
        {
            return OutputStep;
        }
    }


}
