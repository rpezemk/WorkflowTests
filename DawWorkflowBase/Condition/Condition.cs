using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using static DawWorkflowBase.Conditions.Expression;

namespace DawWorkflowBase.Conditions
{

    public interface ICondition
    {
        bool Evaluate(IContext context);
    }

    public class Condition<TContext> : ICondition where TContext: Context.IContext
    {
        public Func<TContext, bool> Func { get; set; }
        public string Name { get; set; }
        public bool IsEndPoint { get; set; }
        public bool IsNot { get; set; }
        public Condition<TContext> Child1 { get; set; }
        public Condition<TContext> Child2 { get; set; }
        public Operator Operator;
        public Condition(Func<TContext,bool> func, string name = "") 
        {
            Func = func;
            IsEndPoint = true;
            Name = name;
        }

        public Condition(string name = "")
        {
            Name = name;
        }

        public Condition<TContext> AND(Condition<TContext> c2)
        {
            return new Condition<TContext>() { Child1 = this, Child2 = c2, Operator = new AND(), IsEndPoint = false };
        }

        public Condition<TContext> ANDNOT(Condition<TContext> c2)
        {
            return new Condition<TContext>() { Child1 = this, Child2 = c2, Operator = new AND(), IsEndPoint = false, IsNot = true };
        }


        public Condition<TContext> OR(Condition<TContext> c2)
        {
            return new Condition<TContext>() { Child1 = this, Child2 = c2, Operator = new OR(), IsEndPoint = false };
        }

        public Condition<TContext> NOT()
        {
            var res = this.Clone();
            res.IsNot = !res.IsNot;
            return res;
        }

        public bool Evaluate(TContext context)
        {
            
            if (IsEndPoint)
            {
                var res = Func.Invoke(context);
                return res ^ IsNot;
            }
            else
            {
                var res1 = Child1.Evaluate(context);
                var opType = Operator.GetType();
                var isOr = opType != typeof(AND);
                if (res1 == isOr)
                    return isOr;
                var res2 = Child2.Evaluate(context);
                return res2 ^ IsNot;
            }
        }

        public Condition<TContext> Clone()
        {
            var cloned = new Condition<TContext>();
            cloned.IsEndPoint = IsEndPoint;
            if (!IsEndPoint)
            {
                if (Child1 != null)
                    cloned.Child1 = Child1;
                if (Child2 != null)
                    cloned.Child2 = Child2;
            }

            cloned.IsNot = IsNot;
            cloned.Name = Name + (IsNot == true? "*NOT ": "");
            if(Operator != null)
            {
                cloned.Operator = Operator;
            }
            if (Func != null)
                cloned.Func = Func;

            return cloned;
        }

        public bool Evaluate(IContext context)
        {
            if (IsEndPoint)
            {
                var res = Func.Invoke((TContext)context);
                return res ^ IsNot;
            }
            else
            {
                var res1 = Child1.Evaluate(context);
                var opType = Operator.GetType();
                var isOr = opType != typeof(AND);
                if (res1 == isOr)
                    return isOr;
                var res2 = Child2.Evaluate(context);
                return res2 ^ IsNot;
            }
        }
    }


    public static class ConditionHelpers
    {
        public static Condition<TContext> IF<TContext>(this Condition<TContext> condition, Func<TContext, bool> func) where TContext : Context.IContext
        {
            return condition;
            //return this;
        }
    }






    public class TestContext : Context.IContext
    {
        public Doc Doc;
    }

    public class Doc
    {
        public int GidType { get; set; } = 0;
        public int GidNumer { get; set; } = 0;
        public int GidLp { get; set; } = 0;
        public int GidSubLp { get; set; } = 0;
        public string StrDocNo { get; set; } = "";
        public string SeriesNo { get; set; } = "";
        public string NB { get; set; } = "N";
        //public List<Pos> Rows { get; set; } = new List<Pos>();

    }

}
