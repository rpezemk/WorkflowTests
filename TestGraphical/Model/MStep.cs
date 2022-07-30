using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGraphical.Model
{
    public class MStep
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public List<MOutput> Outputs { get; set; }

    }
}
