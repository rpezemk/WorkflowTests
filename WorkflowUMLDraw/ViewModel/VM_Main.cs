using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using WorkflowUMLDraw.Model;
namespace WorkflowUMLDraw.ViewModel
{
    public class VM_Main : BindableBase
    {

        public VM_Main()
        {
            Workflow = new MWorkflow();
            VMWorkflow = new VM_Workflow(Workflow);
        }

        private VM_Workflow vmWorkflow;

        public VM_Workflow VMWorkflow
        {
            get { return vmWorkflow; }
            set { SetProperty(ref vmWorkflow, value); }
        }


        private MWorkflow workflow;

        public MWorkflow Workflow
        {
            get { return workflow; }
            set { SetProperty(ref workflow, value); }
        }

    }
}
