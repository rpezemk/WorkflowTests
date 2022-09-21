using DawWorkflowBase.Conditions;
using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Links
{
    //public class LinkDef<InputContext, ResultContext> : ILinkDef where InputContext : IContext where ResultContext : IContext
    //{


    //    public LinkDef(Conditions.Condition<InputContext> condition, Func<InputContext, ResultContext> translator)
    //    {
    //        Condition = condition;
    //        Translator = new Translators.Translator<InputContext, ResultContext>() { Func = translator };
    //    }

    //    public Type InContextType { get => typeof(InputContext); }
    //    public Type OutContext { get => typeof(ResultContext); }
    //    public Conditions.Condition<InputContext> Condition { get; set; }

    //    public Translators.Translator<InputContext, ResultContext> Translator { get; set; }
    //    public ICondition GetCondition()
    //    {
    //        return Condition;
    //    }

    //}

    //public class LinkDef<InputContext> : ILinkDef where InputContext : IContext 
    //{


    //    public LinkDef(Conditions.Condition<InputContext> condition, Func<InputContext, InputContext> translator)
    //    {
    //        Condition = condition;
    //    }

    //    public Type InContextType { get => typeof(InputContext); }
    //    public Conditions.Condition<InputContext> Condition { get; set; }

    //    public ICondition GetCondition()
    //    {
    //        return Condition;
    //    }

    //}




}
