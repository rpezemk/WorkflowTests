namespace XLAPI_Wrapper
{
    internal class ZamPoz : ProcessDocument.ZamPoz
    {
        public int GIDTyp { get; set; }
        public int GIDFirma { get; set; }
        public int GIDNumer { get; set; }
        public int TwrTyp { get; set; }
        public int TwrFirma { get; set; }
        public int TwrNumer { get; set; }
        public decimal Ilosc { get; set; }
        public decimal Cena { get; set; }
        public int CenaPoczatkowa { get; set; }
    }
}