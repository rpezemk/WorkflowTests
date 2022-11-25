using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHUP_Windykacje.ViewModel
{
    public class VM_ErrorFilterColumn : BindableBase
    {

		private int id;

		public int ID
		{
			get { return id; }
			set { SetProperty(ref id, value); }
		}


		private int errorId;

		public int ErrorID
		{
			get { return errorId; }
			set { SetProperty(ref errorId, value); }
		}


		private string tableName;

		public string TableName
		{
			get { return tableName; }
			set { SetProperty(ref tableName, value); }
		}


		private string columnName;

		public string ColumnName
		{
			get { return columnName; }
			set { SetProperty(ref columnName, value); }
		}


		private string val;

		public string Value
		{
			get { return val; }
			set { SetProperty(ref val, value); }
		}


		private bool isWildCard;

		public bool IsWildCard
		{
			get { return isWildCard; }
			set { SetProperty(ref isWildCard, value); }
		}


		private bool isRegex;

		public bool IsRegex
		{
			get { return isRegex; }
			set { SetProperty(ref isRegex, value); }
		}


    }
}
