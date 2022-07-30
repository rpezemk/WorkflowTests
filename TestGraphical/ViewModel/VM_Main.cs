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
    public class VM_Main : BindableBase
    {

        private DelegateCommand selectAllCmd;
        private DelegateCommand deselectAllCmd;
        private DelegateCommand saveAllCmd;
        private DelegateCommand loadWorkflowCmd;

        private DelegateCommand addControlCmd;


        private string statusText;
        private double mouseX;
        private double mouseY;
        private bool controlClicked = false;
        private ObservableCollection<VM_Step> stepVMs = new ObservableCollection<VM_Step>();
        

        public DelegateCommand SaveWorkflowCmd =>
            saveAllCmd ?? (saveAllCmd = new DelegateCommand(SaveWorkflow));

        public DelegateCommand LoadWorkflowCmd =>
            loadWorkflowCmd ?? (loadWorkflowCmd = new DelegateCommand(LoadWorkflow));
        public DelegateCommand SelectAllCmd =>
            selectAllCmd ?? (selectAllCmd = new DelegateCommand(SelectAll));

        public DelegateCommand DeselectAllCmd =>
            deselectAllCmd ?? (deselectAllCmd = new DelegateCommand(DeselectAll));

        private DelegateCommand canvasClickedCmd;
        public DelegateCommand CanvasClickedCmd =>
            canvasClickedCmd ?? (canvasClickedCmd = new DelegateCommand(CanvasClicked));
        public DelegateCommand AddControlCmd =>
            addControlCmd ?? (addControlCmd = new DelegateCommand(AddControl));

        

        public VM_Main()
        {
            stepVMs = new ObservableCollection<VM_Step>();
            StepVMs.Add(new VM_Step());
            StepVMs.Add(new VM_Step());
            StepVMs.Add(new VM_Step());
        }


        void LoadWorkflow() { }
        void SaveWorkflow() { }
        void SelectAll() { }
        void DeselectAll() { }
        void CanvasClicked() 
        {
            // 
            // code for canvas clicking
            //
            controlClicked = false;
        }
        void AddControl()
        {
            var vm = new VM_Step();
            StepVMs.Add(vm);
            Events.AddStepToCanvasEvt.Publish(vm);
        }

        public ObservableCollection<VM_Step> StepVMs
        {
            get { return stepVMs; }
            set { SetProperty(ref stepVMs, value); }
        }
        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        public double MouseX
        {
            get { return mouseX; }
            set { SetProperty(ref mouseX, value); }
        }

        public double MouseY
        {
            get { return mouseY; }
            set { SetProperty(ref mouseY, value); }
        }

    }
}
