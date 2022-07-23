using System;
using System.Collections.Generic;

namespace ProcessDocument.Model
{
    /// <summary>
    /// Zamówienie
    /// </summary>
    internal class Zamowienie
    {
        internal enum DokumentWynikowyEnum
        {
            WZ,
            FW,
            RW,
            MMW,
            PA
        }

        /// <summary>
        /// ID z kolejki GWZ
        /// </summary>
        internal int ID;
        /// <summary>
        /// GIDTyp
        /// </summary>
        internal int GIDType;
        /// <summary>
        /// GIDFirma
        /// </summary>
        internal int GIDFirm;
        /// <summary>
        /// GIDNumer
        /// </summary>
        internal int GIDNumer;
        /// <summary>
        /// Numer słowny
        /// </summary>
        internal string Numer;
        /// <summary>
        /// Seria
        /// </summary>
        internal string Seria;
        /// <summary>
        /// GIDTyp kontrahenta
        /// </summary>
        internal int KntTyp;
        /// <summary>
        /// GIDNumer kontrahenta
        /// </summary>
        internal int KntNumer;
        /// <summary>
        /// GIDTyp płatnika
        /// </summary>
        internal int KnpTyp;
        /// <summary>
        /// GIDNumer płatnika
        /// </summary>
        internal int KnpNumer;
        /// <summary>
        /// GIDTyp docelowego
        /// </summary>
        internal int KndTyp;
        /// <summary>
        /// GIDNumer docelowego
        /// </summary>
        internal int KndNumer;
        /// <summary>
        /// GIDTyp adresu wysyłkowego
        /// </summary>
        internal int AdwTyp;
        /// <summary>
        /// GIDNumer adresu wysyłkowego
        /// </summary>
        internal int AdwNumer;
        /// <summary>
        /// Nazwa formy płatności
        /// </summary>
        internal string FormaPlatnosci;
        /// <summary>
        /// Numer formy płatności
        /// </summary>
        internal int FormaNr;
        /// <summary>
        /// Termin płatności w dniach
        /// </summary>
        internal int TerminPlatnosci;
        /// <summary>
        /// Wartość zamówienia
        /// </summary>
        internal decimal Wartosc;
        /// <summary>
        /// Czy zamówienie Kemonowe.
        /// 0 - zwykłe, 1 - Kemon, 2 - SkinCare
        /// </summary>
        internal int Segment;
        /// <summary>
        /// Wykorzystanie limitu kredytowego kontrahenta
        /// </summary>
        internal decimal WykorzystanieLimitu;
        /// <summary>
        /// Płatności po terminie
        /// </summary>
        internal decimal PoTerminie;
        /// <summary>
        /// Limit kredytowy kontrahenta
        /// </summary>
        internal decimal LimitKredytowy;
        /// <summary>
        /// Opis
        /// </summary>
        internal string Opis;
        /// <summary>
        /// ZaN_Aktywny
        /// </summary>
        internal int Aktywny;
        /// <summary>
        /// ID centrum właściciela
        /// </summary>
        internal int FrsID;
        /// <summary>
        /// GIDNumer rejestru kasowo/bankowego
        /// </summary>
        internal int KarNumer;
        /// <summary>
        /// Lista pozycji
        /// </summary>
        internal List<Pozycja> Pozycje;
        /// <summary>
        /// Flaga netto/brutto
        /// </summary>
        internal string FlagaNB;
        /// <summary>
        /// Stan
        /// </summary>
        internal int Stan;
        /// <summary>
        /// Status przetwarzania
        /// </summary>
        internal int Status;
        /// <summary>
        /// Rodzaj transakcji
        /// </summary>
        internal int ExpoNorm;
        /// <summary>
        /// Sposób dostawy
        /// </summary>
        internal string SposobDostawy;
        /// <summary>
        /// Akwizytor
        /// </summary>
        internal string Akwizytor;
        /// <summary>
        /// Nazwa centrum właściciela
        /// </summary>
        internal string FrsNazwa;
        /// <summary>
        /// ZaN_Rodzaj
        /// </summary>
        internal int Rodzaj;
        /// <summary>
        /// Kod magayznu źródłowego
        /// </summary>
        internal string MagZ;
        /// <summary>
        /// Kod magazynu docelowego
        /// </summary>
        internal string MagD;
        /// <summary>
        /// Wartość netto zamówienia
        /// </summary>
        internal decimal Netto;
        /// <summary>
        /// Dokument wynikowy
        /// </summary>
        internal DokumentWynikowyEnum DokumentWynikowy = DokumentWynikowyEnum.WZ;
        /// <summary>
        /// Waluta
        /// </summary>
        internal string Waluta;
        /// <summary>
        /// Mianownik kursu
        /// </summary>
        internal decimal KursM;
        /// <summary>
        /// Licznik kursu
        /// </summary>
        internal decimal KursL;
        /// <summary>
        /// GIDNumer operatora wysyłającego
        /// </summary>
        internal int OpeNumer;
        /// <summary>
        /// Data realizacji
        /// </summary>
        internal DateTime DataRealizacji;
        /// <summary>
        /// Kod paczkomatu z atrybutu
        /// </summary>
        internal string Paczkomat;
        /// <summary>
        /// Dodatkowa płatność z atrybutu "INT - Dodatkowa płatność"
        /// </summary>
        internal decimal DodatkowaPlatnosc;
        /// <summary>
        /// Czy zamówienie na przedpłatę
        /// </summary>
        internal bool Przedplata;
        /// <summary>
        /// Atrybut "Przeznaczenie RW"
        /// </summary>
        internal string AtrPrzeznaczeniaRW;
        /// <summary>
        /// Atrybut "MPK księgowe"
        /// </summary>
        internal string AtrMPKKsiegowe;
        internal string IncotermsMiejsce;
        internal string IncotermsSymbol;
        /// <summary>
        /// Nr ceny początkowej
        /// </summary>
        internal int CenaSpr;
        /// <summary>
        /// Data, w której GWZ ma realizować zamówienie (zwykle 1 dzień przed faktyczną datą realizacji ZS, może być wcześniej jeśli dni wolne)
        /// </summary>
        internal DateTime? RealDate;
        internal string KodRodzajuTransportu;
        internal string KodRodzajuTransakcji;
        internal string IdoSellPrzedplata;
        internal DateTime? DataOdroczenia;
        /// <summary>
        /// Płatności
        /// </summary>
        internal List<XLAPI_Wrapper.ZamPlat> Platnosci = new List<XLAPI_Wrapper.ZamPlat>();
    }

    internal class Pozycja
    {
        /// <summary>
        /// GIDTyp
        /// </summary>
        internal int GIDTyp;
        /// <summary>
        /// GIDFirma
        /// </summary>
        internal int GIDFirma;
        /// <summary>
        /// GIDNumer
        /// </summary>
        internal int GIDNumer;
        /// <summary>
        /// GIDLp
        /// </summary>
        internal int GIDLp;
        /// <summary>
        /// GIDNumer towaru
        /// </summary>
        internal int TwrGIDNumer;
        /// <summary>
        /// Kod towaru
        /// </summary>
        internal string TwrKod;
        /// <summary>
        /// Ilość
        /// </summary>
        internal decimal Ilosc;
        /// <summary>
        /// Cena
        /// </summary>
        internal decimal Cena;
        /// <summary>
        /// ID promocji pakietowej
        /// </summary>
        internal int PakietId;
        /// <summary>
        /// Czy pozycja jest gratisem
        /// </summary>
        internal int Gratis;
        /// <summary>
        /// ID progu promocji
        /// </summary>
        internal int PromocjaProgId;
        /// <summary>
        /// Nazwa promocji pakietowej
        /// </summary>
        internal string Promocja;
        /// <summary>
        /// Rabat
        /// </summary>
        internal decimal Rabat;
        /// <summary>
        /// Nr cennika
        /// </summary>
        internal int CenaSpr;
        /// <summary>
        /// Wartość ceny początkowej
        /// </summary>
        internal decimal CenaP;
        internal string BudzetPrmID;
        internal string BudzetID;
        internal string BudzetWartosc;
        internal bool Nagroda;
    }
}
