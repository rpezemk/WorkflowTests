using WorkflowUMLDraw.Model;

namespace WorkflowUMLDraw.Solver
{
    public class Workspace
    {
        public Workspace()
        {
            
            for (int i = 0; i < 10; i++)
                AddStep(new MStep($@"Step {i}", "type1"));
        }

        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        public CellCollection Cells = new CellCollection(10, 10);
        
        public void AddStep(MStep step)
        {
            
        }

    }
}
