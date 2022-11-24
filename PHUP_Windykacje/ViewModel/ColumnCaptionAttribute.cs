using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHUP_Windykacje.ViewModel
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class ColumnCaptionAttribute : Attribute
    {
        readonly string positionalString;
        public ColumnCaptionAttribute(string positionalString)
        {
            this.positionalString = positionalString;
        }

        public string PositionalString
        {
            get { return positionalString; }
        }

        public int NamedInt { get; set; }
    }
}
