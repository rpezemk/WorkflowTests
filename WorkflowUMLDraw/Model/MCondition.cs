using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUMLDraw.Model
{
    public class MCondition
    {
        public MCondition(string name = "Some condition")
        {
            Name = name;
        }
        public string Name { get; set; }
        public bool ReturnedValue { get; set; }
    }
}
