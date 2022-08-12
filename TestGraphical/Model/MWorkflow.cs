using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGraphical.Model
{
    public class MWorkflow
    {
        public MWorkflow()
        {

        }
        public List<MStep> Steps { get; set; } = new List<MStep>();
        public MStep RootStep { get; set; }


        public void SetMyWorkflowSomehow()
        {
            MStep root = new MStep("root", "type1", 150, 30);
            MStep step1 = new MStep("step1", "type1", 50, 200);
            MStep step2 = new MStep("step2", "type1", 250, 200);
            MStep step21 = new MStep("step21", "type1", 190, 340);
            MStep step22 = new MStep("step22", "type1", 270, 340);
            MStep endStep = new MStep("endStep", "type1", 150, 500);
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

            RootStep = root;
            Steps.Add(root);
            Steps.Add(step2);
            Steps.Add(step1);
            Steps.Add(step22);
            Steps.Add(step21);
            Steps.Add(endStep);

        }
    }
}
