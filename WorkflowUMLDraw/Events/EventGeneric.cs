using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowUMLDraw.Events
{
    public class Event<TObj>
    {
        public List<Action<TObj>> Subscribers = new List<Action<TObj>>();

        public void Publish(TObj o)
        {
            foreach (var Action in Subscribers)
            {
                if (Action == null)
                    return;
                Action.Invoke(o);
            }
        }
        public void Subscribe(Action<TObj> subscriber)
        {
            if (Subscribers.Where(s => s.Equals(subscriber)).Any())
                return;
            Subscribers.Add(subscriber);
        }
    }
}
