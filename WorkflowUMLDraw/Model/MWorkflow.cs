using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUMLDraw.Model
{
    public class MWorkflow
    {
        public MWorkflow()
        {
            SetMyWorkflowSomehow();
        }
        public List<MStep> Steps { get; set; }
        public MStep RootStep { get; set; }


        public void SetMyWorkflowSomehow()
        {
            MStep root = new MStep("root", "type1");
            MStep step1 = new MStep("root", "type1");
            MStep step2 = new MStep("root", "type1");
            MStep step21 = new MStep("root", "type1");
            MStep step22 = new MStep("root", "type1");
            MStep endStep = new MStep("step3", "type1");
            MCondition cond1 = new MCondition("cond1");
            MCondition cond2 = new MCondition("cond2");
            MCondition cond3 = new MCondition("cond3");
            MCondition cond4 = new MCondition("cond4");

            root.Outputs = new List<MLink>() 
            { 
                new MLink(root, cond1, step1),
                new MLink(root, cond2, step2),
            };

            step2.Outputs = new List<MLink>()
            {
                new MLink(step2, cond3, step21),
                new MLink(step2, cond4, step22),
            };

            step1.Outputs = new List<MLink>()
            {
                new MLink(step1, cond4, endStep)
            };

            step21.Outputs = new List<MLink>()
            {
                new MLink(step21, cond4, endStep),
            };

            step22.Outputs = new List<MLink>()
            {
                new MLink(step21, cond4, endStep),
            };

        }
    }
}
