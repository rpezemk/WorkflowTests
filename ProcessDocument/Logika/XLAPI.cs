using System;
using System.IO;

using ProcessDocument.Model;

namespace ProcessDocument.Logika
{
    /// <summary>
    /// Obsługa API XL
    /// </summary>
    internal static class XLAPI
    {
        /// <summary>
        /// Wersja API
        /// </summary>
        internal static int Version = 20201;

        /// <summary>
        /// Typ licencji
        /// </summary>
        internal enum Licence
        {
            Stanowiskowa = 1,
            Administrator = 2,
            Sprzedaż = 3,
            Księgowość = 4,
            Kompletacja = 5,
            Zamówienia = 11
        }

        /// <summary>
        /// Tryb zamykania dokumentu
        /// </summary>
        internal enum CloseDocMode
        {
            /// <summary>
            /// Zatwierdzenie dokumentu bez automatycznego wydruku
            /// </summary>
            ZatwierdzenieBezWydruku = -10,
            /// <summary>
            /// Zamknięcie otwartego dokumentu
            /// </summary>
            Zamknięcie = -3,
            /// <summary>
            /// Anulowanie dokumentu
            /// </summary>
            Anulowanie = -2,
            /// <summary>
            /// Usunięcie dokumentu
            /// </summary>
            Usunięcie = -1,
            /// <summary>
            /// Zatwierdzenie dokumentu
            /// </summary>
            Zatwierdzenie = 0,
            /// <summary>
            /// Zapisanie dokumentu do bufora
            /// </summary>
            Bufor = 1,
            /// <summary>
            /// Drukowanie dokumentu
            /// </summary>
            Drukowanie = 2,
            /// <summary>
            /// Zatwierdzenie dokumentu i wydrukowanie
            /// </summary>
            ZatwierdzenieIDrukowanie = 10
        }

        /// <summary>
        /// Tryb zamykania zamówienia
        /// </summary>
        internal enum CloseZamMode
        {
            /// <summary>
            /// Zamknięcie zamówienia
            /// </summary>
            Zamknięcie = 0,
            /// <summary>
            /// Usunięcie zamówienia
            /// </summary>
            Usunięcie = 1,
            /// <summary>
            /// Potwierdzenie zamówienia
            /// </summary>
            Potwierdzenie = 2,
            /// <summary>
            /// Status złożone dla zamówień wewnętrznych
            /// </summary>
            Złożone = 3,
            /// <summary>
            /// Anulowanie zamówienia
            /// </summary>
            Anulowane = 4,
            /// <summary>
            /// Zamknięcie potwierdzone zamówienia
            /// </summary>
            ZamkniętePotwierdzone = 5,
            /// <summary>
            /// Otwarcie zamówienia
            /// </summary>
            Otwarte = 6
        }

        /// <summary>
        /// Zatwierdzanie dokumentu po przekroczeniu limitu kontrahenta
        /// </summary>
        internal enum ClientLimit
        {
            /// <summary>
            /// Z definicji dokumentu
            /// </summary>
            Definicja = -1,
            /// <summary>
            /// Zablokuj zatwierdzanie
            /// </summary>
            Zablokuj = 0,
            /// <summary>
            /// Ostrzegaj
            /// </summary>
            Ostrzegaj = 1,
            /// <summary>
            /// Zezwalaj
            /// </summary>
            Zezwalaj = 2
        }

        /// <summary>
        /// Generowanie magazynowych
        /// </summary>
        internal enum Storage
        {
            /// <summary>
            /// Wg konfiguracji
            /// </summary>
            WgKonfiguracji = 0,
            /// <summary>
            /// <para>Dla MMW: nie generuj WM, nie generuj MMP </para>
            /// <para>Dla innych: nie generuj</para>
            /// </summary>
            NieGeneruj = 1,
            /// <summary>
            /// <para>Dla MMW: nie generuj WM, generuj MMP do bufora</para>
            /// <para>Dla innych: generuj do bufora</para>
            /// </summary>
            Bufor = 2,
            /// <summary>
            /// <para>Dla MMW: nie generuj WM, generuj MMP zatwierdzone</para>
            /// <para>Dla innych: generuj zatwierdzone</para>
            /// </summary>
            Zatwierdzone = 3,
            /// <summary>
            /// Dla MMW: generuj WM do bufora, nie generuj MMP
            /// </summary>
            WMBufor = 11,
            /// <summary>
            /// Dla MMW: generuj WM do bufora, generuj MMP do bufora
            /// </summary>
            WMBuforMMPBufor = 12,
            /// <summary>
            /// Dla MMW: generuj WM do bufora, generuj MMP zatwierdzone
            /// </summary>
            WMBuforMMPZatwierdzone = 13,
            /// <summary>
            /// Dla MMW: generuj WM zatwierdzone, nie generuj MMP
            /// </summary>
            WMZatwierdzone = 21,
            /// <summary>
            /// Dla MMW: generuj WM zatwierdzone, generuj MMP do bufora
            /// </summary>
            WMZatwierdzoneMMPBufor = 22,
            /// <summary>
            /// Dla MMW: generuj WM zatwierdzone, generuj MMP zatwierdzone
            /// </summary>
            WMZatwierdzoneMMPZatwierdzone = 23
        }

        /// <summary>
        /// Tryb transakcji
        /// </summary>
        internal enum Transaction
        {
            /// <summary>
            /// Rozpoczęcie transakcji
            /// </summary>
            Begin = 0,
            /// <summary>
            /// Zatwierdzenie transakcji
            /// </summary>
            Commit = 1,
            /// <summary>
            /// Wycofanie transakcji
            /// </summary>
            Rollback = 2,
            /// <summary>
            /// Sprawdzenie aktywności transakcji
            /// </summary>
            Check = 3
        }

        /// <summary>
        /// Typ urządzenia wydruku
        /// </summary>
        internal enum PrintingDevice
        {
            /// <summary>
            /// Plik
            /// </summary>
            Plik = -1,
            /// <summary>
            /// Ekran
            /// </summary>
            Ekran = 1,
            /// <summary>
            /// Domyślna drukarka
            /// </summary>
            DomyslnaDrukarka = 2,
            /// <summary>
            /// Inna drukarka
            /// </summary>
            InnaDrukarka = 3,
            /// <summary>
            /// Serwer wydruków
            /// </summary>
            SerwerWydrukow = 4
        }

        /// <summary>
        /// Logowanie do XL
        /// </summary>
        /// <param name="programId">Nazwa programu korzystającego z API</param>
        /// <param name="dbName">Nazwa bazy danych</param>
        /// <param name="opeIdent">Identyfiaktor operatora</param>
        /// <param name="opePasswd">Hasło operatora</param>
        /// <param name="hasp">Adres serwera HASP</param>
        /// <param name="logPath">Ścieżka do pliku logu</param>
        /// <returns>ID sesji</returns>
        internal static int Login(string programId, string dbName, string opeIdent, string opePasswd, string hasp, string logPath)
        {
            

            return 12323;
        }

        /// <summary>
        /// Sprawdza stan licencji
        /// </summary>
        /// <param name="licence">Typ licencji</param>
        /// <param name="refresh">Czy odświeżyć licencje (0 - sprawdzenie licencji, 1 - odświeżenie i sprawdzenie licencji)</param>
        /// <returns>Stan licencji</returns>
        internal static int CheckLicence(Licence licence, int refresh)
        {
            
            return 1;
        }

        /// <summary>
        /// Otwiera dokument
        /// </summary>
        /// <param name="session">Sesja</param>
        /// <param name="gidType">GIDTyp dokumentu</param>
        /// <param name="gidFirm">GIDFirma dokumentu</param>
        /// <param name="gidNumber">GIDNumer dokumentu</param>
        /// <param name="gidLp">GIDLp dokumentu</param>
        /// <returns>ID otwartego dokumentu</returns>
        internal static int OpenDoc(int session, int gidType, int gidFirm, int gidNumber)
        {
            var result = 0;
           

            return result;
        }

        /// <summary>
        /// Dodaje nowy dokument
        /// </summary>
        /// <param name="session">Sesja</param>
        /// <param name="docHead">Nagłówek dokumentu</param>
        /// <param name="costSettled">Czy koszt ustalony (0-konfiguracja, 1-nie ustalono, 2-ustalono)</param>
        /// <returns>ID dokumentu</returns>
        internal static int NewDoc(int session, ref DocumentHead docHead, int costSettled)
        {
            var result = 0;
           
            return result;
        }

        /// <summary>
        /// Dodaje pozycję do dokumentu
        /// </summary>
        /// <param name="docId">ID dokumentu</param>
        /// <param name="pos">Pozycja dokumentu</param>
        /// <param name="quantityReq">Ilość wymagana</param>
        /// <param name="rezIgnore">Ignorowanie rezerwacji</param>
        /// <param name="serieClass">Numer klasy cechy</param>
        /// <returns>GIDLp dodanej pozycji</returns>
        internal static int AddDocPos(int docId, DocumentPos pos, int quantityReq, int rezIgnore, int serieClass = 0)
        {
            var result = 0;
            

            return result;
        }

        internal static void DeleteDocPos(int docId, int gidLp)
        {
            
        }

        /// <summary>
        /// Zamyka dokument
        /// </summary>
        /// <param name="docId">ID dokumentu</param>
        /// <param name="mode">Tryb zamknięcia</param>
        /// <param name="kntLimit">Czy zatwierdzać mimo przekroczenia limitu</param>
        /// <param name="store">Generowanie magazynowych</param>
        internal static void CloseDoc(int docId, CloseDocMode mode, ClientLimit kntLimit, Storage store)
        {
            
        }

        /// <summary>
        /// Otwiera zamówienie
        /// </summary>
        /// <param name="session">Sesja</param>
        /// <param name="gidType">GIDTyp zamówienia</param>
        /// <param name="gidFirm">GIDFirma zamówienia</param>
        /// <param name="gidNumber">GIDNumer zamówienia</param>
        /// <param name="gidLp">GIDLp zamówienia</param>
        /// <returns>ID otwartego zamówienia</returns>
        internal static int OpenZam(int session, int gidType, int gidFirm, int gidNumber)
        {
            var result = 0;
            

            return result;
        }

        /// <summary>
        /// Zamyka zamówienia
        /// </summary>
        /// <param name="zamId">ID zamówienia</param>
        /// <param name="mode">Tryb zamknięcia</param>
        internal static void CloseZam(int zamId, CloseZamMode mode)
        {
            
        }

        /// <summary>
        /// Pobiera numer dokumentu na podstawie GIDów
        /// </summary>
        /// <param name="gidType">GIDTyp dokumentu</param>
        /// <param name="gidFirm">GIDFirma dokumentu</param>
        /// <param name="gidNumber">GIDNumer dokumentu</param>
        /// <param name="gidLp">GIDLp dokumentu</param>
        /// <returns>Numer dokumentu</returns>
        internal static string GetDocNumber(int gidType, int gidFirm, int gidNumber, int gidLp)
        {

            return "34234";
        }

        /// <summary>
        /// Rozpoczyna nową transakcję
        /// </summary>
        /// <param name="session">Sesja</param>
        /// <returns>Token</returns>
        internal static string BeginTransaction(int session)
        {
            return "sdf8s0df0s";
        }

        /// <summary>
        /// Zatwierdza transakcję
        /// </summary>
        /// <param name="session">Sesja</param>
        internal static void CommitTransaction(int session)
        {
            
        }

        /// <summary>
        /// Wycofanie transakcji
        /// </summary>
        /// <param name="session">Sesja</param>
        internal static void RollbackTransaction(int session)
        {
           
        }

        /// <summary>
        /// Wylogowanie z XL
        /// </summary>
        /// <param name="session">Sesja</param>
        internal static void Logout(int session)
        {
           
        }

        /// <summary>
        /// Usuwa rezerwacje
        /// </summary>
        /// <param name="session">Sesja</param>
        /// <param name="rezType">GIDTyp rezerwacji</param>
        /// <param name="rezFirm">GIDFirma rezerwacji</param>
        /// <param name="rezNumber">GIDNumer rezerwacji</param>
        /// <param name="rezLp">GIDLp rezerwacji</param>
        internal static void DeleteReservation(int session, int rezType, int rezFirm, int rezNumber, int rezLp)
        {
            
        }

        /// <summary>
        /// Zwraca opis błędu API
        /// </summary>
        /// <param name="function">Nr funkcji API</param>
        /// <param name="error">Nr błędu</param>
        /// <returns></returns>
        internal static string GetAPIError(int function, int error)
        {
            return "err";
        }

        internal static int Login(string v1, object bazaAPI, object userAPI, object passwordAPI, object hASP, string v2)
        {
            throw new NotImplementedException();
        }

        internal static int Login(string v1, object bazaAPI, string userAPI, string passwordAPI, string hASP, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
