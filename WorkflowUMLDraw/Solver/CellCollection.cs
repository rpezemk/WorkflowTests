using System.Collections.Generic;
using System.Linq;

namespace WorkflowUMLDraw.Solver
{
    public class CellCollection
    {
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;
        public List<List<BlankCell>> Cells = new List<List<BlankCell>>();

        public CellCollection(int w, int h)
        {
            Cells = Enumerable.Range(0, h).Select(i => Enumerable.Range(0, w).Select(j => new BlankCell()).ToList()).ToList();
        }

        public void AddCols(int n)
        {
            Width += n;
            Cells = Cells.Select(r => r.Union(Enumerable.Range(0, n).Select(i => new BlankCell()).ToList()).ToList()).ToList();
        }

        public void AddRows(int n)
        {
            Height += n;
            Cells = Cells.Union(Enumerable.Range(0, n).Select(i => Enumerable.Range(0, Width).Select(c =>  new BlankCell()).ToList())).ToList();
        }

    }
}
