using DragDropTest.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropTest.SGraph
{
    public class SNode : MyPoint
    {
        public Guid Guid;
        public List<Sub> Subs = new List<Sub>();
    }
}
