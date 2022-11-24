using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PHUP_Windykacje.ViewModel
{
	public class VM_TabRaport : BindableBase
	{

		public VM_TabRaport()
		{
			LoadRaportDataCommand = new DelegateCommand(LoadRaportData);
			Events.RaportRowSelectionChanged.Subscribe(SelectionChanged);
			SendToDBCommand = new DelegateCommand(SendToDB);
			Events.RaportFiltersChangedCmd.Subscribe(FiltersChanged);
		}


		private void InitFilters()
		{
			Filters = new ObservableCollection<VM_RowFilter>();
			var props = typeof(VM_RaportRow).GetProperties();
			foreach (var prop in props)
			{
				var attrList = prop.GetCustomAttributes(false);
				foreach (var attr in attrList)
				{
					ColumnCaptionAttribute captionAttribute = attr as ColumnCaptionAttribute;
					if (captionAttribute != null)
					{
						Filters.Add(new VM_RowFilter() { Name = prop.Name, Caption = captionAttribute.PositionalString, IsActive = false, Value = "" });
					}
				}

			}
		}

		private void FiltersChanged()
		{
			RefreshFiltered();
		}

		#region Private methods

		private async void SendToDB()
		{
			ControlsEnabled = false;
			var task = Task.Run(() => SQL.InsertUpdateOpisyRaportu1(VM_RaportRows.Select(vm => vm.RaportRow).Where(r => string.IsNullOrEmpty(r.Opis) == false).ToList(), (n, c) => UpdateAction(n, c)));
			await task;
			LoadRaportDataCommand.Execute();
			RaportProgress = 0;
			ControlsEnabled = true;
		}

		private void UpdateAction(int n, int c)
		{
			if (n == 0)
				n = 1;
			RaportProgress = (100 * c) / n;
		}

		private void SelectionChanged(VM_RaportRow obj)
		{
			SelectedRaportRowVM = obj;
		}

		private void LoadRaportData()
		{
			InitFilters();
			Events.LoadRaportDataCmd.Publish(this);
			var list = SQL.GetRaportRows(DataOd, DataDo, Seria, out query);
			Query = query;
			VM_OriginalRaportRows.Clear();
			foreach (var row in list)
			{
				VM_OriginalRaportRows.Add(new VM_RaportRow(row));
			}

			RefreshFiltered();
		}

		private void RefreshFiltered()
		{
			var resRaportRows = new ObservableCollection<VM_RaportRow>(VM_OriginalRaportRows.Select(r => r).ToList());
			if (FiltersVisible)
			{

				foreach (var filter in Filters.Where(f => f.IsActive))
				{
					if (filter.Value.NotNullOrEmpty())
					{
						try
						{
							if (filter.Value.Contains(';'))
							{
								var fListLower = filter.Value.ToLower().Split(';', StringSplitOptions.RemoveEmptyEntries).Select(f => f ?? "");
								if (fListLower.All(f => f.IsNullOrEmpty()))
									continue;


                                resRaportRows = new ObservableCollection<VM_RaportRow>(
                                   resRaportRows.Where(vm =>
                                   {
                                       var prop = vm.GetType().GetProperty(filter.Name);
                                       var propStrLower = "";
                                       if (prop.PropertyType == typeof(DateTime))
                                       {
                                           propStrLower = ((DateTime)prop.GetValue(vm)).ToString("yyyy-MM-dd").ToLower();
                                       }
                                       else
                                           propStrLower = prop.GetValue(vm).ToString().ToLower();


									   var res2 = fListLower.Select(f => f.Replace(',', '.') ?? "").Where(f => propStrLower.Contains(f)).Any();
									   if(res2 == false)
										   res2 = fListLower.Select(f => f.Replace('.', ',') ?? "").Where(f => propStrLower.Contains(f)).Any();
									   else if(res2 == false)
                                           res2 = fListLower.Select(f => f.Replace('.', ',') ?? "").Where(f => propStrLower.Contains(f)).Any();

                                       return res2;
                                   }

                                   ).ToList()
                               );
                            }
							else
							{
                                resRaportRows = new ObservableCollection<VM_RaportRow>(
                                    resRaportRows.Where(vm =>
                                    {
                                        var prop = vm.GetType().GetProperty(filter.Name);
                                        var valStr = "";
                                        if (prop.PropertyType == typeof(DateTime))
                                        {
                                            valStr = ((DateTime)prop.GetValue(vm)).ToString("yyyy-MM-dd");
                                        }
                                        else
                                            valStr = prop.GetValue(vm).ToString();

                                        var res = valStr.ToLower().Contains(filter.Value.Replace(',', '.').ToLower() ?? "");
                                        if (res == false)
                                            res = valStr.ToLower().Contains(filter.Value.Replace('.', ',').ToLower() ?? "");
                                        else if (res == false)
                                            res = valStr.ToLower().Contains(filter.Value.ToLower() ?? "");
                                        return res;
                                    }

                                    ).ToList()
                                );
                            }
							
						}
						catch
						{

						}
					}
				}
			}
			VM_RaportRows = new ObservableCollection<VM_RaportRow>(resRaportRows);
		}

		#endregion Private methods


		#region Private fields
		private bool controlsEnabled = true;
		private DateTime dataOd = DateTime.Now.AddDays(-61);// DateTime.Now.AddDays(-61);
		private DateTime dataDo = DateTime.Now;
		private string seria = "01EL";
		private ObservableCollection<VM_RaportRow> vM_RaportRows = new ObservableCollection<VM_RaportRow>();
		private ObservableCollection<VM_RaportRow> vM_OriginalRaportRows = new ObservableCollection<VM_RaportRow>();
		private int raportProgress;
		private string query;
		private DelegateCommand testCommand;
		private DelegateCommand sendToDBCmd;
		private VM_RaportRow vM_raportRow;
		private DelegateCommand exportToExcelCmd;
		private ObservableCollection<VM_RowFilter> filters;
		private bool filtersVisible = false;
		#endregion Private fields



		#region Public Properties
		public bool FiltersVisible
		{
			get { return filtersVisible; }
			set { SetProperty(ref filtersVisible, value); RefreshFiltered(); }
		}

		public bool ControlsEnabled
		{
			get { return controlsEnabled; }
			set { SetProperty(ref controlsEnabled, value); }
		}

		public DateTime DataOd
		{
			get { return dataOd; }
			set { SetProperty(ref dataOd, value); }
		}

		public DateTime DataDo
		{
			get { return dataDo; }
			set { SetProperty(ref dataDo, value); }
		}

		public string Seria
		{
			get { return seria; }
			set { SetProperty(ref seria, value); }
		}

		public ObservableCollection<VM_RaportRow> VM_OriginalRaportRows
		{
			get { return vM_OriginalRaportRows; }
			set { SetProperty(ref vM_OriginalRaportRows, value); }
		}

		public ObservableCollection<VM_RaportRow> VM_RaportRows
		{
			get { return vM_RaportRows; }
			set { SetProperty(ref vM_RaportRows, value); }
		}

		public int RaportProgress
		{
			get { return raportProgress; }
			set { SetProperty(ref raportProgress, value); }
		}

		public string Query
		{
			get { return query; }
			set { SetProperty(ref query, value); }
		}

		public DelegateCommand LoadRaportDataCommand
		{
			get { return testCommand; }
			set { SetProperty(ref testCommand, value); }
		}
		public DelegateCommand ExportToExcelCmd
		{
			get { return exportToExcelCmd; }
			set { SetProperty(ref exportToExcelCmd, value); }
		}
		public DelegateCommand SendToDBCommand
		{
			get { return sendToDBCmd; }
			set { SetProperty(ref sendToDBCmd, value); }
		}

		public VM_RaportRow SelectedRaportRowVM
		{
			get { return vM_raportRow; }
			set { SetProperty(ref vM_raportRow, value); }
		}


		public ObservableCollection<VM_RowFilter> Filters { get => filters; set => SetProperty(ref filters, value); }

		#endregion Public Properties

	}
}
