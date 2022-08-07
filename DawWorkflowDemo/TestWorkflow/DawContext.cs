using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase;
using DawWorkflowBase.WorkflowResult;

namespace DawWorkflowDemo.TestWorkflow
{

    public class DawContext : DawWorkflowBase.Context.IContext
    {
        public DocModels.Doc Doc = new DocModels.Doc(); 
        public DocModels.Zam Zam = new DocModels.Zam(); 
    }

    public class SomeOtherContext : DawWorkflowBase.Context.IContext
    {
        public DocModels.Doc Doc = new DocModels.Doc();
        public DocModels.Zam Zam = new DocModels.Zam();
    }



}
