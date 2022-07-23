using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowDemo.DocModels.Primitives;

namespace DawWorkflowDemo.DocModels
{
    public class Pos
    {
        public int GidType { get; set; } = 0;
        public int GidNumer { get; set; } = 0;
        public int GidLp { get; set; } = 0;
        public int GidSubLp { get; set; } = 0;
        public int TwrType { get; set; } = 0;
        public int TwrNumer { get; set; } = 0;
        public int TwrLp { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public NB NB { get; set; }
    }
}
