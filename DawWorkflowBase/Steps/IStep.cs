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
        bool CheckIfEndPoint();
        void RunStep(IContext context);
        void SetEndPoint(bool isEndPoint);
        IContext GetContext();
        List<Links.ILinkInstance> GetLinks();
        string ToString();
    }


    
}
