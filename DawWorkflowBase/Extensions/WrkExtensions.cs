using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Context;
using DawWorkflowBase.Links;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Workflow;

namespace DawWorkflowBase.Extensions
{


    public static class WrkExtensions
    {

        public static AStep<TContext> SetRoot<TContext>(this AWorkflowBase<TContext> workflow, AStep<TContext> step) where TContext : IContext
        {
            workflow.RootStep = step;
            return step;
        }

        public static void SetContext<TContext>(this AWorkflowBase<TContext> workflow, TContext context) where TContext : IContext
        {
            workflow.Context = context;
        }

        public static AStep<TContext> AppendSteps<TContext>(this AStep<TContext> thisStep, params (Conditions.Condition<TContext>, ChoiceNode<TContext>)[] ps) where TContext : IContext
        {
            return thisStep;
        }

        public static (Conditions.Condition<TContext>, ChoiceNode<TResult>) AppendSteps<TContext, TResult>(
                        this (Conditions.Condition<TContext>, ChoiceNode<TResult>) k, 
                        params (Conditions.Condition<TContext>, ChoiceNode<TContext>)[] ps) where TContext : IContext where TResult : IContext
        {
            return k;
        }


        public static (Conditions.Condition<TContext>, ChoiceNode<TResult>) GetNode<TContext, TResult>(this (Conditions.Condition<TContext>, ChoiceNode<TResult>) k, out ChoiceNode<TResult> s) where TContext : IContext where TResult : IContext
        {
            s = new ChoiceNode<TResult>(new StepDef<TResult>());
            return k;
        }

        public static ChoiceNode<TContext> GetOut<TContext>(this ChoiceNode<TContext> thisStep, out ChoiceNode<TContext> s) where TContext : IContext 
        {//(LinkDef<InputContext, ResultContext> linkDef, Steps.AStep<InputContext> inputStep, Steps.AStep<ResultContext> outputStep)
            s = thisStep;
            return thisStep;
        }

    }



}
