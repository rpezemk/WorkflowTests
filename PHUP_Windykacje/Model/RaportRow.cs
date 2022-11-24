using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PHUP_Windykacje.Model
{
    public class RaportRow
    {
        public int Lp= 0;
        public string KntAronim  = "";
        public string KntNazwa = "";
        public string  NrDok= "";
        public decimal ValueBrut;
        public DateTime DataWyst;
        public DateTime TerminPlat;
        public string PhOpiekun = "";
        public string PhEmail = "";
        public string Opis = "";
        public bool PreventExport;
    }
}
