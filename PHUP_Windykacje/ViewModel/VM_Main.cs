using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace PHUP_Windykacje.ViewModel
{
    public class VM_Main : BindableBase
    {
        public VM_Main()
        {

        }



        private VM_TabRaport vm_RaportTable = new VM_TabRaport();

        public VM_TabRaport VM_RaportTable
        {                       
            get { return vm_RaportTable; }
            set { SetProperty(ref vm_RaportTable, value); }
        }

    }
}
