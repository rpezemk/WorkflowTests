using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGraphical
{
    public static class Events
    {
        public static Event RefreshControlsEvt = new Event();
        public static Event<ViewModel.VM_Step> AddStepToCanvasEvt = new Event<ViewModel.VM_Step>();
        public static Event<Controls.StepControl> ControlClicked = new Event<Controls.StepControl>();
        public static Event<Controls.StepControl> ControlUnClicked = new Event<Controls.StepControl>();
        public static Event<Controls.StepOutput> DeleteOutput = new Event<Controls.StepOutput>();
        public static Event ConnectExperimental = new Event();
    }

    public class Event
    {
        public List<Action> Subscribers = new List<Action>();

        public void Publish()
        {
            foreach(var Action in Subscribers)
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
