using System;
using System.Collections.Generic;

namespace ProcessDocument.Model
{
    /// <summary>
    /// Dokument
    /// </summary>
    class Document
    {
        /// <summary>
        /// Nagłówek dokumentu
        /// </summary>
        internal protected DocumentHead Head;
        /// <summary>
        /// Pozycje dokumentu
        /// </summary>
        internal protected List<DocumentPos> Positions;

        public Document()
        {
            Head = new DocumentHead();
            Positions = new List<DocumentPos>();
        }
    }

    /// <summary>
    /// Nagłowek dokumentu
    /// </summary>
    internal class DocumentHead
    {
        /// <summary>
        /// GIDTyp dokumentu
        /// </summary>
        protected internal int GIDType;

        /// <summary>
        /// GIDFirma dokumentu
        /// </summary>
        protected internal int GIDFirm;

        /// <summary>
        /// GIDNumer dokumentu
        /// </summary>
        protected internal int GIDNumber;

        /// <summary>
        /// GIDLp dokumentu
        /// </summary>
        protected internal int GIDLp;

        /// <summary>
        /// Numer dokumentu
        /// </summary>
        protected internal string DocNumber;

        /// <summary>
        /// Seria dokumentu
        /// </summary>
        protected internal string Serie;

        /// <summary>
        /// Klient na dokumencie
        /// </summary>
        protected internal string Client;

        /// <summary>
        /// GIDTyp kontrahenta
        /// </summary>
        protected internal int KntType;

        /// <summary>
        /// GIDFirma kontrahenta
        /// </summary>
        protected internal int KntFirm;

        /// <summary>
        /// GIDNumer kontrahenta
        /// </summary>
        protected internal int KntNumber;

        /// <summary>
        /// GIDLp kontrahenta
        /// </summary>
        protected internal int KntLp;

        /// <summary>
        /// GIDTyp płatnika
        /// </summary>
        protected internal int KnpType;

        /// <summary>
        /// GIDFirma płatnika
        /// </summary>
        protected internal int KnpFirm;

        /// <summary>
        /// GIDNumer płatnika
        /// </summary>
        protected internal int KnpNumber;

        /// <summary>
        /// GIDLp płatnika
        /// </summary>
        protected internal int KnpLp;

        /// <summary>
        /// GIDTyp odbiorcy
        /// </summary>
        protected internal int KndType;

        /// <summary>
        /// GIDFirma odbiorcy
        /// </summary>
        protected internal int KndFirm;

        /// <summary>
        /// GIDNumer odbiorcy
        /// </summary>
        protected internal int KndNumber;

        /// <summary>
        /// GIDLp odbiorcy
        /// </summary>
        protected internal int KndLp;

        /// <summary>
        /// GIDTyp adresu
        /// </summary>
        protected internal int AdwType;

        /// <summary>
        /// GIDFirma adresu
        /// </summary>
        protected internal int AdwFirm;

        /// <summary>
        /// GIDNumer adresu
        /// </summary>
        protected internal int AdwNumber;

        /// <summary>
        /// GIDLp adresu
        /// </summary>
        protected internal int AdwLp;

        /// <summary>
        /// Czy transakcje z kontrahentem wstrzymane
        /// </summary>
        protected internal bool ClientBlocked;

        /// <summary>
        /// Magazyn źródłowy
        /// </summary>
        protected internal Store SourceStore;

        /// <summary>
        /// Magazyn docelowy
        /// </summary>
        protected internal Store DestStore;

        /// <summary>
        /// Waluta dokumentu
        /// </summary>
        protected internal string Currency;

        /// <summary>
        /// Licznik kursu waluty
        /// </summary>
        protected internal decimal CurrencyL;

        /// <summary>
        /// Mianownik kursu waluty
        /// </summary>
        protected internal decimal CurrencyM;

        /// <summary>
        /// Data dokumentu
        /// </summary>
        protected internal DateTime Date;

        /// <summary>
        /// GIDTyp zamówienia źródłowego
        /// </summary>
        protected internal int ZamType;

        /// <summary>
        /// GIDFirma zamówienia źródłowego
        /// </summary>
        protected internal int ZamFirm;

        /// <summary>
        /// GIDNumer zamówienia źródłowego
        /// </summary>
        protected internal int ZamNumber;

        /// <summary>
        /// GIDLp zamówienia źródłowego
        /// </summary>
        protected internal int ZamLp;

        /// <summary>
        /// ExpoNorm dokumentu
        /// </summary>
        protected internal int ExpoNorm;

        /// <summary>
        /// FRSID dokumentu
        /// </summary>
        protected internal int FRSId;

        /// <summary>
        /// Dokument obcy dokumentu
        /// </summary>
        protected internal string ForeignNumber;

        /// <summary>
        /// Opis dokumentu
        /// </summary>
        protected internal string Description;

        /// <summary>
        /// GIDTyp korekty
        /// </summary>
        protected internal int ZwrType;

        /// <summary>
        /// GIDFirma korekty
        /// </summary>
        protected internal int ZwrFirm;

        /// <summary>
        /// GIDNumer korekty
        /// </summary>
        protected internal int ZwrNumber;

        /// <summary>
        /// GIDLp korekty
        /// </summary>
        protected internal int ZwrLp;

        /// <summary>
        /// Nr formy płatności
        /// </summary>
        protected internal int PaymentMethod;

        /// <summary>
        /// Nr rejestru bankowego
        /// </summary>
        protected internal int KarNumer;

        protected internal string FlagaNB;
        /// <summary>
        /// Termin płatności w dniach
        /// </summary>
        protected internal int PaymentTerm;
        /// <summary>
        /// Sposób dostawy
        /// </summary>
        protected internal string SposobDostawy;
        /// <summary>
        /// Akwizytor
        /// </summary>
        protected internal string Akwizytor;
        /// <summary>
        /// GIDTyp akwizytora
        /// </summary>
        protected internal int AkwTyp;
        /// <summary>
        /// GIDFirma akwizytora
        /// </summary>
        protected internal int AkwFirma;
        /// <summary>
        /// GIDNumer akwizytora
        /// </summary>
        protected internal int AkwNumer;
        /// <summary>
        /// GIDLp akwizytora
        /// </summary>
        protected internal int AkwLp;
        internal string IncotermsMiejsce;
        internal string IncotermsSymbol;
        /// <summary>
        /// Nr ceny początkowej
        /// </summary>
        internal int CenaSpr;
        internal string KodRodzajuTransportu;
        internal string KodRodzajuTransakcji;

        public DocumentHead()
        {
            DocNumber = "";
            Serie = "";
            Client = "";
            Currency = "";
            ForeignNumber = "";
            Description = "";
            SourceStore = new Store();
            DestStore = new Store();
            Date = new DateTime(1800, 12, 28);
        }
    }

    /// <summary>
    /// Pozycja dokumentu
    /// </summary>
    class DocumentPos
    {
        /// <summary>
        /// GIDTyp pozycji
        /// </summary>
        internal protected int GIDType;
        /// <summary>
        /// GIDFirma pozycji
        /// </summary>
        internal protected int GIDFirm;
        /// <summary>
        /// GIDNumer pozycji
        /// </summary>
        internal protected int GIDNumber;
        /// <summary>
        /// GIDLp pozycji
        /// </summary>
        internal protected int GIDLp;
        /// <summary>
        /// Numer pozycji
        /// </summary>
        internal protected int PositionNr;
        /// <summary>
        /// GIDNumer towaru
        /// </summary>
        internal protected int TwrNumber;
        /// <summary>
        /// Kod towaru
        /// </summary>
        internal protected string TwrCode;
        /// <summary>
        /// EAN towaru
        /// </summary>
        internal protected string TwrEan;
        /// <summary>
        /// Typ towaru
        /// </summary>
        internal protected int TwrType;
        /// <summary>
        /// Nazwa towaru
        /// </summary>
        internal protected string TwrName;
        /// <summary>
        /// Jednostka towaru
        /// </summary>
        internal protected string TwrJm;
        /// <summary>
        /// Ilość na pozycji
        /// </summary>
        internal protected decimal Quantity;
        /// <summary>
        /// Cena towaru
        /// </summary>
        internal protected decimal Price;
        /// <summary>
        /// Nr cennika
        /// </summary>
        internal protected int StartPrice;
        /// <summary>
        /// Wartość netto
        /// </summary>
        internal protected decimal NetValue;
        /// <summary>
        /// Rabat pozycji
        /// </summary>
        internal protected decimal Rebate;
        /// <summary>
        /// Grupa podatku Vat
        /// </summary>
        internal protected string Vat;
        /// <summary>
        /// Stawka podatku Vat
        /// </summary>
        internal protected decimal VatQuota;
        /// <summary>
        /// Magazyn
        /// </summary>
        internal protected string Store;
        /// <summary>
        /// ID promocji pakietowej
        /// </summary>
        internal protected int PakietId;
        /// <summary>
        /// Czy towar jest gratisem
        /// </summary>
        internal protected int Gratis;
        /// <summary>
        /// ID progu promocji
        /// </summary>
        internal protected int PromocjaProgId;
        /// <summary>
        /// Flaga Vat
        /// </summary>
        internal protected int FlagaVat;
        /// <summary>
        /// Waluta
        /// </summary>
        internal protected string Currency;
        /// <summary>
        /// Licznik kursu waluty
        /// </summary>
        internal protected decimal CurrencyL;
        /// <summary>
        /// Mianownik kursu waluty
        /// </summary>
        internal protected decimal CurrencyM;
        /// <summary>
        /// GIDTyp zlecenie
        /// </summary>
        internal protected int ZlcType;
        /// <summary>
        /// GIDFirma zlecenia
        /// </summary>
        internal protected int ZlcFirm;
        /// <summary>
        /// GIDNumer zlecenia
        /// </summary>
        internal protected int ZlcNumber;
        /// <summary>
        /// GIDLp zlecenia
        /// </summary>
        internal protected int ZlcLp;
        /// <summary>
        /// Cena początkowa
        /// </summary>
        internal protected decimal CenaP;
        internal protected string BudzetPrmID;
        internal protected string BudzetID;
        internal protected string BudzetWartosc;
        internal protected bool Nagroda;

        public DocumentPos()
        {
            TwrCode = "";
            TwrEan = "";
            TwrName = "";
            TwrJm = "";
            Vat = "";
            Store = "";
        }
    }

    /// <summary>
    /// Magazyn
    /// </summary>
    class Store
    {
        /// <summary>
        /// GIDNumer magazynu
        /// </summary>
        internal protected int GIDNumber;
        /// <summary>
        /// Kod Magazynu
        /// </summary>
        internal protected string Code;
    }
}
