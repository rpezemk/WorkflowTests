﻿using DawWorkflowBase.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestGraphical.Model
{
    public class MStep
    {
        public MStep(string name, string type, double xOffset = 0.0, double yOffset = 0.0)
        {
            Name = name;
            Type = type;
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public List<MLink> Outputs { get; set; } = new List<MLink>();

        public double XOffset { get; set; }
        public double YOffset { get; set; }

    }
}
