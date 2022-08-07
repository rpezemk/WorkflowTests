using DawWorkflowBase.Context;
using DawWorkflowBase.Serializer;
using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Workers
{
    /// <summary>
    /// Class for running Workflow
    /// </summary>
    public class Worker<TContext> where TContext : IContext
    {
        public StepLister StepSerializer = new StepLister();
        public List<IStep> AllSteps = new List<IStep>();
        public Workflow.AWorkflowBase<TContext> Workflow;

        public Worker(Workflow.AWorkflowBase<TContext> workflow)
        {
            Workflow = workflow;
        }

        public bool RunWorkflow()
        {
            try
            {
                StepSerializer.SerializeStep(Workflow.RootStep);
                AllSteps = StepSerializer.StepsSerialized;
                if (AllSteps.Where(st => st.CheckIfEndPoint() == true).Any())
                {
                    var root = AllSteps[0];
                    var currStep = root;
                    var currContext = (IContext)Workflow.Context;
                    while (currStep.CheckIfEndPoint() == false)
                    {
                        currStep.RunStep(currContext);
                        var links = currStep.GetLinks().ToList();
                        var processedOK = false;
                        foreach (var link in links)
                        {
                            var condition = link.GetCondition();
                            var isToProcess = condition.Evaluate(currContext);
                            if (isToProcess)
                            {
                                currStep = link.GetResultStep();
                                currContext = currStep.GetContext();
                                processedOK = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (processedOK == false)
                        {
                            throw new Exception($"did not manage to process {currStep}");
                        }
                    }

                    if (currStep.CheckIfEndPoint() == true)
                    {
                        currStep.RunStep(currContext);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return true;
        }

    }
}
