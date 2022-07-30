using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace TestGraphical.ViewModel
{
    public class VM_Step : BindableBase
    {
        public Guid Guid { get; set; }
        public Guid VisualGuid { get; set; } = Guid.NewGuid();
        private double xOffset;
        public double XOffset
        {
            get { return xOffset; }
            set { SetProperty(ref xOffset, value); }
        }

        private double yOffset;
        public double YOffset
        {
            get { return yOffset; }
            set { SetProperty(ref yOffset, value); }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }


    }
}
