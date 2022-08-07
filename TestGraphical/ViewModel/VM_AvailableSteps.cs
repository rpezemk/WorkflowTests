using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;


namespace TestGraphical.ViewModel
{
    public class VM_AvailableSteps : BindableBase
    {
        public VM_AvailableSteps()
        {
            
        }

        private ObservableCollection<Model.MStepDef> stepDefs;
        public ObservableCollection<Model.MStepDef> StepDefs
        {
            get { return stepDefs; }
            set { SetProperty(ref stepDefs, value); }
        }
    }
}
