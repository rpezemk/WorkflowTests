using DawWorkflowBase.Context;
using System;

namespace DawWorkflowBase.Steps
{
    public interface IStepDef
    {
        string GetName();
        Type GetStepType();
        void SetAction(object action);
    }
}
