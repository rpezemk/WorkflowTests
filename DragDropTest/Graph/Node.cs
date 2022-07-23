using DragDropTest.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DragDropTest.Graph
{
    public class Node : MyPoint
    {
        public DraggableControl Control { get; set; }
    }


    public class MyPoint
    {
        public Point Point { get; set; } = new Point(200, 300);
    }

}
