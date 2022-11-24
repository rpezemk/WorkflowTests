using PHUP_Windykacje.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHUP_Windykacje.ViewModel
{
    public class VM_RaportRow : BindableBase
    {
        public VM_RaportRow(RaportRow _raportRow)
        {
            RaportRow = _raportRow;
        }

        internal Model.RaportRow RaportRow;

        public int Lp
        {
            get { return RaportRow.Lp; }
            set { SetProperty(ref RaportRow.Lp, value); }
        }


        [ColumnCaption("Akronim")]
        public string KntAkronim
        {
            get { return RaportRow.KntAronim; }
            set { SetProperty(ref RaportRow.KntAronim, value); }
        }


        [ColumnCaption("Nazwa")]
        public string KntNazwa
        {
            get { return RaportRow.KntNazwa; }
            set { SetProperty(ref RaportRow.KntNazwa, value); }
        }

        [ColumnCaption("Numer dok.")]
        public string NrDok
        {
            get { return RaportRow.NrDok; }
            set { SetProperty(ref RaportRow.NrDok, value); }
        }


        [ColumnCaption("Wartość brutto")]
        public decimal ValueBrut
        {
            get { return RaportRow.ValueBrut; }
            set { SetProperty(ref RaportRow.ValueBrut, value); }
        }


        [ColumnCaption("Data wyst.")]
        public DateTime DataWyst
        {
            get { return RaportRow.DataWyst; }
            set { SetProperty(ref RaportRow.DataWyst, value); }
        }


        [ColumnCaption("Termin płat.")]
        public DateTime TerminPlat
        {
            get { return RaportRow.TerminPlat; }
            set { SetProperty(ref RaportRow.TerminPlat, value); }
        }

        [ColumnCaption("Opiekun")]
        public string PhOpiekun
        {
            get { return RaportRow.PhOpiekun; }
            set { SetProperty(ref RaportRow.PhOpiekun, value); }
        }


        [ColumnCaption("Email op.")]
        public string PhEmail
        {
            get { return RaportRow.PhEmail; }
            set { SetProperty(ref RaportRow.PhEmail, value); }
        }



        [ColumnCaption("Opis")]
        public string Opis
        {
            get { return RaportRow.Opis; }
            set { SetProperty(ref RaportRow.Opis, value); }
        }


        public bool PreventExport
        {
            get { return RaportRow.PreventExport; }
            set { SetProperty(ref RaportRow.PreventExport, value); }
        }
    }
}
