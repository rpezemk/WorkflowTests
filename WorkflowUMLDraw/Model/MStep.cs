using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUMLDraw.Model
{
    public class MStep
    {
        public MStep(string name, string type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<MLink> Outputs { get; set; } = new List<MLink>();
    }
}
