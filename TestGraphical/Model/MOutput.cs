using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGraphical.Model
{
    public class MOutput
    {
        public string Name { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid ParentGuid { get; set; }
        public MCondition MCondition  { get; set; }
        public MStep MStep { get; set; }
    }
}
