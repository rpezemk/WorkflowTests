using WorkflowUMLDraw.Model;

namespace WorkflowUMLDraw.Solver
{
    public class BlankCell : ACell
    {
        MStep MStep = null;
        private int myVar;

        public override bool IsUsed
        {
            get { return MStep != null; }
        }
    }



}
