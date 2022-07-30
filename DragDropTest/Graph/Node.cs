﻿using DragDropTest.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropTest.Graph
{
    public class Node : MyPoint
    {
        public DraggableControl Control { get; set; }
        public Guid Guid = Guid.NewGuid();
        public List<Sub> Subs = new List<Sub>();
    }

}
