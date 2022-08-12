using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowUMLDraw.Events
{
    public class Event
    {
        public List<Action> Subscribers = new List<Action>();

        public void Publish()
        {
            foreach (var Action in Subscribers)
            {
                if (Action == null)
                    return;
                Action.Invoke();
            }
        }
        public void Subscribe(Action subscriber)
        {
            if (Subscribers.Where(s => s == subscriber).Any())
                return;
            Subscribers.Add(subscriber);
        }
    }
}
