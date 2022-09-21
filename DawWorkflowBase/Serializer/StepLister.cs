using DawWorkflowBase.Links;
using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Serializer
{
    public class StepLister
    {
        public List<IStep> StepsSerialized = new List<IStep>();

        public List<ILinkInstance> LinkInstances = new List<ILinkInstance>();

        public List<IStep> CurrPath = new List<IStep>();

        public void SerializeStep(IStep step)
        {
            //StepsSerialized.Clear();
            if (!StepsSerialized.Contains(step))
            {
                StepsSerialized.Add(step);
                var children = step.GetLinks().Select(l => l.GetResultStep()).ToList();
                if(children.Count > 0)
                {
                    foreach (var childStep in children)
                    {
                        SerializeStep(childStep);
                    }
                }
                else
                {
                    step.SetEndPoint(true);
                }

            }
            LinkInstances = StepsSerialized.SelectMany(s => s.GetLinks()).Distinct().ToList();
        }
    }
}
