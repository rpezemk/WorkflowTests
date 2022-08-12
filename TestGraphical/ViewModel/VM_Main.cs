using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TestGraphical.Model;
using System.Net;
using System.Net.Sockets;

namespace TestGraphical.ViewModel
{
    public class VM_Main : BindableBase
    {

        private DelegateCommand selectAllCmd;
        private DelegateCommand deselectAllCmd;
        private DelegateCommand saveAllCmd;
        private DelegateCommand loadWorkflowCmd;
        private DelegateCommand addControlCmd;
        private DelegateCommand testingCmd;

        private DelegateCommand connectExperimentalCmd;
        public DelegateCommand ConnectExperimentalCmd =>
            connectExperimentalCmd ?? (connectExperimentalCmd = new DelegateCommand(ExecuteConnectExperimentalCmd));
        private DelegateCommand loadExampleCmd;
        public DelegateCommand LoadExampleCmd =>
            loadExampleCmd ?? (loadExampleCmd = new DelegateCommand(LoadExample));
        public DelegateCommand TestingCmd => testingCmd ?? (testingCmd = new DelegateCommand(TestingFunc));

        private void TestingFunc()
        {
            
        }

        void ExecuteConnectExperimentalCmd()
        {
            Events.RefreshLinesEvent.Publish();
        }

        //LoadExampleCmd

        void LoadExample()
        {
            MWorkflow mWorkflow = new MWorkflow();
            mWorkflow.SetMyWorkflowSomehow();
            VM_Workflow.Workflow = mWorkflow;
            VM_Workflow.StepVMs = new ObservableCollection<VM_Step>(mWorkflow.Steps.Select(ms => new VM_Step(ms)).ToList());
            Events.RefreshWorkflow.Publish();
        }

        private string statusText;
        private double mouseX;
        private double mouseY;

        

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

        }
        void AddControl()
        {
            var vm = new VM_Step();
            VM_Workflow.StepVMs.Add(vm);
            Events.AddStepToCanvasEvt.Publish(vm);
        }

        private VM_Workflow vm_workflow = new VM_Workflow();

        public VM_Workflow VM_Workflow
        {
            get { return vm_workflow; }
            set { SetProperty(ref vm_workflow, value); }
        }

    }
}
