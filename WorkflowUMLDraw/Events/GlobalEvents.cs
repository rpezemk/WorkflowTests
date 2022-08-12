using System.Text;
using System.Threading.Tasks;

namespace WorkflowUMLDraw.Events
{
    public static class GlobalEvents
    {
        public static Event RefreshWorkflow = new Event();
        public static Event<Controls.StepControl> ControlClicked = new Event<Controls.StepControl>();
        public static Event<Controls.StepControl> ControlUnClicked = new Event<Controls.StepControl>();
    }
}
