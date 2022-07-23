using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTests
{
    public static class DocOperations
    {

//        public static void PrzetworzZam(Model.Zamowienie zam, List<RejestrPH> rejestryPH, out bool czekaNaWspolne)
//        {
//            var twrKodyBlokujaceWMS = new List<string> { "0109970", "0109971", "0109972", "0109973", "0109974", "0109975", "0109976", "0109977", "0109978", "0109979", };
//            var oddzialy = new List<string> { "Sprzedaż FLK.Oddziały", "Sprzedaż FLK.Alpinus" };
//            var docId = 0;
//            var zsKar = 0;
//            var braki = false;
//            var brakiPozycje = "";
//            var uslugaTransportu = false;
//            czekaNaWspolne = false;

//            RejestrPH rejestr = null;
//            var seria = zam.Seria;

//            if (zam.Aktywny > 0)
//                throw new Exception($"Dokument jest edytowany przez operatora (ID Sesji:{zam.Aktywny})!");

//            if (zam.Rodzaj == ZamRodzaj.Zewn)
//            {
//                if (zam.FrsNazwa.StartsWithAnyOf(oddzialy) || zam.Segment > 0 || zam.Seria == "EC")
//                {
//                    while (seria.Length > 0)
//                    {
//                        foreach (var r in rejestryPH.Where(r => r.Rejestr.ToUpper() == seria.ToUpper()))
//                        {
//                            rejestr = r;
//                            break;
//                        }

//                        if (rejestr != null)
//                            break;

//                        seria = seria.Length > 1 ? seria.Substring(0, seria.Length - 1) : "";
//                    }

//                    if (rejestr != null)
//                    {
//                        SetLogText("Znaleziono rejestr " + rejestr.Rejestr + ", próg MIN:" + rejestr.Prog + "  MAX:" + rejestr.ProgMax);
//                        if (zam.Wartosc < rejestr.Prog)
//                        {
//                            SetLogText("Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") poniżej progu " + rejestr.Prog.ToString("0.00"));
//                            if (Properties.Settings.Default.INTXLID == 1 && zam.Seria.ToUpper().Trim() != "EC" && _transportTwrNumer > 0)
//                            {
//                                SetLogText("Zostanie dodany koszt transortu do ZS");
//                                uslugaTransportu = true;
//                            }
//                            else
//                            {

//                                SQL.AktualizujBlad(zam.ID, "Błąd: Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") poniżej progu " + rejestr.Prog.ToString("0.00"));
//                                return;
//                            }

//                        }

//                        if (rejestr.ProgMax > 0 && zam.Wartosc > rejestr.ProgMax)
//                        {
//                            SetLogText("Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") powyżej progu " + rejestr.ProgMax.ToString("0.00"));
//                            SQL.AktualizujBlad(zam.ID, "Błąd: Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") powyżej progu " + rejestr.ProgMax.ToString("0.00"));
//                            return;
//                        }
//                    }
//                    else
//                    {
//                        if (zam.Wartosc < progZS && zam.Segment == 0)
//                        {
//                            SetLogText("Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") poniżej progu " + progZS.ToString("0.00"));
//                            if (Properties.Settings.Default.INTXLID == 1 && zam.Seria.ToUpper().Trim() != "EC" && _transportTwrNumer > 0)
//                            {
//                                SetLogText("Zostanie dodany koszt transortu do ZS");
//                                uslugaTransportu = true;
//                            }
//                            else
//                            {
//                                SQL.AktualizujBlad(zam.ID, "Błąd: Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") poniżej progu " + progZS.ToString("0.00"));
//                                return;
//                            }
//                        }
//                        else if (zam.Wartosc < progZSKemon && (zam.Segment == 1 || zam.Segment == 2))
//                        {
//                            SetLogText("Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") poniżej progu " + progZSKemon.ToString("0.00"));
//                            if (Properties.Settings.Default.INTXLID == 1 && zam.Seria.ToUpper().Trim() != "EC")
//                            {
//                                SetLogText("Zostanie dodany koszt transortu do ZS");
//                                uslugaTransportu = true;
//                            }
//                            else
//                            {
//                                SQL.AktualizujBlad(zam.ID, "Błąd: Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") poniżej progu " + progZSKemon.ToString("0.00"));
//                                return;
//                            }
//                        }

//                        if (zam.Segment == 0 && progZSMax > 0 && zam.Wartosc > progZSMax)
//                        {
//                            SetLogText("Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") powyżej progu " + progZSMax.ToString("0.00"));
//                            SQL.AktualizujBlad(zam.ID, "Błąd: Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") powyżej progu " + progZSMax.ToString("0.00"));
//                            return;
//                        }
//                        else if ((zam.Segment == 1 || zam.Segment == 2) && progZSKemonMax > 0 && zam.Wartosc > progZSKemonMax)
//                        {
//                            SetLogText("Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") powyżej progu " + progZSKemonMax.ToString("0.00"));
//                            SQL.AktualizujBlad(zam.ID, "Błąd: Wartość zamówienia (" + zam.Wartosc.ToString("0.00") + ") powyżej progu " + progZSKemonMax.ToString("0.00"));
//                            return;
//                        }
//                    }
//                }

//                if ((zam.FormaPlatnosci == "Przelew" && zam.Segment == 0) || (zam.Segment == 1 || zam.Segment == 2))
//                {
//                    if (!SQL.SprawdzBrakKontroliLimitu(zam.OpeNumer))
//                    {
//                        if (zam.PoTerminie > 0)
//                        {
//                            SetLogText("Kontrahent z zamówienia ma przeterminowane płatności!");
//                            SQL.AktualizujBlad(zam.ID, "Błąd: Przeterminowane płatności");
//                            return;
//                        }

//                        if (zam.LimitKredytowy > 0)
//                        {
//                            if (zam.LimitKredytowy - zam.WykorzystanieLimitu < zam.Wartosc)
//                            {
//                                SetLogText("Limit kredytowy kontrahenta nie wystarcza na zrealizowane zamówienia!");
//                                SQL.AktualizujBlad(zam.ID, "Błąd: Limit kredytowy kontrahenta nie wystarcza na zrealizowane zamówienia.");
//                                return;
//                            }
//                        }
//                    }
//                }

//                if (Properties.Settings.Default.INTXLID == 1)
//                {
//                    SetLogText("Sprawdzenie promocji");
//                    if (!SQL.SprawdzPromocje(zam.GIDNumer, out var error))
//                    {
//                        SetLogText(error);
//                        SQL.AktualizujBlad(zam.ID, "Błąd: " + error);
//                        return;
//                    }
//                }
//            }

//            var niepelnePozyje = zam.Pozycje.Where(p => p.Ilosc - Math.Round(p.Ilosc, 0) != 0).Aggregate("", (current, p) => current + ("Lp. " + p.GIDLp + " Towar " + p.TwrKod + " Ilość: " + p.Ilosc.ToString("0.00") + Environment.NewLine));
//            if (!string.IsNullOrEmpty(niepelnePozyje))
//            {
//                SetLogText("Na ZS występują pozycje z niepełną ilością:" + Environment.NewLine + niepelnePozyje);
//                SQL.AktualizujBlad(zam.ID, "Błąd: Na ZS występują pozycje z niepełną ilością" + Environment.NewLine + niepelnePozyje);
//            }

//            var doc = new Document();
//            bool zmienFrsIdSQLem = false;
//            int frsId = 0;
//            if (zam.Rodzaj == ZamRodzaj.Zewn)
//            {
//                doc.Head.GIDFirm = zam.GIDFirm;
//                if (Properties.Settings.Default.INTXLID == 1 && zam.SposobDostawy.ToLower() == "paragon")
//                {
//                    SQL.SetAttrValue(zam.GIDType, zam.GIDFirm, zam.GIDNumer, 0, "GWZ - Dokument wynikowy", "PA", "");
//                    zam.DokumentWynikowy = Model.Zamowienie.DokumentWynikowyEnum.PA;
//                }

//                switch (zam.DokumentWynikowy)
//                {
//                    case Model.Zamowienie.DokumentWynikowyEnum.WZ:
//                        doc.Head.GIDType = zam.FrsNazwa.Contains("Eksport") ? 2005 : 2001;
//                        break;
//                    case Model.Zamowienie.DokumentWynikowyEnum.FW:
//                        doc.Head.GIDType = 2036;
//                        break;
//                    case Model.Zamowienie.DokumentWynikowyEnum.RW:
//                        doc.Head.GIDType = 1616;
//                        break;
//                    case Model.Zamowienie.DokumentWynikowyEnum.PA:
//                        doc.Head.GIDType = 2034;
//                        break;
//                    default:
//                        doc.Head.GIDType = zam.FrsNazwa.Contains("Eksport") ? 2005 : 2001;
//                        break;
//                }

//                if (doc.Head.GIDType == 2034 && zam.FlagaNB.ToLower() == "n")
//                {
//                    SetLogText("Zamówienie ma flagę liczenia VAT od netto - nie można wygenerować PA");
//                    SQL.AktualizujBlad(zam.ID, "Zamówienie ma flagę liczenia VAT od netto i nie można wygenerować PA - proszę zmienić flagę na zamówieniu!");
//                    return;
//                }
//                else if ((doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005) && zam.FlagaNB.ToLower() == "b")
//                {
//                    SetLogText("Zamówienie ma flagę liczenia VAT od brutto - nie można wygenerować WZ");
//                    SQL.AktualizujBlad(zam.ID, "Zamówienie ma flagę liczenia VAT od brutto i nie można wygenerować WZ - proszę zmienić flagę na zamówieniu!");
//                    return;
//                }

//                doc.Head.SourceStore.Code = Properties.Settings.Default.MagazynRealizacji;
//                doc.Head.KntType = zam.KntTyp;
//                doc.Head.KntFirm = zam.GIDFirm;
//                doc.Head.KntNumber = zam.KntNumer;
//                doc.Head.KntLp = 0;
//                doc.Head.KnpType = zam.KnpTyp;
//                doc.Head.KnpFirm = zam.GIDFirm;
//                doc.Head.KnpNumber = zam.KnpNumer;
//                doc.Head.KnpLp = 0;
//                doc.Head.KndType = zam.KndTyp;
//                doc.Head.KndFirm = zam.GIDFirm;
//                doc.Head.KndNumber = zam.KndNumer;
//                doc.Head.KntLp = 0;
//                doc.Head.AdwType = zam.AdwTyp;
//                doc.Head.AdwFirm = zam.GIDFirm;
//                doc.Head.AdwNumber = zam.AdwNumer;
//                doc.Head.AdwLp = 0;
//                if (doc.Head.GIDType == 2005)
//                {
//                    doc.Head.Currency = zam.Waluta;
//                    doc.Head.CurrencyM = zam.KursM;
//                    doc.Head.CurrencyM = zam.KursL;
//                    doc.Head.IncotermsMiejsce = zam.IncotermsMiejsce;
//                    doc.Head.IncotermsSymbol = zam.IncotermsSymbol;
//                    doc.Head.KodRodzajuTransportu = zam.KodRodzajuTransportu;
//                    doc.Head.KodRodzajuTransakcji = zam.KodRodzajuTransakcji;
//                }
//                if (doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005 || doc.Head.GIDType == 2036 || doc.Head.GIDType == 2034)
//                {
//                    if (Properties.Settings.Default.INTXLID == 1)
//                    {
//                        if (zam.FrsNazwa.StartsWithAnyOf(oddzialy) || zam.Seria == "EC")
//                            doc.Head.PaymentMethod = (zam.FormaNr == 10 || zam.FormaNr == 11 || zam.FormaNr == 17 || zam.FormaNr == 50) ? 21 : zam.FormaNr;
//                        else
//                            doc.Head.PaymentMethod = zam.FormaNr;
//                    }
//                    else
//                        doc.Head.PaymentMethod = zam.FormaNr;

//                    doc.Head.PaymentTerm = zam.TerminPlatnosci;
//                    if (zam.DataOdroczenia.HasValue && doc.Head.GIDType == 2001)
//                        doc.Head.PaymentTerm += zam.DataOdroczenia.Value.Date.Subtract(DateTime.Now.Date).Days;
//                }

//                if (doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005 || doc.Head.GIDType == 2036 || doc.Head.GIDType == 2034)
//                {
//                    if (zam.SposobDostawy.ToLower().Contains("gls"))
//                    {
//                        SetLogText("Sprawdzenie promocji dotyczących terminu płatności");
//                        var terminPromo = SQL.PobierzTerminPlZPromocji(zam.GIDNumer);
//                        if (terminPromo > -1)
//                        {
//                            SetLogText($"Pobrano termin z promocji: {terminPromo}");
//                            doc.Head.PaymentTerm = terminPromo;
//                            doc.Head.PaymentMethod = 22;

//                            if (zam.DataOdroczenia.HasValue && doc.Head.GIDType == 2001)
//                                doc.Head.PaymentTerm += zam.DataOdroczenia.Value.Date.Subtract(DateTime.Now.Date).Days;
//                        }
//                    }

//                    doc.Head.ExpoNorm = zam.ExpoNorm <= 2 ? zam.ExpoNorm + 1 : zam.ExpoNorm;
//                }

//                if (doc.Head.GIDType == 2036 || doc.Head.GIDType == 1616)
//                {
//                    doc.Head.Serie = zam.Seria;
//                    doc.Head.FRSId = zam.FrsID;
//                }
//                else
//                {
//                    if (Properties.Settings.Default.INTXLID == 1)
//                    {
//                        if (zam.Seria.ToUpper() == "EC" && SQL.SprawdzCzyKlientToSalon(zam.GIDNumer))
//                        {
//                            var ecParams = SQL.PobierzParametryDlaSeriiEC(zam.GIDNumer);
//                            doc.Head.Serie = "ECF";
//                            frsId = ecParams.FrsId;
//                            zmienFrsIdSQLem = true;
//                            doc.Head.FRSId = Properties.Settings.Default.DomyslneFrsIdWZ;
//                            doc.Head.AkwTyp = ecParams.AkwTyp;
//                            doc.Head.AkwFirma = ecParams.AkwFirma;
//                            doc.Head.AkwNumer = ecParams.AkwNumer;
//                            doc.Head.AkwLp = ecParams.AkwLp;
//                        }
//                        else if (zam.Seria.ToUpper() == "FLK")
//                        {
//                            doc.Head.Serie = "FLK";
//                            doc.Head.FRSId = Properties.Settings.Default.FlkFrsId;
//                        }
//                        else if (zam.Seria.ToUpper() == "CBF")
//                        {
//                            doc.Head.Serie = "CBF";
//                            doc.Head.FRSId = zam.FrsID;
//                            doc.Head.Akwizytor = zam.Akwizytor;
//                        }
//                        else
//                        {
//                            if (zam.Segment == 0)
//                            {
//                                if (zam.FrsNazwa.StartsWithAnyOf(oddzialy))
//                                {
//                                    doc.Head.Serie = Properties.Settings.Default.DomyslnaSeriaWZ;
//                                    doc.Head.FRSId = Properties.Settings.Default.DomyslneFrsIdWZ;
//                                }
//                                else
//                                {
//                                    doc.Head.Serie = zam.Seria;
//                                    doc.Head.FRSId = zam.FrsID;
//                                }
//                            }
//                            else if (zam.Segment == 1)
//                            {
//                                doc.Head.Serie = zam.Seria;
//                                doc.Head.FRSId = Properties.Settings.Default.KemonFrsIdWZ;
//                            }
//                            else if (zam.Segment == 2)
//                            {
//                                doc.Head.Serie = zam.Seria;
//                                doc.Head.FRSId = Properties.Settings.Default.SkinCareFrsIdWZ;
//                            }
//                        }
//                    }
//                    else if (Properties.Settings.Default.INTXLID == 2)
//                    {
//                        doc.Head.Serie = Properties.Settings.Default.DomyslnaSeriaWZ;
//                        doc.Head.FRSId = Properties.Settings.Default.DomyslneFrsIdWZ;
//                    }
//                    else if (Properties.Settings.Default.INTXLID == 3)
//                    {
//                        if (zam.Seria == "EC")
//                            doc.Head.Serie = "EC";
//                        else
//                            doc.Head.Serie = Properties.Settings.Default.DomyslnaSeriaWZ;
//                        doc.Head.FRSId = Properties.Settings.Default.DomyslneFrsIdWZ;
//                    }
//                    else if (Properties.Settings.Default.INTXLID == 4)
//                    {

//                        if (zam.FrsNazwa.StartsWith("MOBILNi"))
//                        {
//                            doc.Head.Serie = "MOB";
//                            doc.Head.FRSId = Properties.Settings.Default.MOBILNiFrsId;
//                        }
//                        else
//                        {
//                            doc.Head.Serie = zam.Seria;
//                            doc.Head.FRSId = zam.FrsID;
//                        }
//                    }
//                    else if (Properties.Settings.Default.INTXLID == 5)
//                    {
//                        doc.Head.Serie = Properties.Settings.Default.DomyslnaSeriaWZ;
//                        doc.Head.FRSId = Properties.Settings.Default.DomyslneFrsIdWZ;
//                    }
//                    else
//                    {
//                        doc.Head.Serie = zam.Seria;
//                        doc.Head.FRSId = zam.FrsID;
//                    }
//                }
//                doc.Head.ZamType = zam.GIDType;
//                doc.Head.ZamFirm = zam.GIDFirm;
//                doc.Head.ZamNumber = zam.GIDNumer;
//                doc.Head.ZamLp = 0;
//                doc.Head.Description = zam.Opis;
//                doc.Head.FlagaNB = zam.FlagaNB;
//                if (zam.SposobDostawy.ToLower() == "paragon")
//                    doc.Head.SposobDostawy = "Spedycja";
//                else
//                    doc.Head.SposobDostawy = zam.SposobDostawy;
//                doc.Head.CenaSpr = zam.CenaSpr;

//                foreach (var zp in zam.Pozycje)
//                {
//                    if (doc.Head.GIDType == 1616 || doc.Head.GIDType == 2036)
//                    {
//                        var p = new DocumentPos
//                        {
//                            GIDType = doc.Head.GIDType,
//                            GIDFirm = zam.GIDFirm,
//                            TwrNumber = zp.TwrGIDNumer,
//                            TwrCode = zp.TwrKod,
//                            Quantity = zp.Ilosc,
//                            Price = zp.Cena,
//                            ZlcType = zp.GIDTyp,
//                            ZlcFirm = zp.GIDFirma,
//                            ZlcNumber = zp.GIDNumer,
//                            ZlcLp = zp.GIDLp,
//                            Rebate = zp.Rabat,
//                            Store = Properties.Settings.Default.MagazynRealizacji,
//                            BudzetID = zp.BudzetID,
//                            BudzetPrmID = zp.BudzetPrmID,
//                            BudzetWartosc = zp.BudzetWartosc
//                        };

//                        doc.Positions.Add(p);
//                    }
//                    else
//                    {
//                        var p = new DocumentPos
//                        {
//                            GIDType = doc.Head.GIDType,
//                            GIDFirm = zam.GIDFirm,
//                            TwrNumber = zp.TwrGIDNumer,
//                            TwrCode = zp.TwrKod,
//                            Quantity = zp.Ilosc,
//                            Price = zp.Cena,
//                            ZlcType = zp.GIDTyp,
//                            ZlcFirm = zp.GIDFirma,
//                            ZlcNumber = zp.GIDNumer,
//                            ZlcLp = zp.GIDLp,
//                            PakietId = zp.PakietId,
//                            Gratis = zp.Gratis,
//                            PromocjaProgId = zp.PromocjaProgId,
//                            Rebate = zp.Rabat,
//                            Store = Properties.Settings.Default.MagazynRealizacji,
//                            StartPrice = zp.CenaSpr,
//                            BudzetID = zp.BudzetID,
//                            BudzetPrmID = zp.BudzetPrmID,
//                            BudzetWartosc = zp.BudzetWartosc,
//                            CenaP = zp.CenaP,
//                            Nagroda = zp.Nagroda
//                        };

//                        doc.Positions.Add(p);
//                    }
//                }
//            }
//            else
//            {
//                doc.Head.GIDFirm = zam.GIDFirm;
//                doc.Head.GIDType = 1603;
//                doc.Head.SourceStore.Code = Properties.Settings.Default.MagazynRealizacji;
//                doc.Head.DestStore.Code = zam.MagD;


//                //doc.Head.ExpoNorm = zam.ExpoNorm;
//                doc.Head.Serie = zam.Seria;
//                doc.Head.FRSId = Properties.Settings.Default.DomyslneFrsIdWZ;
//                doc.Head.ZamType = zam.GIDType;
//                doc.Head.ZamFirm = zam.GIDFirm;
//                doc.Head.ZamNumber = zam.GIDNumer;
//                doc.Head.ZamLp = 0;
//                doc.Head.Description = zam.Opis;
//                doc.Head.SposobDostawy = zam.SposobDostawy;

//                foreach (var zp in zam.Pozycje)
//                {
//                    var p = new DocumentPos
//                    {
//                        GIDType = 1603,
//                        GIDFirm = zam.GIDFirm,
//                        TwrNumber = zp.TwrGIDNumer,
//                        TwrCode = zp.TwrKod,
//                        Quantity = zp.Ilosc,
//                        ZlcType = zp.GIDTyp,
//                        ZlcFirm = zp.GIDFirma,
//                        ZlcNumber = zp.GIDNumer,
//                        ZlcLp = zp.GIDLp,
//                        Store = Properties.Settings.Default.MagazynRealizacji
//                    };

//                    doc.Positions.Add(p);
//                }
//            }

//            List<DocumentPos> gratisy = null;

//            if (zam.Rodzaj == ZamRodzaj.Zewn && doc.Head.GIDType.In(G.WZ, G.WZE, G.PA) && Properties.Settings.Default.INTXLID == 1)
//            {
//                if (
//                    (doc.Head.Serie == "MC" && (zam.FrsNazwa.StartsWithAnyOf(oddzialy)))
//                    || zam.Seria == "EC"
//                    || (zam.Segment == 1 && zam.FrsNazwa.StartsWith("Kemon"))
//                    || doc.Head.Serie.In("MC", "RIC", "CBF")
//                    || zam.Segment == 2
//                    || zam.Seria == "EC")
//                {
//                    SetLogText("Szukanie gratisów");
//                    gratisy = SQL.PobierzGratisy(zam.GIDNumer);
//                }
//            }

//            if (zam.Stan <= 2 || zam.Stan == 4)
//            {
//                SetLogText("Potwierdzanie zamówienia");
//                try
//                {
//                    token = XLAPI.BeginTransaction(apiSession);
//                }
//                catch
//                {
//                    SetLogText("Błąd zakładania transakcji - nastąpi próba przelogowania.");

//                    XLAPI.Logout(apiSession);
//                    apiSession = XLAPI.Login("Generator WZ", Properties.Settings.Default.BazaAPI, Properties.Settings.Default.UserAPI, Properties.Settings.Default.PasswordAPI,
//                        Properties.Settings.Default.HASP, "");
//                    token = XLAPI.BeginTransaction(apiSession);
//                }

//                try
//                {
//                    var zamId = XLAPI.OpenZam(apiSession, zam.GIDType, zam.GIDFirm, zam.GIDNumer);

//                    if (gratisy != null)
//                    {
//                        foreach (var p in gratisy)
//                        {
//                            if (!zam.Pozycje.Any(a => a.TwrKod == p.TwrCode))
//                            {
//                                WystawGratis(zam, doc, zamId, p);
//                            }
//                        }
//                    }

//                    var op = SQL.PobierzPozycjeDoPelnychOp(zam.GIDNumer);
//                    if (op != null)
//                    {
//                        SetLogText("Zaokrąglanie do pełnych opakowań");
//                        foreach (var p in op)
//                        {
//                            var wzPos = doc.Positions.Find(f => f.ZlcLp == p.GIDLp);
//                            wzPos.Quantity = p.Ilosc;

//                            var pMod = p.Kopiuj();
//                            API.ModyfikujPozycjeZam(zamId, ref pMod);
//                        }
//                    }

//                    if (uslugaTransportu && !zam.Pozycje.Any(a => a.TwrGIDNumer == _transportTwrNumer))
//                    {
//                        SetLogText("Dodawanie usługi transportu");
//                        var transportPos = new XLAPI_Wrapper.ZamPoz
//                        {
//                            GIDTyp = zam.GIDType,
//                            GIDFirma = zam.GIDFirm,
//                            GIDNumer = zam.GIDNumer,
//                            TwrKod = Properties.Settings.Default.UslugaTransportu,
//                            TwrNumer = _transportTwrNumer,
//                            Ilosc = 1
//                        };
//                        var lp = API.DodajPozycjeZam(zamId, ref transportPos);

//                        var transportWZPos = new DocumentPos
//                        {
//                            GIDType = doc.Head.GIDType,
//                            GIDFirm = zam.GIDFirm,
//                            TwrCode = Properties.Settings.Default.UslugaTransportu,
//                            TwrNumber = _transportTwrNumer,
//                            Quantity = 1,
//                            Price = transportPos.Cena,
//                            ZlcType = zam.GIDType,
//                            ZlcFirm = zam.GIDFirm,
//                            ZlcNumber = zam.GIDNumer,
//                            ZlcLp = lp,
//                            Store = Properties.Settings.Default.MagazynRealizacji,
//                            StartPrice = -1
//                        };
//                        doc.Positions.Add(transportWZPos);
//                    }

//                    XLAPI.CloseZam(zamId, XLAPI.CloseZamMode.Potwierdzenie);
//                    if (!API.TransakcjaAktywna(apiSession))
//                    {
//                        TryMultipleTimes(() => SQL.AktualizujZanFrsId(zam.GIDNumer, zam.FrsID), 5);
//                        throw new Exception("Transakcja nie jest aktywna po potwierdzeniu zamówienia!");
//                    }
//                    else
//                        SQL.AktualizujZanFrsId(zam.GIDNumer, zam.FrsID, token);
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Błąd podczas potwierdzania zamówienia: " + ex.Message);
//                }

//                try
//                {
//                    SQL.ZmienMagNaRezerwacjach(zam.GIDType, zam.GIDNumer, Properties.Settings.Default.MagazynRealizacji, token);
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Błąd podczas czyszczenia rezerwacji: " + ex.Message);
//                }

//                try
//                {
//                    XLAPI.CommitTransaction(apiSession);
//                    token = "";
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Błąd zatwierdzania transakcji: " + ex.Message);
//                }
//            }
//            else if (zam.Status > -2)
//            {
//                SetLogText("Zamówienie już potwierdzone");

//                try
//                {
//                    token = XLAPI.BeginTransaction(apiSession);
//                }
//                catch
//                {
//                    SetLogText("Błąd zakładania transakcji - nastąpi próba przelogowania.");

//                    XLAPI.Logout(apiSession);
//                    apiSession = XLAPI.Login("Generator WZ", Properties.Settings.Default.BazaAPI, Properties.Settings.Default.UserAPI, Properties.Settings.Default.PasswordAPI,
//                        Properties.Settings.Default.HASP, "");
//                    token = XLAPI.BeginTransaction(apiSession);
//                }
//                if (zam.Rodzaj == ZamRodzaj.Zewn)
//                {
//                    try
//                    {
//                        var op = SQL.PobierzPozycjeDoPelnychOp(zam.GIDNumer);
//                        if (gratisy != null || op != null || uslugaTransportu)
//                        {
//                            var zamId = API.OtworzDokumentZam(apiSession, zam.GIDType, zam.GIDFirm, zam.GIDNumer, 0);
//                            API.ZamknijDokumentZam(zamId, API.TrybZamknieciaZam.Otwórz);

//                            zamId = API.OtworzDokumentZam(apiSession, zam.GIDType, zam.GIDFirm, zam.GIDNumer, 0);

//                            if (gratisy != null)
//                            {
//                                foreach (var p in gratisy)
//                                {
//                                    if (!zam.Pozycje.Any(a => a.TwrKod == p.TwrCode))
//                                    {
//                                        WystawGratis(zam, doc, zamId, p);
//                                    }
//                                }
//                            }
//                            if (op != null && !(Properties.Settings.Default.INTXLID == 1 && zam.Seria.StartsWith("REK") && doc.Head.GIDType == (int)G.RW))
//                            {
//                                SetLogText("Zaokrąglanie do pełnych opakowań");
//                                foreach (var p in op)
//                                {
//                                    var wzPos = doc.Positions.Find(f => f.ZlcLp == p.GIDLp);
//                                    wzPos.Quantity = p.Ilosc;

//                                    var pMod = p.Kopiuj();
//                                    API.ModyfikujPozycjeZam(zamId, ref pMod);
//                                }
//                            }

//                            if (uslugaTransportu && !zam.Pozycje.Any(a => a.TwrGIDNumer == _transportTwrNumer))
//                            {
//                                SetLogText("Dodawanie usługi transportu");
//                                var transportPos = new XLAPI_Wrapper.ZamPoz
//                                {
//                                    GIDTyp = zam.GIDType,
//                                    GIDFirma = zam.GIDFirm,
//                                    GIDNumer = zam.GIDNumer,
//                                    TwrKod = Properties.Settings.Default.UslugaTransportu,
//                                    TwrNumer = _transportTwrNumer,
//                                    Ilosc = 1
//                                };
//                                var lp = API.DodajPozycjeZam(zamId, ref transportPos);

//                                var transportWZPos = new DocumentPos
//                                {
//                                    GIDType = doc.Head.GIDType,
//                                    GIDFirm = zam.GIDFirm,
//                                    TwrCode = Properties.Settings.Default.UslugaTransportu,
//                                    TwrNumber = _transportTwrNumer,
//                                    Quantity = 1,
//                                    Price = transportPos.Cena,
//                                    ZlcType = zam.GIDType,
//                                    ZlcFirm = zam.GIDFirm,
//                                    ZlcNumber = zam.GIDNumer,
//                                    ZlcLp = lp,
//                                    Store = Properties.Settings.Default.MagazynRealizacji,
//                                    StartPrice = -1
//                                };
//                                doc.Positions.Add(transportWZPos);
//                            }

//                            API.ZamknijDokumentZam(zamId, API.TrybZamknieciaZam.Potwierdzenie);
//                            if (!API.TransakcjaAktywna(apiSession))
//                            {
//                                TryMultipleTimes(() => SQL.AktualizujZanFrsId(zam.GIDNumer, zam.FrsID), 5);
//                                throw new Exception("Transakcja nie jest aktywna po potwierdzeniu zamówienia!");
//                            }
//                            else
//                                SQL.AktualizujZanFrsId(zam.GIDNumer, zam.FrsID, token);
//                        }

//                        SQL.ZmienMagNaRezerwacjach(zam.GIDType, zam.GIDNumer, Properties.Settings.Default.MagazynRealizacji, token);
//                    }
//                    catch (Exception ex)
//                    {
//                        throw new Exception("Błąd: " + ex.Message);
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        var op = SQL.PobierzPozycjeDoPelnychOp(zam.GIDNumer);
//                        if (gratisy != null || op != null)
//                        {
//                            var zamId = API.OtworzDokumentZam(apiSession, zam.GIDType, zam.GIDFirm, zam.GIDNumer, 0);

//                            if (gratisy != null)
//                            {
//                                foreach (var p in gratisy)
//                                {
//                                    if (!zam.Pozycje.Any(a => a.TwrKod == p.TwrCode))
//                                    {
//                                        WystawGratis(zam, doc, zamId, p);
//                                    }
//                                }
//                            }

//                            if (op != null)
//                            {
//                                SetLogText("Zaokrąglanie do pełnych opakowań");
//                                foreach (var p in op)
//                                {
//                                    var wzPos = doc.Positions.Find(f => f.ZlcLp == p.GIDLp);
//                                    wzPos.Quantity = p.Ilosc;

//                                    var pMod = p.Kopiuj();
//                                    API.ModyfikujPozycjeZam(zamId, ref pMod);
//                                }
//                            }

//                            API.ZamknijDokumentZam(zamId, API.TrybZamknieciaZam.Potwierdzenie);
//                            if (!API.TransakcjaAktywna(apiSession))
//                            {
//                                TryMultipleTimes(() => SQL.AktualizujZanFrsId(zam.GIDNumer, zam.FrsID), 5);
//                                throw new Exception("Transakcja nie jest aktywna po potwierdzeniu zamówienia!");
//                            }
//                            else
//                                SQL.AktualizujZanFrsId(zam.GIDNumer, zam.FrsID, token);
//                        }

//                        SQL.ZmienMagNaRezerwacjach(zam.GIDType, zam.GIDNumer, Properties.Settings.Default.MagazynRealizacji, token);
//                    }
//                    catch (Exception ex)
//                    {
//                        throw new Exception("Błąd: " + ex.Message);
//                    }
//                }

//                try
//                {
//                    XLAPI.CommitTransaction(apiSession);
//                    token = "";
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Błąd zatwierdzania transakcji: " + ex.Message);
//                }
//            }

//            SetLogText("Analiza towarów wspólnych");

//            //Obsługa towarów wspólnych
//            var cadStatus = SQL.CheckCommonArticleDocsStatus(zam.ID);
//            if (cadStatus == -1) //Jeśli oczekuje na wygenerowanie dokumentów sprzedaży/zakupu z inny firm
//            {
//                SetLogText("Oczekuje na wystawienie dokumentów zakupu/sprzedaży towarów wspólnych - pomijanie.");
//                czekaNaWspolne = true;
//                return;
//            }

//            if (cadStatus == 0) //Jeśli dokumenty sprzedaży/zakupu towarów wspólnych jeszcze nie istnieją
//            {
//                var cadList = new List<CommonArticleDoc>();
//                var cadPosList = SQL.GetCommonDocPos(Properties.Settings.Default.INTXLID, zam.GIDNumer);

//                foreach (var p in doc.Positions)
//                {
//                    var ca = SQL.GetCommonArticleForTwr(p.TwrNumber);
//                    if (ca != null)
//                    {
//                        if (!ca.DisableSelling)
//                        {
//                            var cadPos = cadPosList.Find(f => f.MappedTwrNumer == p.TwrNumber);
//                            if (cadPos != null)
//                            {
//                                if (cadPos.Quantity >= p.Quantity)
//                                {
//                                    cadPos.Quantity -= p.Quantity;
//                                    continue;
//                                }
//                                else
//                                {
//                                    var orderQuantity = p.Quantity - cadPos.Quantity;
//                                    cadPos.Quantity -= orderQuantity;
//                                    if (cadPos.Quantity < 0)
//                                        cadPos.Quantity = 0;

//                                    var caStock = SQL.GetCommonArticleStock(ca, Properties.Settings.Default.MagazynRealizacji);
//                                    if (caStock > 0)
//                                    {
//                                        orderQuantity = orderQuantity > caStock ? caStock : orderQuantity;
//                                        var cad = cadList.Find(f => f.Head.DestXLID == ca.XLID && f.Head.DestCompanyID == ca.CompanyID);
//                                        if (cad == null)
//                                        {
//                                            cad = new CommonArticleDoc
//                                            {
//                                                Head =
//                                        {
//                                            DestCompanyID = ca.CompanyID,
//                                            DestXLID = ca.XLID,
//                                            SourceCompanyID = _companyId,
//                                            SourceXLID = Properties.Settings.Default.INTXLID,
//                                            SourceGIDTyp = zam.GIDType,
//                                            SourceGIDNumer = zam.GIDNumer,
//                                            SourceDocNumber = zam.Numer,
//                                            GIDTyp = 2033,
//                                            Export = doc.Head.GIDType == 2005
//                                        }
//                                            };
//                                            cadList.Add(cad);
//                                        }

//                                        var cap = cad.Pos.Find(f => f.CommonTwrNumer == ca.TwrNumer && f.MappedTwrNumer == p.TwrNumber);
//                                        if (cap == null)
//                                        {
//                                            cad.Pos.Add(new CommonArticleDoc.CommonArticleDocPos
//                                            {
//                                                CommonTwrNumer = ca.TwrNumer,
//                                                CommonTwrCode = ca.TwrCode,
//                                                MappedTwrNumer = p.TwrNumber,
//                                                MappedTwrCode = p.TwrCode,
//                                                Quantity = orderQuantity
//                                            });
//                                        }
//                                        else
//                                            cap.Quantity += orderQuantity;
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                var orderQuantity = p.Quantity;
//                                var caStock = SQL.GetCommonArticleStock(ca, Properties.Settings.Default.MagazynRealizacji);
//                                if (caStock > 0)
//                                {
//                                    orderQuantity = orderQuantity > caStock ? caStock : orderQuantity;
//                                    var cad = cadList.Find(f => f.Head.DestXLID == ca.XLID && f.Head.DestCompanyID == ca.CompanyID);
//                                    if (cad == null)
//                                    {
//                                        cad = new CommonArticleDoc
//                                        {
//                                            Head =
//                                        {
//                                            DestCompanyID = ca.CompanyID,
//                                            DestXLID = ca.XLID,
//                                            SourceCompanyID = _companyId,
//                                            SourceXLID = Properties.Settings.Default.INTXLID,
//                                            SourceGIDTyp = zam.GIDType,
//                                            SourceGIDNumer = zam.GIDNumer,
//                                            SourceDocNumber = zam.Numer,
//                                            GIDTyp = 2033,
//                                            Export = doc.Head.GIDType == 2005
//                                        }
//                                        };
//                                        cadList.Add(cad);
//                                    }

//                                    var cap = cad.Pos.Find(f => f.CommonTwrNumer == ca.TwrNumer && f.MappedTwrNumer == p.TwrNumber);
//                                    if (cap == null)
//                                    {
//                                        cad.Pos.Add(new CommonArticleDoc.CommonArticleDocPos
//                                        {
//                                            CommonTwrNumer = ca.TwrNumer,
//                                            CommonTwrCode = ca.TwrCode,
//                                            MappedTwrNumer = p.TwrNumber,
//                                            MappedTwrCode = p.TwrCode,
//                                            Quantity = orderQuantity
//                                        });
//                                    }
//                                    else
//                                        cap.Quantity += orderQuantity;
//                                }
//                            }
//                        }
//                        else
//                            SetLogText($"Sprzedaż dla towaru wspólnego {ca.TwrCode} została wyłączona - pomijanie");
//                    }
//                }

//                if (cadList.Count > 0)
//                {
//                    SQL.CreateCommonArticleDoc(cadList);
//                    SQL.SaveCommonArticleDocsForZam(zam.ID, cadList.Select(cad => cad.Head.ID).ToList());
//                    SQL.CreateIntMessages(zam.ID, cadList, zam.Numer);

//                    SetLogText("Oczekuje na wystawienie dokumentów zakupu/sprzedaży towarów wspólnych - pomijanie.");
//                    czekaNaWspolne = true;
//                    return;
//                }
//            }

//            try
//            {
//                zsKar = SQL.PobierzRejestrZS(zam.GIDNumer);
//                doc.Head.KarNumer = zsKar;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Błąd podczas pobierania nr rejestru ZS: " + ex.Message);
//            }

//            var aktualizujTermin = true;

//            try
//            {
//                SetLogText($"DEBUG FrsID:{doc.Head.FRSId} Seria:{doc.Head.Serie} AkwNumer:{doc.Head.AkwNumer}");
//                docId = XLAPI.NewDoc(apiSession, ref doc.Head, 0);
//                SetLogText("Wystawiono dokument " + doc.Head.DocNumber);
//                if (doc.Head.GIDType == 2001 && zam.DataOdroczenia.HasValue)
//                {
//                    SetLogText("Ustawianie atrybutu 'Odroczona data realizacji'");
//                    var atr = new XLAPI_Wrapper.Atrybut
//                    {
//                        ObiTyp = doc.Head.GIDType,
//                        ObiFirma = doc.Head.GIDFirm,
//                        ObiNumer = doc.Head.GIDNumber,
//                        Klasa = "Odroczona data realizacji",
//                        Wartosc = zam.DataOdroczenia.Value.Date.Subtract(new DateTime(1800, 12, 28).Date).Days.ToString()
//                    };
//                    API.DodajAtrybut(apiSession, ref atr);
//                }
//                if (!string.IsNullOrEmpty(zam.Paczkomat))
//                {
//                    SetLogText("Przepisywanie atrybutu 'INT - Paczkomat'");
//                    var atr = new XLAPI_Wrapper.Atrybut
//                    {
//                        ObiTyp = doc.Head.GIDType,
//                        ObiFirma = doc.Head.GIDFirm,
//                        ObiNumer = doc.Head.GIDNumber,
//                        Klasa = "INT - Paczkomat",
//                        Wartosc = zam.Paczkomat
//                    };
//                    API.DodajAtrybut(apiSession, ref atr);
//                }
//                if (doc.Head.GIDType == 1616)
//                {
//                    SetLogText("Przepisywanie atrybutów 'Przeznaczenie RW' i 'MPK księgowe'");
//                    var atr = new XLAPI_Wrapper.Atrybut
//                    {
//                        ObiTyp = doc.Head.GIDType,
//                        ObiFirma = doc.Head.GIDFirm,
//                        ObiNumer = doc.Head.GIDNumber,
//                        Klasa = "MPK księgowe",
//                        Wartosc = zam.AtrMPKKsiegowe
//                    };
//                    API.DodajAtrybut(apiSession, ref atr);

//                    atr = new XLAPI_Wrapper.Atrybut
//                    {
//                        ObiTyp = doc.Head.GIDType,
//                        ObiFirma = doc.Head.GIDFirm,
//                        ObiNumer = doc.Head.GIDNumber,
//                        Klasa = "Przeznaczenie RW",
//                        Wartosc = zam.AtrPrzeznaczeniaRW
//                    };
//                    API.ModyfikujAtrybut(atr);
//                }

//                SetLogText("Sprawdzenie adresu docelowego");
//                if (zam.Rodzaj == ZamRodzaj.Zewn && (doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005 || doc.Head.GIDType == 2036 || doc.Head.GIDType == 2034))
//                {
//                    var dok = API.PobierzDokument(doc.Head.GIDType, doc.Head.GIDNumber);
//                    SetLogText($"DEBUG ZaN_FormaNr:{zam.FormaNr} TrN_FormaNr:{dok.Naglowek.Forma}\tZaN_AdwNumer:{zam.AdwNumer} TrN_AdwNumer:{dok.Naglowek.AdwNumer}");
//                    if (dok.Naglowek.AdwNumer != zam.AdwNumer)
//                    {
//                        SetLogText("Niezgodny adresy docelowy między ZS a WZ - poprawianie");
//                        SQL.ModyfikujAdresDocelowy(doc.Head.GIDType, doc.Head.GIDNumber, zam.AdwTyp, zam.AdwNumer);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Błąd dodawania nowego dokumentu: " + ex.Message);
//            }

//            try
//            {
//                SetLogText("Dodawanie pozycji na dokument");
//                foreach (var p in doc.Positions)
//                {
//                    try
//                    {
//                        var rebate = p.Rebate;
//                        SetLogText($"DEBUG Dodawanie pozycji Towar:{p.TwrNumber} Ilość:{p.Quantity} Cena:{p.Price} Rabat:{p.Rebate} Nagroda:{p.Nagroda}");
//                        p.GIDLp = XLAPI.AddDocPos(docId, p, 0, 0);
//                        if (p.GIDLp > 0 && p.Nagroda)
//                        {
//                            SetLogText($"Nagroda w programie lojalnościowym - modyfikacja pozycji aby wymusić rabat {rebate}%");
//                            var pos = API.PobierzDokument(doc.Head.GIDType, doc.Head.GIDNumber).Pozycje.Find(f => f.GIDLp == p.GIDLp);
//                            if (pos == null)
//                                throw new Exception($"Nie znaleziono pozycji {p.GIDLp} na dokumencie!");

//                            var mod = new XLAPI_Wrapper.TraElem
//                            {
//                                GIDLp = p.GIDLp,
//                                Ilosc = pos.Ilosc,
//                                GrupaVat = pos.GrupaVat,
//                                Cena = pos.Cena,
//                                CenaPoczatkowa = pos.CenaPoczatkowa,
//                                Wartosc = pos.Wartosc,
//                                Waluta = pos.Waluta,
//                                StawkaVat = pos.StawkaVat,
//                                Rabat = rebate,
//                                FlagaVat = pos.FlagaVat
//                            };
//                            API.ModyfikujPozycje(docId, ref mod);

//                            SQL.UpdateTreNagroda(doc.Head.GIDType, doc.Head.GIDNumber, p.GIDLp);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        throw new Exception("Błąd dodawania pozycji Towar:" + p.TwrNumber + " ZlcNumer:" + p.ZlcNumber + " ZlcLp:" + p.ZlcLp + " Ilość:" + p.Quantity.ToString("0.00") + ": " +
//                                            ex.Message);
//                    }

//                    var twrType = SQL.GetTwrType(p.TwrNumber);
//                    if (twrType == 1 || twrType == 2)
//                    {
//                        var ilosc = SQL.PobierzIloscNaDokumencie(p.TwrNumber, doc.Head.GIDNumber, p.GIDLp, p.ZlcNumber, p.ZlcLp);
//                        var brakPozycji = false;
//                        if (ilosc < p.Quantity && doc.Head.GIDType != 1603)
//                        {
//                            SetLogText("Zabrakło ilości dla towaru " + p.TwrCode + " - szukanie zamienników");
//                            var zamiennik = SQL.WyszukajZamiennik(p.TwrNumber);
//                            if (zamiennik == null)
//                            {
//                                SetLogText("Nie znaleziono zamienników");
//                                brakPozycji = true;
//                            }
//                            else
//                            {
//                                SetLogText("Znaleziono zamiennik " + zamiennik.TwrCode);
//                                if (ilosc > 0)
//                                {
//                                    SetLogText("Usunięcie oryginalnej pozycji");
//                                    XLAPI.DeleteDocPos(docId, p.GIDLp);
//                                }
//                                SetLogText("Dodanie pozycji zamiennika");
//                                if (zam.Rodzaj == ZamRodzaj.Zewn)
//                                {
//                                    zamiennik.GIDType = p.GIDType;
//                                    zamiennik.GIDFirm = p.GIDFirm;
//                                    zamiennik.Quantity = p.Quantity;
//                                    zamiennik.Price = p.Price;
//                                    zamiennik.PakietId = p.PakietId;
//                                    zamiennik.Gratis = p.Gratis;
//                                    zamiennik.PromocjaProgId = p.PromocjaProgId;
//                                    zamiennik.Rebate = p.Rebate;
//                                    zamiennik.Store = p.Store;
//                                    zamiennik.Nagroda = p.Nagroda;
//                                }
//                                else
//                                {
//                                    zamiennik.GIDType = p.GIDType;
//                                    zamiennik.GIDFirm = p.GIDFirm;
//                                    zamiennik.Quantity = p.Quantity;
//                                    zamiennik.Store = p.Store;
//                                }

//                                try
//                                {
//                                    zamiennik.GIDLp = XLAPI.AddDocPos(docId, zamiennik, 0, 0);
//                                    if (zamiennik.Nagroda)
//                                        SQL.UpdateTreNagroda(doc.Head.GIDType, doc.Head.GIDNumber, zamiennik.GIDLp);
//                                }
//                                catch (Exception ex)
//                                {
//                                    throw new Exception("Błąd dodawania pozycji Towar:" + zamiennik.TwrNumber + " Ilość:" + zamiennik.Quantity.ToString("0.00") + ": " +
//                                                        ex.Message);
//                                }

//                                ilosc = SQL.PobierzIloscNaDokumencie(zamiennik.TwrNumber, doc.Head.GIDNumber, zamiennik.GIDLp, 0, 0);
//                                if (ilosc < zamiennik.Quantity)
//                                    brakPozycji = true;
//                            }

//                            if (brakPozycji)
//                            {
//                                braki = true;
//                                var ca = SQL.GetCommonArticleForTwr(p.TwrNumber);
//                                if (p.PakietId == 0)
//                                {
//                                    if (ca != null)
//                                        brakiPozycje += "Lp. " + p.ZlcLp + " Towar " + p.TwrCode + " Firma: " + ca.CompanyName + " zabrakło ilości: " + (p.Quantity - ilosc).ToString("0.00") + Environment.NewLine;
//                                    else
//                                        brakiPozycje += "Lp. " + p.ZlcLp + " Towar " + p.TwrCode + " zabrakło ilości: " + (p.Quantity - ilosc).ToString("0.00") + Environment.NewLine;
//                                }
//                                else
//                                {
//                                    var zp = zam.Pozycje.Find(f => f.TwrGIDNumer == p.TwrNumber && f.PakietId == p.PakietId);
//                                    if (zp != null)
//                                    {
//                                        if (ca != null)
//                                            brakiPozycje += "Lp. " + p.ZlcLp + " Towar " + p.TwrCode + " Firma: " + ca.CompanyName + " zabrakło ilości: " + (p.Quantity - ilosc).ToString("0.00") + " Promocja pakietowa: " + zp.Promocja +
//                                                        Environment.NewLine;
//                                        else
//                                            brakiPozycje += "Lp. " + p.ZlcLp + " Towar " + p.TwrCode + " zabrakło ilości: " + (p.Quantity - ilosc).ToString("0.00") + " Promocja pakietowa: " + zp.Promocja +
//                                                        Environment.NewLine;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }

//                if (zam.Platnosci.Count > 0 && (doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005 || doc.Head.GIDType == 2034))
//                {
//                    SetLogText("Przepisywanie płatności z ZS");
//                    var docXL = API.PobierzDokument(doc.Head.GIDType, doc.Head.GIDNumber);
//                    var brutto = 0.0m;
//                    foreach (var v in docXL.VAT)
//                        brutto += v.Wartosc + v.WartoscVat;

//                    foreach (var zap in zam.Platnosci.OrderByDescending(o => o.Kwota))
//                    {
//                        var trp = new TraPlat
//                        {
//                            Forma = zap.FormaPl,
//                            Data = zap.TerminData.Year == 1800 ? DateTime.Now.AddDays(zap.TerminDni) : zap.TerminData,
//                            Kwota = zap.Kwota,
//                            Waluta = zap.Waluta
//                        };

//                        if (zap.Kwota == 0)
//                        {
//                            var sumPlat = 0.0m;
//                            foreach (var z in zam.Platnosci.Where(w => w.Kwota != 0))
//                                sumPlat += z.Kwota;

//                            trp.Kwota = brutto - sumPlat;
//                        }

//                        if (Properties.Settings.Default.INTXLID == 1)
//                        {
//                            if (zam.FrsNazwa.StartsWithAnyOf(oddzialy) || zam.Seria == "EC")
//                                trp.Forma = (zap.FormaPl == 10 || zap.FormaPl == 11 || zap.FormaPl == 17 || zap.FormaPl == 50) ? 21 : zap.FormaPl;

//                            if (zam.Platnosci.Count == 1)
//                                trp.Forma = doc.Head.PaymentMethod;
//                        }

//                        if (zam.DataOdroczenia.HasValue && doc.Head.GIDType == 2001)
//                            trp.Data = trp.Data.AddDays(zam.DataOdroczenia.Value.Date.Subtract(DateTime.Now.Date).Days);

//                        SetLogText($"DEBUG Dodanie płatności Forma:{trp.Forma}\tData:{trp.Data:yyyy-MM-dd}\tKwota:{trp.Kwota}");

//                        API.DodajPlatnosc(docId, ref trp);
//                    }
//                }

//                if (doc.Head.GIDType == 2001)
//                {
//                    SetLogText("Sprawdzenie promocji pod kątem płatności");
//                    SQL.PobierzPlatnoscZPromocji(doc.Head.GIDType, doc.Head.GIDNumber, out var formaNazwa, out var formaNr, out var termin);
//                    SetLogText($"DEBUG Forma nazwa:{formaNazwa} Forma nr:{formaNr} Termin:{termin}");
//                    if (!string.IsNullOrEmpty(formaNazwa))
//                    {
//                        if (formaNr == 0)
//                            throw new Exception($"Nie znaleziono numeru formy płatności dla '{formaNazwa}'");

//                        if (termin == -1)
//                            SetLogText("Nie znaleziono terminu płatności na promocji!");

//                        SetLogText("Zmiana formy i terminu płatności na WZ");
//                        SQL.AktualizujPlatnosci(doc.Head.GIDType, doc.Head.GIDNumber, formaNazwa, formaNr, termin);
//                        aktualizujTermin = false;
//                    }
//                }

//                SetLogText("Kopiowanie dodatkowych rozliczeń");
//                SQL.KopiujDodatkoweRozliczenia(zam.GIDType, zam.GIDNumer, doc.Head.GIDType, doc.Head.GIDNumber);

//                if (zam.DodatkowaPlatnosc > 0)
//                {
//                    SetLogText($"Kopiowanie atrybutu 'INT - Dodatkowa płatność' ({zam.DodatkowaPlatnosc})");
//                    SQL.SetAttrValue(doc.Head.GIDType, doc.Head.GIDFirm, doc.Head.GIDNumber, 0, "INT - Dodatkowa płatność",
//                        zam.DodatkowaPlatnosc.ToString("0.00").Replace(",", "."), Properties.Settings.Default.UserAPI);
//                }
//            }
//            catch
//            {
//                SetLogText("Próba wycofania dokumentu");
//                try
//                {
//                    TryMultipleTimes(() => XLAPI.CloseDoc(docId, XLAPI.CloseDocMode.Usunięcie, XLAPI.ClientLimit.Definicja, XLAPI.Storage.WgKonfiguracji), 5);
//                }
//                catch (Exception ex)
//                {
//                    SetLogText("Nie udało się wycofać dokumentu: " + ex.Message);
//                    SQL.ModyfikujOpisDokumentu(doc.Head.GIDType, doc.Head.GIDFirm, doc.Head.GIDNumber, "do usunięcia");
//                    SendMail("ITbox@polwell.pl", "Generator WZ - nie udało się wycofać dokumentu",
//                                        "Podczas próby usunięcia dokumentu " + doc.Head.DocNumber + "wystąpił błąd:" + Environment.NewLine +
//                                        ex.Message + Environment.NewLine + "Proszę usunąć dokument ręcznie.");
//                    throw;
//                }
//                throw;
//            }

//            DokumentHandlowy tmpMMW = null;
//            if (doc.Head.GIDType == 1603)
//                tmpMMW = API.PobierzDokument(doc.Head.GIDType, doc.Head.GIDNumber);

//            if (tmpMMW != null && tmpMMW.Pozycje.Count == 0)
//            {
//                SetLogText("Nie udało się dodać żadnej pozycji na MMW - wycofywanie dokumentu");
//                try
//                {
//                    TryMultipleTimes(() => XLAPI.CloseDoc(docId, XLAPI.CloseDocMode.Usunięcie, XLAPI.ClientLimit.Definicja, XLAPI.Storage.WgKonfiguracji), 5);
//                }
//                catch (Exception ex)
//                {
//                    SetLogText("Nie udało się wycofać dokumentu: " + ex.Message);
//                    SQL.ModyfikujOpisDokumentu(doc.Head.GIDType, doc.Head.GIDFirm, doc.Head.GIDNumber, "do usunięcia");
//                    SendMail("ITbox@polwell.pl", "Generator WZ - nie udało się wycofać dokumentu",
//                                        "Podczas próby usunięcia dokumentu " + doc.Head.DocNumber + "wystąpił błąd:" + Environment.NewLine +
//                                        ex.Message + Environment.NewLine + "Proszę usunąć dokument ręcznie.");
//                }

//                SQL.AktualizujBlad(zam.ID, "Błąd: Nie udało się dodać żadnej pozycji na MMW - brak stanu dla wszystkich towarów");
//            }
//            else if (braki && (
//                        zam.FrsNazwa.StartsWithAnyOf(oddzialy) || zam.FrsNazwa.StartsWith("Sprzedaż FLK.Dział handlowy") || zam.Segment > 0
//                        || (zam.Seria == "EC" && !SQL.SprawdzCzyKlientToHurtownia(zam.GIDNumer))
//                        ) && zam.Seria != "CEDI")
//            {
//                SetLogText("Niepełna realizacja ZS, wycofywanie dokumentu.");

//                try
//                {
//                    TryMultipleTimes(() => XLAPI.CloseDoc(docId, XLAPI.CloseDocMode.Usunięcie, XLAPI.ClientLimit.Definicja, XLAPI.Storage.WgKonfiguracji), 5);
//                    if (zmienFrsIdSQLem && frsId > 0)
//                    {
//                        SetLogText("Zmiana centrum właściciela na ZS na FrsID:" + frsId);
//                        TryMultipleTimes(() => API.ModyfikujFrsIdZamowienia(zam.GIDType, zam.GIDFirm, zam.GIDNumer, 0, frsId), 5);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    SetLogText("Nie udało się wycofać dokumentu: " + ex.Message);
//                    SQL.ModyfikujOpisDokumentu(doc.Head.GIDType, doc.Head.GIDFirm, doc.Head.GIDNumber, "do usunięcia");
//                    SendMail("ITbox@polwell.pl", "Generator WZ - nie udało się wycofać dokumentu",
//                                        "Podczas próby usunięcia dokumentu " + doc.Head.DocNumber + "wystąpił błąd:" + Environment.NewLine +
//                                        ex.Message + Environment.NewLine + "Proszę usunąć dokument ręcznie.");
//                }

//                SQL.AktualizujBlad(zam.ID, "Błąd: Dokument nie spełnia automatu realizacji z mag. " + Properties.Settings.Default.MagazynRealizacji + Environment.NewLine +
//                                                    "Brakujące ilości:" + Environment.NewLine + brakiPozycje);
//            }
//            else
//            {
//                try
//                {
//                    XLAPI.CloseDoc(docId, XLAPI.CloseDocMode.Bufor, XLAPI.ClientLimit.Definicja, XLAPI.Storage.WgKonfiguracji);
//                    if (zmienFrsIdSQLem && frsId > 0)
//                    {
//                        SetLogText("Zmiana centrum właściciela na FrsID:" + frsId);
//                        TryMultipleTimes(() => API.ModyfikujFrsIdDokumentu(doc.Head.GIDType, zam.GIDFirm, doc.Head.GIDNumber, 0, frsId), 5);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    try
//                    {
//                        TryMultipleTimes(() => XLAPI.CloseDoc(docId, XLAPI.CloseDocMode.Usunięcie, XLAPI.ClientLimit.Definicja, XLAPI.Storage.WgKonfiguracji), 5);
//                    }
//                    catch (Exception ex2)
//                    {
//                        SetLogText("Nie udało się wycofać dokumentu: " + ex2.Message);
//                        SQL.ModyfikujOpisDokumentu(doc.Head.GIDType, doc.Head.GIDFirm, doc.Head.GIDNumber, "do usunięcia");
//                        SendMail("ITbox@polwell.pl", "Generator WZ - nie udało się wycofać dokumentu",
//                                            "Podczas próby usunięcia dokumentu " + doc.Head.DocNumber + "wystąpił błąd:" + Environment.NewLine +
//                                            ex2.Message + Environment.NewLine + "Proszę usunąć dokument ręcznie.");
//                        throw;
//                    }
//                }

//                SetLogText("Sprawdzenie czy dokument istnieje w bazie");
//                var istnieje = SQL.SprawdzCzyWZIstnieje(doc.Head.GIDType, doc.Head.GIDNumber);
//                if (!istnieje)
//                    throw new Exception("Dokument został usunięty z bazy!");

//                if (zam.Rodzaj == ZamRodzaj.Zewn && (doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005 || doc.Head.GIDType == 2036 || doc.Head.GIDType == 2034))
//                {
//                    SetLogText("DEBUG KarNumer przed zatwierdzeniem ZS:" + zam.KarNumer);

//                    try
//                    {
//                        var trnKar = 0;
//                        var trpKar = 0;

//                        SetLogText("DEBUG KarNumer po zatwierdzeniu ZS:" + zsKar);

//                        SQL.AktualizujRejestr(doc.Head.GIDType, doc.Head.GIDNumber, zsKar);
//                        if (aktualizujTermin)
//                            SQL.AktualizujTermin(doc.Head.GIDType, doc.Head.GIDNumber, DateTime.Now.Subtract(new DateTime(1800, 12, 28)).Days + doc.Head.PaymentTerm);

//                        SQL.PobierzRejestryWZ(doc.Head.GIDType, doc.Head.GIDNumber, out trnKar, out trpKar);

//                        SetLogText("DEBUG ZS KarNumer:" + zsKar + "  TRN KarNumer:" + trnKar + "  TRP KarNumer:" + trpKar);
//                        if (trnKar != zsKar || trpKar != zsKar || trnKar != trpKar)
//                            SetLogText("Niezgodność rejestrów pomiędzy ZS, nagłówkiem WZ i/lub płatnością WZ.");
//                    }
//                    catch (Exception ex)
//                    {
//                        SetLogText("Błąd podczas weryfikacji rejestrów: " + ex.Message);
//                    }

//                    try
//                    {
//                        if (zam.Seria == "EC" && doc.Head.AkwNumer > 0)
//                            SQL.AktualizujAkw(doc.Head.GIDType, doc.Head.GIDNumber, doc.Head.AkwTyp, doc.Head.AkwFirma, doc.Head.AkwNumer, doc.Head.AkwLp);
//                    }
//                    catch (Exception ex)
//                    {
//                        SetLogText($"Błąd aktualizowania akwizytora: {ex.Message}");
//                    }

//                    if (doc.Head.GIDType == 2005 && (!string.IsNullOrEmpty(doc.Head.IncotermsMiejsce) || !string.IsNullOrEmpty(doc.Head.IncotermsSymbol)))
//                    {
//                        try
//                        {
//                            SetLogText("Aktualizacja warunków incoterms");
//                            SQL.UpdateIncoterms(doc.Head.GIDType, doc.Head.GIDNumber, doc.Head.IncotermsSymbol, doc.Head.IncotermsMiejsce);
//                        }
//                        catch (Exception ex)
//                        {
//                            SetLogText($"Błąd aktualizowania warunków incoterms: {ex.Message}");
//                        }
//                    }
//                }

//                var wms = true;
//                var sendPrzedplata = false;
//                var errorCount = 0;

//                if (zam.Rodzaj == ZamRodzaj.Zewn && (doc.Head.GIDType == 2001 || doc.Head.GIDType == 2005 || doc.Head.GIDType == 2034))
//                {
//                    if (Properties.Settings.Default.ObslugiwacPrzedplaty)
//                    {
//                        //Pominięcie wysyłania dokumentów na przedpłatę
//                        if (zam.Przedplata)
//                        {
//                            wms = false;
//                            sendPrzedplata = true;
//                        }

//                        if (sendPrzedplata)
//                        {
//                            SetLogText("Ustawianie atrybutu [Przelewy24]");
//                            try
//                            {
//                                SQL.UstawAtrybutPrzelewy24(doc.Head.GIDType, doc.Head.GIDNumber);
//                            }
//                            catch (Exception ex)
//                            {
//                                SetLogText("Błąd ustawiania atrybutu [Przelewy24]: " + ex.Message);
//                            }

//#if !DEBUG
//                            if (!(zam.Seria == "EC" && Properties.Settings.Default.INTXLID == 1))
//                            {
//                                SetLogText("Wysyłanie powiadomienia o przedpłacie");
//                                while (errorCount < 5)
//                                {
//                                    try
//                                    {
//                                        SQL.WyslijPowiadomienieOPrzedplacie(_companyId, zam.GIDTyp, zam.GIDNumer, Properties.Settings.Default.UserAPI);
//                                        break;
//                                    }
//                                    catch (Exception ex)
//                                    {
//                                        errorCount++;
//                                        SetLogText("Błąd podczas wysyłki powiadomienia o przedpłacie: " + ex.Message);
//                                    }
//                                }
//                            }
//#endif
//                        }
//                    }
//                }

//                if (zam.FrsNazwa.Contains("Eksport"))
//                    wms = false;

//                if (wms && zam.Seria == "FLK" && zam.IdoSellPrzedplata.Trim().ToLower() == "tak")
//                {
//                    SetLogText("'IdoSell - Przedpłata' = TAK, bez wysyłki do WMS.");
//                    wms = false;
//                }

//                if (!Properties.Settings.Default.WysylajZSzONEdoWMS && zam.Seria == "EC")
//                    wms = false;

//                if (wms)
//                {
//                    SetLogText("Sprawdzanie dokumentu przed wysyłką do WMS");

//                    try
//                    {
//                        wms = SQL.SprawdzPrzedWysylkaDoWMS(doc.Head.GIDType, doc.Head.GIDNumber);
//                        if (doc.Head.Serie == "MC")
//                        {
//                            if (zam.Pozycje.Any(a => a.TwrKod.In(twrKodyBlokujaceWMS)))
//                                wms = false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        wms = false;
//                        SetLogText(ex.Message);
//                        SQL.AktualizujBlad(zam.ID, "Błąd: " + ex.Message);
//                    }
//                }

//                if (wms)
//                {
//                    if (zam.DataOdroczenia.HasValue && doc.Head.GIDType == 2001)
//                        SetLogText("Wysyłanie komunikatu 'SEND_WZ_TO_WMS_CONF' do Interfejsu WMS");
//                    else
//                        SetLogText("Wysyłanie do WMS");

//                    while (errorCount < 5)
//                    {
//                        try
//                        {
//                            if (zam.DataOdroczenia.HasValue && doc.Head.GIDType == 2001)
//                                SQL.SendMessageToInt(_companyId, doc.Head.GIDType, doc.Head.GIDNumber, Properties.Settings.Default.UserAPI);
//                            else
//                                SQL.WyslijDoWMS(_companyId, doc.Head.GIDType, doc.Head.GIDFirm, doc.Head.GIDNumber, Properties.Settings.Default.UserAPI);
//                            break;
//                        }
//                        catch (Exception ex)
//                        {
//                            errorCount++;
//                            SetLogText("Błąd podczas wysyłki do WMS: " + ex.Message);
//                        }
//                    }
//                }

//                try
//                {
//                    SQL.AktualizujBlad(zam.ID, "Przetworzony poprawnie");
//                }
//                catch (Exception ex)
//                {
//                    SetLogText("Błąd oznaczania zamówienia jako przetworzone: " + ex.Message);
//                }
//            }
//        }

    }
}
