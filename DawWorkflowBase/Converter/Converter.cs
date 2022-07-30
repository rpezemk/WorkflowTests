using DawWorkflowBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawWorkflowBase.Converter
{
    public class Converter<InputContext, OutputContext> where InputContext: IContext where  OutputContext: IContext
    {
        public Func<InputContext, OutputContext> Func { get; set; }
    }
}
