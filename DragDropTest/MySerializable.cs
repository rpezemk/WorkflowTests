using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropTest
{
    public class MySerializable
    {
        //public List<Graph.Link> Links = new List<Graph.Link>();
        //public List<Graph.Node> Nodes = new List<Graph.Node>();
        public List<SGraph.SLink> SLinks = new List<SGraph.SLink>();
        public List<SGraph.SNode> SNodes = new List<SGraph.SNode>();
    }
}
