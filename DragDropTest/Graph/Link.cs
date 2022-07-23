using DragDropTest.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropTest.Graph
{
    public class Link
    {
        public LinkControl LinkControl { get; set; }
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
    }
}
