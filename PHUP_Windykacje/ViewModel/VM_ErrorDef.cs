using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHUP_Windykacje.ViewModel
{
    public class VM_ErrorDef : BindableBase
    {

		private int id;

		public int ID
		{
			get { return id; }
			set { SetProperty(ref id, value); }
		}



		private string name;

		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}


		private string tip;

		public string Tip
		{
			get { return tip; }
			set { SetProperty(ref tip, value); }
		}



		private ObservableCollection<VM_ErrorFilterColumn> filters = new ObservableCollection<VM_ErrorFilterColumn>();

		public ObservableCollection<VM_ErrorFilterColumn> Filters
		{
			get { return filters; }
			set { SetProperty(ref filters, value); }
		}
    }
}
