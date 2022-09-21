using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;

namespace DawWorkflowBase.Context
{
    public interface IContext
    {
        string GetName();
    }
}
