using DawWorkflowBase.Steps;
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
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public List<MOutput> Outputs { get; set; }
        public Point Offset { get; set; }
        public IStep Step { get; set; }

    }
}
