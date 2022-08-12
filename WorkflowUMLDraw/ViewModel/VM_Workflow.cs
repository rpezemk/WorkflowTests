using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using WorkflowUMLDraw.Model;
using WorkflowUMLDraw.Solver;

namespace WorkflowUMLDraw.ViewModel
{
    public class VM_Workflow : BindableBase
    {

        public VM_Workflow(MWorkflow _workflow)
        {
            this.workflow = _workflow;
            Workspace = new Workspace();
            Workspace.Cells.AddCols(20);
            var a = Workspace.Cells.Cells.SelectMany(c => c).ToList().Count();
            Workspace.Cells.AddRows(10);
            var b = Workspace.Cells.Cells.SelectMany(c => c).ToList().Count();
        }



        private MWorkflow workflow;

        public MWorkflow Workflow
        {
            get { return workflow; }
            set { SetProperty(ref workflow, value); }
        }


        private Workspace workspace;

        public Workspace Workspace
        {
            get { return workspace; }
            set { SetProperty(ref workspace, value); }
        }

    }
}
