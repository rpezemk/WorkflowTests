using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TestGraphical.Model;


namespace TestGraphical.ViewModel
{
    public class VM_Workflow : BindableBase
    {
        public VM_Workflow()
        {
            Events.StepSelectedEvent.Subscribe(MStepSelected);
            Events.ClearSelectionEvent.Subscribe(MStepSelectedClear);
        }

        private void MStepSelectedClear()
        {
            SelectedMSteps.Clear();
        }

        private void MStepSelected(MStep mstep)
        {
            if (!SelectedMSteps.Contains(mstep))
                SelectedMSteps.Add(mstep);
        }

        public VM_Workflow(MWorkflow _workflow)
        {
            this.workflow = _workflow;
        }

        private MWorkflow workflow;

        public MWorkflow Workflow
        {
            get { return workflow; }
            set { SetProperty(ref workflow, value); }
        }

        private ObservableCollection<VM_Step> stepVMs = new ObservableCollection<VM_Step>();
        public ObservableCollection<VM_Step> StepVMs
        {
            get { return stepVMs; }
            set { SetProperty(ref stepVMs, value); }
        }


        private ObservableCollection<MStep> selectedMSteps = new ObservableCollection<MStep>();

        public ObservableCollection<MStep> SelectedMSteps
        {
            get { return selectedMSteps; }
            set { SetProperty(ref selectedMSteps, value); }
        }

    }
}
