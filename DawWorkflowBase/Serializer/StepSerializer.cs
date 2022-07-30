using DawWorkflowBase.Links;
using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Serializer
{
    public class StepSerializer
    {
        public List<IStep> Steps = new List<IStep>();
        public List<ILink> Links = new List<ILink>();

        public List<IStep> CurrPath = new List<IStep>();

        public void SerializeStep(IStep step)
        {
            if (!Steps.Contains(step))
            {
                Steps.Add(step);
                foreach(var childStep in step.GetLinks().Select(l => l.GetResultStep()))
                {
                    SerializeStep(childStep);
                }
            }
        }
    }
}
