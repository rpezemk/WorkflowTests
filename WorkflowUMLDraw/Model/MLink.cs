using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUMLDraw.Model
{
    public class MLink
    {
        private MStep root;
        private MCondition cond1;
        private MStep step2;

        public MLink(MStep root, MCondition cond1, MStep step2)
        {
            this.root = root;
            this.cond1 = cond1;
            this.step2 = step2;
        }

        public MStep InputStep { get; set; }
        public MStep OutputStep { get; set; }
        public MCondition Condition { get; set; }
    }
}
