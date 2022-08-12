﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGraphical.Model
{
    public class MLink
    {
        //private MStep root;
        //private MCondition cond1;
        //private MStep step2;

        public MLink(MStep root, MCondition cond1, MStep step2)
        {
            this.InputStep = root;
            this.Condition = cond1;
            this.OutputStep = step2;
        }

        public MStep InputStep { get; set; }
        public MStep OutputStep { get; set; }
        public MCondition Condition { get; set; }
    }
}
