using DawWorkflowBase.Context;
using DawWorkflowBase.Links;
using DawWorkflowBase.Steps;
using System;
using System.Linq;
using System.Reflection;

namespace DawWorkflowBase.Creators
{
    public class Creator
    {

    }

    public class TestContext : IContext
    {
        public string GetName()
        {
            return this.GetType().Name;
        }
    }
}
