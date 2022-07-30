using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Context;

namespace DawWorkflowBase.Steps
{
    public interface IStep
    {
        string GetName();
        void AcceptContext(IContext parentContext);
        List<Links.ILinkInstance> GetLinks();
    }


    
}
