using PHUP_Windykacje.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PHUP_Windykacje
{
    public static class Events
    {
        public static Event<VM_RaportRow> RaportRowSelectionChanged = new Event<VM_RaportRow>();
        public static Event<VM_TabRaport> LoadRaportDataCmd = new Event<VM_TabRaport>();
        public static Event RaportFiltersChangedCmd = new Event();
    }

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
