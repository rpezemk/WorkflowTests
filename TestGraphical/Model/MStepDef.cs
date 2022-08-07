using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Steps;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
namespace TestGraphical.Model
{
    public class MStepDef
    {
        public MStepDef(){}
        public MStepDef(IStepDef stepDef)
        {
            StepDef = stepDef;
        }
        public IStepDef StepDef { get; set; }
    }
}
