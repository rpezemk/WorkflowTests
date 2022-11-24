using Prism.Commands;
using Prism.Mvvm;

namespace PHUP_Windykacje.ViewModel
{
    public class VM_RowFilter : BindableBase
    {
		public VM_RowFilter()
		{
			ResetFilterCmd = new DelegateCommand(() => { IsActive = false; Value = ""; });
		}

		private string name = "";

		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}



		private DelegateCommand resetFilterCmd;

		public DelegateCommand ResetFilterCmd
		{
			get { return resetFilterCmd; }
			set { SetProperty(ref resetFilterCmd, value); }
		}

		private string caption;

		public string Caption
		{
			get { return caption; }
			set { SetProperty(ref caption, value); }
		}

		private string _value;

		public string Value
		{
			get { return _value; }
			set { SetProperty(ref _value, value); Events.RaportFiltersChangedCmd.Publish(); }
		}


		private bool isActive;

		public bool IsActive
		{
			get { return isActive; }
			set { SetProperty(ref isActive, value); Events.RaportFiltersChangedCmd.Publish(); }
		}

    }
}