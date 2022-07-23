using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ProcessDocument.Model;

namespace ProcessDocument.Logika
{
    /// <summary>
    /// Obsługa bazy danych
    /// </summary>
    internal static class SQL
    {
        /// <summary>
        /// Pobranie listy zamówień do przetworzenia z kolejki GWZ
        /// </summary>
        /// <returns>Lista zamówień</returns>
        internal static List<Zamowienie> PobierzZamowienia()
        {
            var result = new List<Zamowienie>();


            return result;
        }

        internal static void Odroczona(int id, DateTime data)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"update dbo.GeneratorWZ set Status=2, LastReadDT=getdate(), RealDate=@Data, LastError='Odroczone'  where ID=@ID", sqlConn);
                sqlCmd.Parameters.Add("@Data", SqlDbType.Date).Value = data.Date;
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Aktualizuje status zamówienia w kolejce GWZ
        /// </summary>
        /// <param name="id">ID zamówienia</param>
        /// <param name="status">Status</param>
        /// <param name="token">Token transakcji</param>
        internal static void AktualizujStatus(int id, int status, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"update dbo.GeneratorWZ set Status=@Status, LastReadDT=getdate(), FinishDT=case @Status when 1 then getdate() else null end where ID=@ID", sqlConn);
                sqlCmd.Parameters.AddWithValue("@Status", status);
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }

        }

        /// <summary>
        /// Aktualizuje pole ZaN_Aktywny na dokumencie zamówienia
        /// </summary>
        /// <param name="gidNumer">GIDNumer zamówienia</param>
        /// <param name="sesja">Nr sesji XL, która zostanie ustawiona w polu ZaN_Aktywny</param>
        /// <param name="token">Token transakcji</param>
        internal static void AktualizujZanAktywny(int gidNumer, int sesja, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"update cdn.ZamNag set ZaN_Aktywny=@Sesja where ZaN_GIDNumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@Sesja", sesja);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Aktualizuje opis błędu w kolejce GWZ
        /// </summary>
        /// <param name="id">ID zamówienia</param>
        /// <param name="error">Opis błędu</param>
        /// <param name="token">Token transakcji</param>
        internal static void AktualizujBlad(int id, string error, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"update dbo.GeneratorWZ set LastError=@Error where ID=@ID", sqlConn);
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.Parameters.AddWithValue("@Error", error);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Pobiera dane o limitach kontrahenta
        /// </summary>
        /// <param name="kntNumer">GIDNumer kontrahenta</param>
        /// <param name="wykorzystanie">Zwracane wykorzystanie limitu</param>
        /// <param name="poTerminie">Zwracana wartość przeterminowanych płatności</param>
        /// <param name="limit">Zwracany limit kredytowy</param>
        internal static void PobierzDaneLimitK(int kntNumer, out decimal wykorzystanie, out decimal poTerminie, out decimal limit)
        {
            wykorzystanie = 0;
            poTerminie = 0;
            limit = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"declare @ts int 
                                        declare @gidnumer int                                             
                                        declare @data int 
                                        declare @makslimit decimal(19, 4) 
                                        declare @przeterminowane smallint 
                                        set @gidnumer = @KntNumer                                            
                                        set @ts = datediff(ss, '01/01/1990 00:00:00', getdate()) 
                                        set @data = datediff(day, convert(datetime, '28-12-1800', 105), getdate())                                             
                                        select @gidnumer = case when knt.knt_knpparam = 0 then knt.knt_gidnumer else knt.knt_knpnumer end
                                        from cdn.kntkarty knt with(nolock) inner join cdn.kntkarty plat with(nolock)
                                                          on plat.knt_gidnumer = knt.knt_knpnumer 
                                        where knt.knt_gidnumer = @gidnumer 
                                        select @makslimit = round(klk_maxlimitwart * klk_kursl / klk_kursm, 2)
                                        from cdn.kntlimityk with(nolock)
                                        where klk_kntnumer = @gidnumer and 
                                              klk_dataod <= @data and 
                                              klk_datado >= @data 
                                        declare @wykorzystanie decimal(19, 4) 
                                        declare @poterminie decimal(19, 4) 
                                        select @wykorzystanie = cdn.SumaWartosciKredytuKontrahenta(@ts, @gidnumer, 1, 0, 1,0,0) 
                                        select @poterminie = cdn.SumaWartosciKredytuTerminKontrahenta(@ts, 0, @gidnumer, 1, 0, 0,0,0)                                             
                                        select isnull(@wykorzystanie, 0) as wykorzystanie, isnull(@poterminie, 0) as poterminie,
                                               isnull(@makslimit, 0) as maks_limit", sqlConn);
                sqlCmd.Parameters.AddWithValue("@KntNumer", kntNumer);
                sqlCmd.CommandTimeout = 600;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                    {
                        wykorzystanie = Convert.ToDecimal(sqlDR["wykorzystanie"]);
                        poTerminie = Convert.ToDecimal(sqlDR["poterminie"]);
                        limit = Convert.ToDecimal(sqlDR["maks_limit"]);
                    }
                }

                sqlConn.Close();
            }
        }


        /// <summary>
        /// Pobiera próg wartościowy dla realizacji zamówień
        /// </summary>
        /// <param name="prog">Zwracany próg dla zwykłych zamówień</param>
        /// <param name="kemon">Zwracany próg dla zamówień Kemon</param>
        /// <param name="progMax">Zwracany próg MAX dla zwykłych zamówień</param>
        /// <param name="kemonMax">Zwaracany próg MAX dla zamówień Kemon</param>
        internal static void PobierzProgZS(out decimal prog, out decimal kemon, out decimal progMax, out decimal kemonMax)
        {
            prog = 0;
            kemon = 0;
            progMax = 0;
            kemonMax = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select SLW_WartoscS from cdn.Slowniki
                                        where SLW_Kategoria='Generator WZ' and SLW_Nazwa='Próg ZS'", sqlConn);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        prog = Convert.ToDecimal(sqlDR[0]);
                }

                sqlCmd = new SqlCommand(@"select SLW_WartoscS from cdn.Slowniki
                                        where SLW_Kategoria='Generator WZ' and SLW_Nazwa='Próg ZS Kemon'", sqlConn);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        kemon = Convert.ToDecimal(sqlDR[0]);
                }

                sqlCmd = new SqlCommand(@"select SLW_WartoscS from cdn.Slowniki
                                        where SLW_Kategoria='Generator WZ' and SLW_Nazwa='Próg ZS MAX'", sqlConn);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        progMax = Convert.ToDecimal(sqlDR[0]);
                }

                sqlCmd = new SqlCommand(@"select SLW_WartoscS from cdn.Slowniki
                                        where SLW_Kategoria='Generator WZ' and SLW_Nazwa='Próg ZS Kemon MAX'", sqlConn);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        kemonMax = Convert.ToDecimal(sqlDR[0]);
                }

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Pobiera ilość towaru wstawioną na WZ
        /// </summary>
        /// <param name="twrNumer">GIDNumer towaru</param>
        /// <param name="treNumer">GIDNumer pozycji WZ</param>
        /// <param name="treLp">GIDLp pozycji WZ</param>
        /// <param name="zamNumer">GIDNumer pozycji ZS</param>
        /// <param name="zamLp">GIDLp pozycji ZS</param>
        /// <returns>Ilość towaru</returns>
        internal static decimal PobierzIloscNaDokumencie(int twrNumer, int treNumer, int treLp, int zamNumer, int zamLp)
        {
            decimal result = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select isnull(sum(trs_ilosc),0) from cdn.traselem with(nolock) inner join cdn.traelem with(nolock) on tre_gidnumer=trs_gidnumer and tre_gidlp=trs_gidlp
                                            where tre_twrnumer=@TwrNumer and trs_gidnumer=@TreNumer and trs_gidlp=@TreLp and trs_zlcnumer=@ZamNumer and trs_zlclp=@ZamLp", sqlConn);
                sqlCmd.Parameters.AddWithValue("@TwrNumer", twrNumer);
                sqlCmd.Parameters.AddWithValue("@TreNumer", treNumer);
                sqlCmd.Parameters.AddWithValue("@TreLp", treLp);
                sqlCmd.Parameters.AddWithValue("@ZamNumer", zamNumer);
                sqlCmd.Parameters.AddWithValue("@ZamLp", zamLp);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToDecimal(sqlDR[0]);
                }

                sqlConn.Close();
            }

            return result;
        }

        /// <summary>
        /// Sprawdzenie dokumentu WZ przed wysłaniem do WMS
        /// </summary>
        /// <param name="trnTyp">GIDTyp WZ</param>
        /// <param name="trnNumer">GIDNumer WZ</param>
        /// <returns>Czy dokument jest poprawny</returns>
        internal static bool SprawdzPrzedWysylkaDoWMS(int trnTyp, int trnNumer)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                //sprawdzenie pozycji
                var sqlCmd = new SqlCommand(@"if exists(select * from cdn.TraElem with(nolock)
                                                        inner join cdn.TwrKarty with(nolock) on Twr_GIDNumer=TrE_TwrNumer
                                                        where TrE_GIDTyp=@GIDTyp and TrE_GIDNumer=@GIDNumer and Twr_Typ in (1,2))
                                                select 0
                                            else
                                                select 1", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", trnTyp);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", trnNumer);
                if (Convert.ToBoolean(sqlCmd.ExecuteScalar()))
                    throw new Exception("Na dokumencie WZ brak pozycji, które mogą być przesłane do WMS - wysyłka do WMS niemożliwa!");

                //sprawdzenie magazynów
                sqlCmd.CommandText = @"if exists(select 1 from cdn.traselem with(nolock)
                                                    inner join cdn.traelem on tre_gidnumer=trs_gidnumer and tre_gidlp=trs_gidlp
                                                    inner join cdn.magazyny on mag_gidnumer=trs_magnumer
                                                    where trs_gidtyp=@GIDTyp and trs_gidnumer=@GIDNumer and trs_ilosc<>0
                                                    having count(distinct mag_kod)>1)
                                            select 1
                                        else
                                            select 0";
                var wieleMag = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                if (wieleMag)
                    throw new Exception("Na dokumencie znajdują się pozycje z więcej niż 1 magazynem, wysyłka do WMS niemożliwa!");

                //porównanie mag. na pozycjach i nagłówku
                sqlCmd.CommandText = @"if exists(select 1 from cdn.tranag with(nolock)
                                                inner join cdn.traselem with(nolock) on trs_gidnumer=trn_gidnumer
                                                inner join cdn.magazyny on mag_gidnumer=trs_magnumer
                                                where trn_gidtyp=@GIDTyp and trn_gidnumer=@GIDNumer and trs_magnumer<>trn_magznumer)
                                            select 1
                                        else
                                            select 0";
                var niezgodneMag = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                if (niezgodneMag)
                    throw new Exception("Na dokumencie znajdują się pozycje z mag. innego niż w nagłówku, wysyłka do WMS niemożliwa!");

                if (trnTyp != 1603 && trnTyp != 1616)
                {
                    //sprawdzenie blokady transakcji kontrahenta
                    var blokadaTransakcji = false;
                    sqlCmd = new SqlCommand(@"select knt_blokadatransakcji from cdn.tranag with(nolock)
                                        inner join cdn.kntkarty with(nolock) on knt_gidnumer=trn_kntnumer
                                        where trn_gidtyp=@GIDTyp and trn_gidnumer=@GIDNumer", sqlConn);
                    sqlCmd.Parameters.AddWithValue("@GIDTyp", trnTyp);
                    sqlCmd.Parameters.AddWithValue("@GIDNumer", trnNumer);
                    using (var sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                            blokadaTransakcji = Convert.ToBoolean(sqlDR[0]);
                    }

                    if (blokadaTransakcji)
                        throw new Exception("Kontrahent ma zablokowane transakcje, wysyłka do WMS niemożliwa!");

                    //sprawdzenie danych kontrahenta
                    sqlCmd.CommandText = @"if exists(select 1 from cdn.TraNag with(nolock)
                                                    inner join cdn.KntKarty with(nolock) on Knt_GIDNumer=TrN_KntNumer
                                                    where TrN_GIDTyp=@GIDTyp and TrN_GIDNumer=@GIDNumer and Knt_PlatnikVat=1 and (isnull(Knt_Miasto,'')='' or isnull(Knt_Nip,'')='' or isnull(Knt_Nazwa1,'')='' or isnull(Knt_KodP,'')='')
                                                    )
                                            select 1
                                        else
                                            select 0";
                    var brakDanych = Convert.ToBoolean(sqlCmd.ExecuteScalar());
                    if (brakDanych)
                        throw new Exception("Na kontrahencie brakuje uzupełnionego miasta, kodu pocztowego, nazwy lub numeru NIP, wysyłka do WMS niemożliwa!");

                    sqlCmd.CommandText = @"if exists(select 1 from cdn.traelem with(nolock) where tre_gidtyp=@GIDTyp and tre_gidnumer=@GIDNumer and tre_ksiegowanetto+tre_ksiegowabrutto=0 and TrE_Nagroda=0)
                                            select 1
                                        else
                                            select 0";
                    var zeroweCeny = Convert.ToBoolean(sqlCmd.ExecuteScalar());
                    if (zeroweCeny)
                        throw new Exception("Na dokumencie znajdują się pozycje z zerową wartością, wysyłka do WMS niemożliwa!");

                    sqlCmd.CommandText = @"if exists(select * from cdn.TraNag with(nolock)
                                                    inner join cdn.KntAdresy with(nolock) on KnA_GIDTyp=TrN_AdwTyp and KnA_GIDNumer=TrN_AdwNumer
                                                    where TrN_GIDTyp=@GIDTyp and TrN_GIDNumer=@GIDNumer and (isnull(KnA_Miasto,'')='' or isnull(KnA_Ulica,'')='' or isnull(KnA_Nazwa1,'')='' or isnull(KnA_KodP,'')='')
                                                    )
                                            select 1
                                        else
                                            select 0";
                    var brakDanychAdresu = Convert.ToBoolean(sqlCmd.ExecuteScalar());
                    if (brakDanychAdresu)
                        throw new Exception("Na adresie docelowym brakuje uzupełnionego miasta, ulicy, kodu pocztowego lub nazwy, wysyłka do WMS niemożliwa!");
                }

                sqlConn.Close();
            }

            return true;
        }

        /// <summary>
        /// Wysłanie WZ do WMS
        /// </summary>
        /// <param name="companyId">Id firmy</param>
        /// <param name="trnTyp">GIDTyp WZ</param>
        /// <param name="trnFirm">GIDFirma WZ</param>
        /// <param name="trnNumer">GIDNumer WZ</param>
        /// <param name="opeIdent">Operator</param>
        internal static void WyslijDoWMS(int companyId, int trnTyp, int trnFirm, int trnNumer, string opeIdent)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var opeNumer = 0;
                var sqlCmd = new SqlCommand(@"select Ope_GIDNumer from cdn.OpeKarty with(nolock) where Ope_Ident=@Operator", sqlConn);
                sqlCmd.Parameters.Add("@Operator", SqlDbType.VarChar, 8).Value = opeIdent;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        opeNumer = Convert.ToInt32(sqlDR[0]);
                }

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    sqlCmd = new SqlCommand(@"insert into dbo.INT_Pozycje2
                                            select trs_gidnumer,trs_gidtyp,trs_gidlp,trs_subgidlp,trs_ilosc,getdate(),tre_pozycja,tre_twrkod,tre_twrnumer,'POLWELL',isnull(dst_cecha,'')
                                            from cdn.traelem with(nolock)
                                            inner join cdn.traselem with(nolock) on trs_gidnumer=tre_gidnumer and trs_gidlp=tre_gidlp
                                            inner join cdn.dostawy with(nolock) on dst_gidnumer=trs_dstnumer
                                            where tre_gidtyp=@GIDTyp and tre_gidnumer=@GIDNumer and tre_typtwr in (1,2,4)", sqlConn, sqlTran);
                    sqlCmd.Parameters.AddWithValue("@GIDTyp", trnTyp);
                    sqlCmd.Parameters.AddWithValue("@GIDNumer", trnNumer);
                    sqlCmd.ExecuteNonQuery();

                    sqlCmd.CommandText = @"update cdn.tranag set trn_cechaopis='wysłano do WMS' where trn_gidtyp=@GIDTyp and trn_gidnumer=@GIDNumer";
                    sqlCmd.ExecuteNonQuery();

                    sqlCmd.CommandText =
                        @"insert into dbo.int_messagesprod2(CompanyID,XLID,WMSID,[Type],GIDTyp,GIDNumer,GIDTyp2,GIDNumer2,MagZNumer,MagDNumer,CreateDT,ReadDT,LastReadTry,TryCount,FinishDT,ServerName,Info,Priority,Property1,Property2)
                                        select @CompanyID,@XLID,@WMSID,@Type,@GIDTyp,@GIDNumer,0,0,trn_magznumer,trn_magdnumer,getdate(),null,null,0,null,@@servername,
                                        cdn.numerdokumentu(trn_gidtyp,trn_spityp,trn_trntyp,trn_trnnumer,trn_trnrok,trn_trnseria,trn_trnmiesiac),null,@Operator,null
                                        from cdn.tranag where trn_gidtyp=@GIDTyp and trn_gidnumer=@GIDNumer";
                    sqlCmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                    sqlCmd.Parameters.Add("@XLID", SqlDbType.Int).Value = Properties.Settings.Default.INTXLID;
                    sqlCmd.Parameters.Add("@WMSID", SqlDbType.Int).Value = Properties.Settings.Default.INTWMSID;
                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar, 50);
                    switch (trnTyp)
                    {
                        case 2001:
                            sqlCmd.Parameters["@Type"].Value = "ORDER";
                            break;
                        case 2005:
                            sqlCmd.Parameters["@Type"].Value = "ORDER_EXP";
                            break;
                        case 1603:
                            sqlCmd.Parameters["@Type"].Value = "MOVE_OUT";
                            break;
                        case 2036:
                            sqlCmd.Parameters["@Type"].Value = "ORDER_INTERNAL_INV";
                            break;
                        case 1616:
                            sqlCmd.Parameters["@Type"].Value = "STORE_OUT";
                            break;
                        case 2034:
                            sqlCmd.Parameters["@Type"].Value = "ORDER_BILL";
                            break;
                    }
                    sqlCmd.Parameters.Add("@Operator", SqlDbType.VarChar, 8).Value = opeIdent;
                    sqlCmd.ExecuteNonQuery();

                    SetAttrValue(trnTyp, trnFirm, trnNumer, 0, "WMS Storno", "", opeNumer, sqlConn, sqlTran);
                    SetAttrValue(trnTyp, trnFirm, trnNumer, 0, "WMS", "Wysłano do WMS", opeNumer, sqlConn, sqlTran);
                    SetAttrValue(trnTyp, trnFirm, trnNumer, 0, "WMS Status", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Wysłano do WMS", opeNumer, sqlConn, sqlTran);

                    sqlTran.Commit();
                }

                sqlConn.Close();
            }
        }

        internal static void SendMessageToInt(int companyId, int trnTyp, int trnNumer, string opeIdent)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var opeNumer = 0;
                var sqlCmd = new SqlCommand(@"select Ope_GIDNumer from cdn.OpeKarty with(nolock) where Ope_Ident=@Operator", sqlConn);
                sqlCmd.Parameters.Add("@Operator", SqlDbType.VarChar, 8).Value = opeIdent;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        opeNumer = Convert.ToInt32(sqlDR[0]);
                }

                sqlCmd = new SqlCommand(@"insert into dbo.int_messagesprod2(CompanyID,XLID,WMSID,[Type],GIDTyp,GIDNumer,GIDTyp2,GIDNumer2,MagZNumer,MagDNumer,CreateDT,ReadDT,LastReadTry,TryCount,FinishDT,ServerName,Info,Priority,Property1,Property2)
                                        select @CompanyID,@XLID,@WMSID,'SEND_WZ_TO_WMS_CONF',@GIDTyp,@GIDNumer,0,0,trn_magznumer,trn_magdnumer,getdate(),null,null,0,null,@@servername,
                                        cdn.numerdokumentu(trn_gidtyp,trn_spityp,trn_trntyp,trn_trnnumer,trn_trnrok,trn_trnseria,trn_trnmiesiac),null,@Operator,null
                                        from cdn.tranag where trn_gidtyp=@GIDTyp and trn_gidnumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", trnTyp);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", trnNumer);
                sqlCmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                sqlCmd.Parameters.Add("@XLID", SqlDbType.Int).Value = Properties.Settings.Default.INTXLID;
                sqlCmd.Parameters.Add("@WMSID", SqlDbType.Int).Value = Properties.Settings.Default.INTWMSID;
                sqlCmd.Parameters.Add("@Operator", SqlDbType.VarChar, 8).Value = opeIdent;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static void SetAttrValue(int gidType, int gidFirm, int gidNumber, int gidLp, string attrName, string attrValue, int opeNumber, SqlConnection sqlConn, SqlTransaction sqlTran, int attrType = 0, int attrFirm = 0, int attrNumber = 0, int attrLp = 0)
        {
            int atrId = 0, atkId = 0;
            var historia = false;

            var sqlCmd = new SqlCommand(@"select AtK_ID, AtK_Historia from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa=@AtkNazwa", sqlConn, sqlTran);
            sqlCmd.Parameters.AddWithValue("@AtkNazwa", attrName);
            using (var sqlDR = sqlCmd.ExecuteReader())
            {
                if (sqlDR.Read())
                {
                    atkId = Convert.ToInt32(sqlDR[0]);
                    if (Convert.ToInt32(sqlDR[1]) > 0)
                        historia = true;
                }
            }

            if (atkId == 0)
                throw new Exception("Nie znaleziono klasy atrybutu!");

            sqlCmd = new SqlCommand(@"select Atr_ID from cdn.Atrybuty with(nolock) 
                                                where Atr_ObiTyp=@GIDTyp and Atr_ObiFirma=@GIDFirma and Atr_ObiNumer=@GIDNumer and Atr_ObiLp=@GIDLp and Atr_ObiSubLp=0 and Atr_AtkId=@AtkId", sqlConn, sqlTran);
            sqlCmd.Parameters.AddWithValue("@GIDTyp", gidType);
            sqlCmd.Parameters.AddWithValue("@GIDFirma", gidFirm);
            sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumber);
            sqlCmd.Parameters.AddWithValue("@GIDLp", gidLp);
            sqlCmd.Parameters.AddWithValue("@AtkId", atkId);
            using (var sqlDR = sqlCmd.ExecuteReader())
            {
                if (sqlDR.Read())
                    atrId = Convert.ToInt32(sqlDR[0]);
            }

            if (atrId > 0)
            {
                sqlCmd = new SqlCommand(@"update cdn.Atrybuty set Atr_Wartosc=@Wartosc, Atr_AtrTyp=@AtrTyp, Atr_AtrFirma=@AtrFirma, Atr_AtrNumer=@AtrNumer, Atr_AtrLp=@AtrLp where Atr_Id=@AtrId", sqlConn, sqlTran);
                sqlCmd.Parameters.AddWithValue("@AtrId", atrId);
                sqlCmd.Parameters.AddWithValue("@Wartosc", attrValue);
                sqlCmd.Parameters.AddWithValue("@AtrTyp", attrType);
                sqlCmd.Parameters.AddWithValue("@AtrFirma", attrFirm);
                sqlCmd.Parameters.AddWithValue("@AtrNumer", attrNumber);
                sqlCmd.Parameters.AddWithValue("@AtrLp", attrLp);
                sqlCmd.CommandTimeout = 180;
                sqlCmd.ExecuteNonQuery();
            }
            else
            {
                sqlCmd = new SqlCommand(@"insert into cdn.Atrybuty (Atr_ObiTyp,Atr_ObiFirma,Atr_ObiNumer,Atr_ObiLp,Atr_ObiSubLp,Atr_AtkId,Atr_Wartosc,Atr_AtrTyp,Atr_AtrFirma,Atr_AtrNumer,Atr_AtrLp,Atr_AtrSubLp,Atr_OptimaId,Atr_Grupujacy)
                                                            values(@ObiTyp,@ObiFirma,@ObiNumer,@ObiLp,0,@AtkId,@Wartosc,@AtrTyp,@AtrFirma,@AtrNumer,@AtrLp,0,0,0);
                                                            select scope_identity()", sqlConn, sqlTran);
                sqlCmd.Parameters.AddWithValue("@ObiTyp", gidType);
                sqlCmd.Parameters.AddWithValue("@ObiFirma", gidFirm);
                sqlCmd.Parameters.AddWithValue("@ObiNumer", gidNumber);
                sqlCmd.Parameters.AddWithValue("@ObiLp", gidLp);
                sqlCmd.Parameters.AddWithValue("@AtkId", atkId);
                sqlCmd.Parameters.AddWithValue("@Wartosc", attrValue);
                sqlCmd.Parameters.AddWithValue("@AtrTyp", attrType);
                sqlCmd.Parameters.AddWithValue("@AtrFirma", attrFirm);
                sqlCmd.Parameters.AddWithValue("@AtrNumer", attrNumber);
                sqlCmd.Parameters.AddWithValue("@AtrLp", attrLp);
                sqlCmd.CommandTimeout = 180;
                atrId = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }

            if (historia)
            {
                sqlCmd = new SqlCommand(@"insert into cdn.AtrybutyHist (AtH_Id,AtH_ObiTyp,AtH_ObiFirma,AtH_ObiNumer,AtH_ObiLp,AtH_ObiSubLp,AtH_AtkId,AtH_Wartosc,
							                                                        AtH_AtrTyp,AtH_AtrFirma,AtH_AtrNumer,AtH_AtrLp,AtH_AtrSubLp,AtH_TimeStamp,AtH_OpeTyp,AtH_OpeFirma,AtH_OpeNumer,AtH_OpeLp,AtH_TimeStampDo)
                                                        values (@AtrId,@ObiTyp,@ObiFirma,@ObiNumer,@ObiLp,0,@AtkId,@Wartosc,
		                                                        @AtrTyp,@AtrFirma,@AtrNumer,@AtrLp,0,datediff(second,'19900101',getdate()),128,@ObiFirma,@OpeNumer,0,datediff(second,'19900101',getdate()))", sqlConn, sqlTran);
                sqlCmd.Parameters.AddWithValue("@AtrId", atrId);
                sqlCmd.Parameters.AddWithValue("@ObiTyp", gidType);
                sqlCmd.Parameters.AddWithValue("@ObiFirma", gidFirm);
                sqlCmd.Parameters.AddWithValue("@ObiNumer", gidNumber);
                sqlCmd.Parameters.AddWithValue("@ObiLp", gidLp);
                sqlCmd.Parameters.AddWithValue("@AtkId", atkId);
                sqlCmd.Parameters.AddWithValue("@Wartosc", attrValue);
                sqlCmd.Parameters.AddWithValue("@AtrTyp", attrType);
                sqlCmd.Parameters.AddWithValue("@AtrFirma", attrFirm);
                sqlCmd.Parameters.AddWithValue("@AtrNumer", attrNumber);
                sqlCmd.Parameters.AddWithValue("@AtrLp", attrLp);
                sqlCmd.Parameters.AddWithValue("@OpeNumer", opeNumber);
                sqlCmd.CommandTimeout = 180;
                sqlCmd.ExecuteNonQuery();
            }
        }

        internal static void SetAttrValue(int gidType, int gidFirm, int gidNumber, int gidLp, string attrName, string attrValue, string opeIdent, int attrType = 0, int attrFirm = 0, int attrNumber = 0, int attrLp = 0)
        {
            int atrId = 0, atkId = 0;
            var historia = false;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    var opeNumber = 0;
                    var sqlCmd = new SqlCommand(@"select Ope_GIDNumer from cdn.OpeKarty with(nolock) where Ope_Ident=@Operator", sqlConn, sqlTran);
                    sqlCmd.Parameters.Add("@Operator", SqlDbType.VarChar, 8).Value = opeIdent;
                    using (var sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                            opeNumber = Convert.ToInt32(sqlDR[0]);
                    }

                    sqlCmd = new SqlCommand(@"select AtK_ID, AtK_Historia from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa=@AtkNazwa", sqlConn, sqlTran);
                    sqlCmd.Parameters.AddWithValue("@AtkNazwa", attrName);
                    using (var sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            atkId = Convert.ToInt32(sqlDR[0]);
                            if (Convert.ToInt32(sqlDR[1]) > 0)
                                historia = true;
                        }
                    }

                    if (atkId == 0)
                        throw new Exception("Nie znaleziono klasy atrybutu!");

                    sqlCmd = new SqlCommand(@"select Atr_ID from cdn.Atrybuty with(nolock) 
                                                where Atr_ObiTyp=@GIDTyp and Atr_ObiFirma=@GIDFirma and Atr_ObiNumer=@GIDNumer and Atr_ObiLp=@GIDLp and Atr_ObiSubLp=0 and Atr_AtkId=@AtkId", sqlConn, sqlTran);
                    sqlCmd.Parameters.AddWithValue("@GIDTyp", gidType);
                    sqlCmd.Parameters.AddWithValue("@GIDFirma", gidFirm);
                    sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumber);
                    sqlCmd.Parameters.AddWithValue("@GIDLp", gidLp);
                    sqlCmd.Parameters.AddWithValue("@AtkId", atkId);
                    using (var sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                            atrId = Convert.ToInt32(sqlDR[0]);
                    }

                    if (atrId > 0)
                    {
                        sqlCmd = new SqlCommand(@"update cdn.Atrybuty set Atr_Wartosc=@Wartosc, Atr_AtrTyp=@AtrTyp, Atr_AtrFirma=@AtrFirma, Atr_AtrNumer=@AtrNumer, Atr_AtrLp=@AtrLp where Atr_Id=@AtrId", sqlConn, sqlTran);
                        sqlCmd.Parameters.AddWithValue("@AtrId", atrId);
                        sqlCmd.Parameters.AddWithValue("@Wartosc", attrValue);
                        sqlCmd.Parameters.AddWithValue("@AtrTyp", attrType);
                        sqlCmd.Parameters.AddWithValue("@AtrFirma", attrFirm);
                        sqlCmd.Parameters.AddWithValue("@AtrNumer", attrNumber);
                        sqlCmd.Parameters.AddWithValue("@AtrLp", attrLp);
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        sqlCmd = new SqlCommand(@"insert into cdn.Atrybuty (Atr_ObiTyp,Atr_ObiFirma,Atr_ObiNumer,Atr_ObiLp,Atr_ObiSubLp,Atr_AtkId,Atr_Wartosc,Atr_AtrTyp,Atr_AtrFirma,Atr_AtrNumer,Atr_AtrLp,Atr_AtrSubLp,Atr_OptimaId,Atr_Grupujacy)
                                                            values(@ObiTyp,@ObiFirma,@ObiNumer,@ObiLp,0,@AtkId,@Wartosc,@AtrTyp,@AtrFirma,@AtrNumer,@AtrLp,0,0,0);
                                                            select scope_identity()", sqlConn, sqlTran);
                        sqlCmd.Parameters.AddWithValue("@ObiTyp", gidType);
                        sqlCmd.Parameters.AddWithValue("@ObiFirma", gidFirm);
                        sqlCmd.Parameters.AddWithValue("@ObiNumer", gidNumber);
                        sqlCmd.Parameters.AddWithValue("@ObiLp", gidLp);
                        sqlCmd.Parameters.AddWithValue("@AtkId", atkId);
                        sqlCmd.Parameters.AddWithValue("@Wartosc", attrValue);
                        sqlCmd.Parameters.AddWithValue("@AtrTyp", attrType);
                        sqlCmd.Parameters.AddWithValue("@AtrFirma", attrFirm);
                        sqlCmd.Parameters.AddWithValue("@AtrNumer", attrNumber);
                        sqlCmd.Parameters.AddWithValue("@AtrLp", attrLp);
                        sqlCmd.CommandTimeout = 180;
                        atrId = Convert.ToInt32(sqlCmd.ExecuteScalar());
                    }

                    if (historia)
                    {
                        sqlCmd = new SqlCommand(@"insert into cdn.AtrybutyHist (AtH_Id,AtH_ObiTyp,AtH_ObiFirma,AtH_ObiNumer,AtH_ObiLp,AtH_ObiSubLp,AtH_AtkId,AtH_Wartosc,
							                                                        AtH_AtrTyp,AtH_AtrFirma,AtH_AtrNumer,AtH_AtrLp,AtH_AtrSubLp,AtH_TimeStamp,AtH_OpeTyp,AtH_OpeFirma,AtH_OpeNumer,AtH_OpeLp,AtH_TimeStampDo)
                                                        values (@AtrId,@ObiTyp,@ObiFirma,@ObiNumer,@ObiLp,0,@AtkId,@Wartosc,
		                                                        @AtrTyp,@AtrFirma,@AtrNumer,@AtrLp,0,datediff(second,'19900101',getdate()),128,@ObiFirma,@OpeNumer,0,datediff(second,'19900101',getdate()))", sqlConn, sqlTran);
                        sqlCmd.Parameters.AddWithValue("@AtrId", atrId);
                        sqlCmd.Parameters.AddWithValue("@ObiTyp", gidType);
                        sqlCmd.Parameters.AddWithValue("@ObiFirma", gidFirm);
                        sqlCmd.Parameters.AddWithValue("@ObiNumer", gidNumber);
                        sqlCmd.Parameters.AddWithValue("@ObiLp", gidLp);
                        sqlCmd.Parameters.AddWithValue("@AtkId", atkId);
                        sqlCmd.Parameters.AddWithValue("@Wartosc", attrValue);
                        sqlCmd.Parameters.AddWithValue("@AtrTyp", attrType);
                        sqlCmd.Parameters.AddWithValue("@AtrFirma", attrFirm);
                        sqlCmd.Parameters.AddWithValue("@AtrNumer", attrNumber);
                        sqlCmd.Parameters.AddWithValue("@AtrLp", attrLp);
                        sqlCmd.Parameters.AddWithValue("@OpeNumer", opeNumber);
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.ExecuteNonQuery();
                    }

                    sqlTran.Commit();
                }
            }
        }

        /// <summary>
        /// Zmiana mag. na rezerwacjach ZS
        /// </summary>
        /// <param name="zamTyp">GIDTyp ZS</param>
        /// <param name="zamNumer">GIDNumer ZS</param>
        /// <param name="magKod">Kod magazynu</param>
        /// <param name="token">Token transakcji</param>
        internal static void ZmienMagNaRezerwacjach(int zamTyp, int zamNumer, string magKod, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"declare @MagNumer int
                                        select @MagNumer=MAG_GIDNumer from cdn.Magazyny where MAG_Kod=@Magazyn

                                        delete from cdn.rezmagazyny where rem_zrdtyp=@GIDTyp and rem_zrdnumer=@GIDNumer and rem_reztyp=2576;
                                        update cdn.rezerwacje set rez_magnumer=@MagNumer where rez_zrdtyp=@GIDTyp and rez_zrdnumer=@GIDNumer and rez_typ=1 and rez_gidtyp=2576;
                                        insert into cdn.rezmagazyny(ReM_RezTyp,ReM_RezFirma,ReM_RezNumer,ReM_RezLp,ReM_MagTyp,ReM_MagFirma,ReM_MagNumer,ReM_MagLp,ReM_ZrdTyp,ReM_ZrdFirma,ReM_ZrdNumer,ReM_ZrdLp)
                                        select distinct rez_gidtyp,rez_gidfirma,rez_gidnumer,rez_gidlp,rez_magtyp,rez_magfirma,rez_magnumer,rez_maglp,rez_zrdtyp,rez_zrdfirma,rez_zrdnumer,rez_zrdlp
                                        from cdn.rezerwacje where rez_zrdtyp=@GIDTyp and rez_zrdnumer=@GIDNumer and rez_typ=1 and rez_gidtyp=2576", sqlConn);
                sqlCmd.Parameters.Add("@Magazyn", SqlDbType.VarChar, 10).Value = magKod;
                sqlCmd.Parameters.AddWithValue("@GIDTyp", zamTyp);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", zamNumer);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Aktualizacja właściciela na ZS
        /// </summary>
        /// <param name="gidNumer">GIDNumer ZS</param>
        /// <param name="frsId">ID centrum struktury, która ma być właścicielem</param>
        /// <param name="token">Token transakcji</param>
        internal static void AktualizujZanFrsId(int gidNumer, int frsId, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"update cdn.ZamNag set ZaN_FrSID=@FrsId, ZaN_Aktywny=ZaN_Aktywny where ZaN_GIDNumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@FrsId", frsId);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Aktualizacja rejestru kasowo/bankowego na WZ
        /// </summary>
        /// <param name="gidTyp">GIDTyp WZ</param>
        /// <param name="gidNumer">GIDNumer WZ</param>
        /// <param name="karNumer">GIDNumer rejestru</param>
        /// <param name="token">Token transakcji</param>
        internal static void AktualizujRejestr(int gidTyp, int gidNumer, int karNumer, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"update cdn.tranag set trn_karnumer=@KarNumer where trn_gidnumer=@GIDNumer;
                                            update cdn.traplat set trp_karnumer=@KarNumer where trp_gidtyp=@GIDTyp and trp_gidnumer=@GIDNumer;", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", gidTyp);
                sqlCmd.Parameters.AddWithValue("@KarNumer", karNumer);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Pobranie rejestru kasowo/bankowego z ZS
        /// </summary>
        /// <param name="zamNumer">GIDNumer ZS</param>
        /// <returns>GIDNumer rejestru</returns>
        internal static int PobierzRejestrZS(int zamNumer)
        {
            var result = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select isnull(zan_karnumer,0) from cdn.zamnag with(nolock) where zan_gidnumer=@ZamNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@ZamNumer", zamNumer);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToInt32(sqlDR[0]);
                }

                sqlConn.Close();
            }

            return result;
        }

        /// <summary>
        /// Pobranie rejestrów kasowo/bankowych z WZ
        /// </summary>
        /// <param name="gidTyp">GIDTyp WZ</param>
        /// <param name="gidNumer">GIDNumer WZ</param>
        /// <param name="trnKar">GIDNumer rejestru z TraNag</param>
        /// <param name="trpKar">GIDNumer rejestru z TraPlat</param>
        internal static void PobierzRejestryWZ(int gidTyp, int gidNumer, out int trnKar, out int trpKar)
        {
            trnKar = 0;
            trpKar = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select trn_karnumer from cdn.tranag with(nolock) where trn_gidnumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        trnKar = Convert.ToInt32(sqlDR[0]);
                }

                sqlCmd = new SqlCommand(@"select trp_karnumer from cdn.traplat with(nolock) where trp_gidtyp=@GIDTyp and trp_gidnumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", gidTyp);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        trpKar = Convert.ToInt32(sqlDR[0]);
                }

                sqlConn.Close();
            }
        }

        /// <summary>
        /// Pobranie rejestrów PH
        /// </summary>
        /// <returns>Lista rejestrów PH</returns>
        internal static List<RejestrPH> PobierzRejestryPH()
        {
            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"select ID, Rejestr, Prog, isnull(ProgMax,0.0) as ProgMax from dbo.GWZ_RejestryPH with(nolock) order by ID", sqlConn))
                    sqlDA.Fill(dt);
            }


            return (from DataRow r in dt.Rows
                    select new RejestrPH
                    {
                        ID = Convert.ToInt32(r["ID"]),
                        Rejestr = r["Rejestr"].ToString(),
                        Prog = Convert.ToDecimal(r["Prog"]),
                        ProgMax = Convert.ToDecimal(r["ProgMax"])
                    }).ToList();
        }

        internal static void WyslijPowiadomienieOPrzedplacie(int companyId, int zamTyp, int zamNumer, string opeIdent)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(
                    @"insert into dbo.INT_MessagesProd2(CompanyID,XLID,WMSID,Type,GIDTyp,GIDNumer,GIDTyp2,GIDNumer2,MagZNumer,MagDNumer,CreateDT,TryCount,ServerName,Info,Priority,Property1,Property2)
                                            select @CompanyID,@XLID,@WMSID,'SEND_EMAIL_PREPAYMENT_CONF',ZaN_GIDTyp,ZaN_GIDNumer,0,0,ZaN_MagNumer,ZaN_MagDNumer,GETDATE(),0,@@SERVERNAME,
                                                CDN.NumerDokumentu(CDN.DokMapTypDokumentu(ZaN_GIDTyp,ZaN_ZamTyp,ZaN_Rodzaj),0,0,ZaN_ZamNumer,ZaN_ZamRok,ZaN_ZamSeria,ZaN_ZamMiesiac),null,@Operator,null
                                            from cdn.ZamNag where ZaN_GIDTyp=@GIDTyp and ZaN_GIDNumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyId;
                sqlCmd.Parameters.Add("@XLID", SqlDbType.Int).Value = Properties.Settings.Default.INTXLID;
                sqlCmd.Parameters.Add("@WMSID", SqlDbType.Int).Value = Properties.Settings.Default.INTWMSID;
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.Int).Value = zamTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = zamNumer;
                sqlCmd.Parameters.Add("@Operator", SqlDbType.VarChar, 8).Value = opeIdent;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static void AktualizujTermin(int gidTyp, int gidNumer, int termin, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"UPDATE CDN.TraNag
                                            SET TrN_Termin = @Termin
                                            WHERE TrN_GIDTyp = @GIDTyp
                                                    AND TrN_GIDNumer = @GIDNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", gidTyp);
                sqlCmd.Parameters.AddWithValue("@Termin", termin);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static void UstawAtrybutPrzelewy24(int gidTyp, int gidNumer, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"exec [CDN].[Polwell_Przelew24_link] @Gid,@Typ", sqlConn);
                sqlCmd.Parameters.Add("@Gid", SqlDbType.Int).Value = gidNumer;
                sqlCmd.Parameters.Add("@Typ", SqlDbType.Int).Value = gidTyp;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static DocumentPos WyszukajZamiennik(int twrNumer)
        {
            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"select t.Twr_GIDNumer as TwrNumer, t.Twr_Kod as Kod from [dbo].[func_WyszukajZamienniki] (@TwrNumer) as z
                                                        inner join cdn.TwrKarty t with(nolock) on t.Twr_GIDNumer=z.ZamNumer", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@TwrNumer", SqlDbType.Int).Value = twrNumer;
                    sqlDA.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
                return null;

            var r = dt.Rows[0];
            return new DocumentPos
            {
                TwrNumber = Convert.ToInt32(r["TwrNumer"]),
                TwrCode = r["Kod"].ToString()
            };
        }

        internal static void ModyfikujOpisDokumentu(int gidTyp, int gidFirma, int gidNumer, string opis)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                if (opis.Length > 2000)
                    opis = opis.Substring(0, 2000);

                var sqlCmd = new SqlCommand(@"if exists(select 1 from cdn.tranag where trn_gidtyp=@GIDTyp and trn_gidfirma=@GIDFirma and trn_gidnumer=@GIDNumer and trn_gidlp=@GIDLp)
                                                        select 1
                                                    else
                                                        select 0", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", gidTyp);
                sqlCmd.Parameters.AddWithValue("@GIDFirma", gidFirma);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@GIDLp", 0);
                if (!Convert.ToBoolean(sqlCmd.ExecuteScalar()))
                    throw new Exception("Nie znaleziono dokumentu");

                sqlCmd = new SqlCommand(@"if exists(select 1 from cdn.trnopisy where tno_trntyp=@GIDTyp and tno_trnfirma=@GIDFirma and tno_trnnumer=@GIDNumer and tno_trnlp=@GIDLp)
                                                        select 1
                                                    else
                                                        select 0", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", gidTyp);
                sqlCmd.Parameters.AddWithValue("@GIDFirma", gidFirma);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@GIDLp", 0);
                var exists = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                if (exists)
                    sqlCmd = new SqlCommand(@"update cdn.trnopisy set tno_opis=@Opis where tno_trntyp=@GIDTyp and tno_trnfirma=@GIDFirma and tno_trnnumer=@GIDNumer and tno_trnlp=@GIDLp", sqlConn);
                else
                    sqlCmd = new SqlCommand(@"insert into cdn.trnopisy (tno_trntyp,tno_trnfirma,tno_trnnumer,tno_trnlp,tno_typ,tno_opis)
                                                            values(@GIDTyp,@GIDFirma,@GIDNumer,@GIDLp,0,@Opis)", sqlConn);
                sqlCmd.Parameters.AddWithValue("@Opis", opis);
                sqlCmd.Parameters.AddWithValue("@GIDTyp", gidTyp);
                sqlCmd.Parameters.AddWithValue("@GIDFirma", gidFirma);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", gidNumer);
                sqlCmd.Parameters.AddWithValue("@GIDLp", 0);
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();

            }
        }

        internal static bool SprawdzPromocje(int zamNumer, out string error)
        {
            error = "";

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select [CDN].[_polwell_Trigger_ZAMNAG_SP500](960, @GIDNumer)", sqlConn);
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = zamNumer;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        error = sqlDR[0] != DBNull.Value ? sqlDR[0].ToString() : "";
                }

                sqlConn.Close();
            }

            if (string.IsNullOrEmpty(error))
                return true;
            else
                return false;
        }

        internal static DocumentPos WyszukajGratis(string atrybut)
        {
            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"select Twr_GIDNumer as TwrNumer, Twr_Kod as Kod, Atr_Wartosc as Cena
                                                        into #tmp
                                                        from cdn.TwrKarty with(nolock)
                                                        inner join cdn.Atrybuty with(nolock) on Atr_ObiTyp=Twr_GIDTyp and Atr_ObiNumer=Twr_GIDNumer and Atr_ObiLp=0 and Atr_ObiSubLp=0
                                                        inner join cdn.AtrybutyKlasy with(nolock) on Atk_Id=Atr_AtkId
                                                        where AtK_Nazwa=@Atrybut and isnull(Atr_Wartosc,'')<>'' and isnumeric(Atr_Wartosc)=1

                                                        select TwrNumer,Kod,cast(Cena as decimal(5,2)) as Cena from #tmp
                                                        where cast(Cena as decimal(5,2))>0

                                                        drop table #tmp", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@Atrybut", SqlDbType.VarChar, 256).Value = atrybut;
                    sqlDA.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
                return null;

            var r = dt.Rows[0];
            return new DocumentPos
            {
                TwrNumber = Convert.ToInt32(r["TwrNumer"]),
                TwrCode = r["Kod"].ToString(),
                Quantity = 1,
                Price = Convert.ToDecimal(r["Cena"])
            };
        }

        internal static void ModyfikujAdresDocelowy(int gidTyp, int gidNumer, int adwTyp, int adwNumer, string token = "")
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                SqlCommand sqlCmd;
                if (token != "")
                {
                    sqlCmd = new SqlCommand("exec sp_bindsession '" + token + "'", sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd = new SqlCommand(@"update cdn.TraNag set TrN_AdwTyp=@AdwTyp, TrN_AdwNumer=@AdwNumer where TrN_GIDTyp=@GIDTyp and TrN_GIDNumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.Add("@AdwTyp", SqlDbType.SmallInt).Value = adwTyp;
                sqlCmd.Parameters.Add("@AdwNumer", SqlDbType.Int).Value = adwNumer;
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = gidTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static bool SprawdzCzyWZIstnieje(int gidTyp, int gidNumer)
        {
            bool result = false;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"if exists(select * from cdn.TraNag with(nolock) where TrN_GIDTyp=@GIDTyp and TrN_GIDNumer=@GIDNumer) select 1 else select 0", sqlConn);
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = gidTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                result = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                sqlConn.Close();
            }

            return result;
        }

        internal static int PobierzTerminPlZPromocji(int gidNumer)
        {
            int result = -1;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"if exists(select * from cdn.ZamElem with(nolock) where ZaE_GIDNumer=@GIDNumer and ZaE_PakietId=4412)
	                                            select 30
                                            else if exists(select * from cdn.ZamElem with(nolock) where ZaE_GIDNumer=@GIDNumer and ZaE_PakietId=4413)
	                                            select 14
                                            else
	                                            select -1", sqlConn);
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());

                sqlConn.Close();
            }

            return result;
        }

        internal static List<DocumentPos> PobierzGratisyZPromocji(int kntNumer, decimal nettoZam)
        {
            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"declare @atkId int
                                                        select @atkId=AtK_Id from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa='GeneratorWZ_Gratis'

                                                        select Twr_GIDNumer as TwrNumer, Twr_Kod as Kod, GPR_Ilosc as Ilosc, GPR_Wartosc as Cena, Atr_Wartosc as Prog
                                                        from cdn.PrmKarty with(nolock)
                                                        inner join cdn.Atrybuty with(nolock) on Atr_ObiTyp=5104 and Atr_ObiNumer=PRM_Id and Atr_ObiLp=0 and Atr_ObiSubLp=0 and Atr_AtkId=@atkId
                                                        inner join cdn.GratisyPromocje with(nolock) on GPR_PrmId=PRM_Id
                                                        inner join cdn.TwrKarty with(nolock) on Twr_GIDNumer=GPR_TwrNumer
                                                        where isnull(Atr_Wartosc,'')<>'' and isnumeric(Atr_Wartosc)=1 and PRM_Stan=1
                                                        and cdn.CzyPromocjaObowiazuje(PRM_DataOd, PRM_DataDo,PRM_Cykliczna,DATEDIFF(second,'19900101',getdate()),PRM_CyklRodzaj,PRM_CyklCzestotliwosc,PRM_CyklWystepowanie,PRM_CyklDniOd,PRM_CyklDniDo)=1
                                                        and (exists(select * from cdn.KntPromocje with(nolock) where KPR_PrmId=PRM_Id and KPR_KntTyp=32 and KPR_KntNumer=@KntNumer)
                                                        or exists(select * from cdn.KntPromocje with(nolock)
			                                                        inner join cdn.KntLinki with(nolock) on KnL_GrOTyp=KPR_KntTyp and KnL_GrONumer=KPR_KntNumer
			                                                        where KPR_PrmId=PRM_Id and KPR_KntTyp=-32 and KnL_GIDTyp=32 and KnL_GIDNumer=@KntNumer)
                                                        )", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@KntNumer", SqlDbType.Int).Value = kntNumer;
                    sqlDA.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
                return null;

            var result = new List<DocumentPos>();
            foreach (DataRow r in dt.Rows)
            {
                var nettoS = r["Prog"].ToString();
                decimal nettoD;
                if (!decimal.TryParse(nettoS.Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out nettoD))
                {
                    if (!decimal.TryParse(nettoS.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out nettoD))
                        nettoD = -1.0m;
                }

                if (nettoD > -1.0m && nettoZam >= nettoD)
                {
                    result.Add(new DocumentPos
                    {
                        TwrNumber = Convert.ToInt32(r["TwrNumer"]),
                        TwrCode = r["Kod"].ToString(),
                        Quantity = Convert.ToDecimal(r["Ilosc"]),
                        Price = Convert.ToDecimal(r["Cena"])
                    });
                }
            }

            return result;
        }

        internal static bool SprawdzStan(int twrNumer, string magKod, decimal ilosc)
        {
            bool result = false;

            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"select isnull((select sum(TwZ_Ilosc) from cdn.TwrZasoby with(nolock) where TwZ_MagNumer=MAG_GIDNumer and TwZ_TwrNumer=Twr_GIDNumer),0.0000)-
                                                        isnull((select sum(Rez_Ilosc-Rez_Zrealizowano) from cdn.Rezerwacje with(nolock)
		                                                        where Rez_GIDTyp=2576 and Rez_TwrNumer=Twr_GIDNumer and Rez_MagNumer=MAG_GIDNumer
		                                                        and Rez_Aktywna=1 and Rez_DataAktywacji<=@Data and Rez_DataWaznosci>=@Data),0.0000) as [Stan]
                                                        from cdn.TwrKarty with(nolock)
                                                        inner join cdn.Magazyny with(nolock) on MAG_Kod=@Magazyn
                                                        where Twr_GIDNumer=@TwrNumer", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@Magazyn", SqlDbType.VarChar, 10).Value = magKod;
                    sqlDA.SelectCommand.Parameters.Add("@TwrNumer", SqlDbType.Int).Value = twrNumer;
                    sqlDA.SelectCommand.Parameters.Add("@Data", SqlDbType.Int).Value = DateTime.Now.Subtract(new DateTime(1800, 12, 28)).Days;
                    sqlDA.Fill(dt);
                }
            }

            if (dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                var stock = Convert.ToDecimal(r["Stan"]);
                if (stock < 0)
                    stock = 0;

                if (stock >= ilosc)
                    result = true;
            }

            return result;
        }

        internal static int GetCompanyID(int xlid)
        {
            var result = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select CompanyID from dbo.INT_XLDatabases with(nolock) where ID=@XLID", sqlConn);
                sqlCmd.Parameters.AddWithValue("@XLID", xlid);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToInt32(sqlDR[0]);
                }

                sqlConn.Close();
            }

            return result;
        }

        internal static string GetXLConnStr(int xlid)
        {
            var result = "";

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select dbo.func_INT_CLR_GetXLDBString(@XLID)", sqlConn);
                sqlCmd.Parameters.AddWithValue("@XLID", xlid);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = sqlDR[0].ToString();
                }

                sqlConn.Close();
            }

            return result;
        }

        internal static CommonArticle GetCommonArticleForTwr(int twrNumer)
        {
            CommonArticle ca = null;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select ca.ID, ca.XLID, ca.TwrNumer, ca.TwrCode, ca.SKU, isnull(xl.CDNPrefix,'') as Prefix, xl.CompanyID, ca.MinStock, isnull(c.Name,'') as CompanyName, isnull(ca.DisableSelling,0) as DisableSelling
                                            from dbo.INT_CommonArticlesMapping cam with(nolock)
                                            inner join dbo.INT_CommonArticles ca with(nolock) on ca.ID=cam.CommonArticleID
                                            inner join dbo.INT_XLDatabases xl with(nolock) on xl.ID=ca.XLID
                                            inner join dbo.INT_Companies c with(nolock) on c.ID=ca.CompanyID
                                            where cam.XLID=@XLID and cam.TwrNumer=@TwrNumer", sqlConn);
                sqlCmd.Parameters.Add("@XLID", SqlDbType.Int).Value = Properties.Settings.Default.INTXLID;
                sqlCmd.Parameters.Add("@TwrNumer", SqlDbType.Int).Value = twrNumer;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                    {
                        ca = new CommonArticle
                        {
                            ID = Convert.ToInt32(sqlDR["ID"]),
                            XLID = Convert.ToInt32(sqlDR["XLID"]),
                            TwrNumer = Convert.ToInt32(sqlDR["TwrNumer"]),
                            TwrCode = sqlDR["TwrCode"].ToString(),
                            SKU = sqlDR["SKU"].ToString(),
                            Prefix = sqlDR["Prefix"].ToString(),
                            CompanyID = Convert.ToInt32(sqlDR["CompanyID"]),
                            MinStock = Convert.ToDecimal(sqlDR["MinStock"]),
                            CompanyName = sqlDR["CompanyName"].ToString(),
                            DisableSelling = Convert.ToBoolean(sqlDR["DisableSelling"])
                        };
                    }
                }

                sqlConn.Close();
            }

            return ca;
        }

        internal static decimal GetTwrStock(int zanTyp, int zanNumer, int zanLp, string magazyn)
        {
            var result = 0.0m;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"declare @TwrNumer int, @MagNumer int
                        select @TwrNumer=ZaE_TwrNumer from cdn.ZamElem with(nolock) where ZaE_GIDTyp=@GIDTyp and ZaE_GIDNumer=@GIDNumer and ZaE_GIDLp=@GIDLp
                        select @MagNumer=MAG_GIDNumer from cdn.Magazyny with(nolock) where MAG_Kod=@Magazyn
                        select isnull((select sum(TwZ_IlSpr) from cdn.TwrZasoby with(nolock) where TwZ_TwrNumer=@TwrNumer and TwZ_MagNumer=@MagNumer),0)
		                        -isnull((select sum(Rez_Ilosc) from cdn.Rezerwacje with(nolock)
                                                        where Rez_TwrNumer=@TwrNumer and Rez_MagNumer=@MagNumer and Rez_ZrdNumer<>@GIDNumer
								                        and Rez_GIDTyp=2576 and Rez_Aktywna=1 and Rez_DataAktywacji<=datediff(day,'18001228',getdate())
                                                        and Rez_DataWaznosci>=datediff(day,'18001228',getdate())),0)", sqlConn);
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.Int).Value = zanTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = zanNumer;
                sqlCmd.Parameters.Add("@GIDLp", SqlDbType.Int).Value = zanLp;
                sqlCmd.Parameters.Add("@Magazyn", SqlDbType.VarChar, 10).Value = magazyn;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToDecimal(sqlDR[0]);
                }

                sqlConn.Close();
            }

            if (result < 0)
                result = 0;

            return result;
        }

        internal static decimal GetCommonArticleStock(CommonArticle ca, string magazyn)
        {
            var result = 0.0m;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select dbo.func_INT_CLR_GetCommonArticleStock(@XLID,@TwrNumer,@Magazyn)", sqlConn);
                sqlCmd.Parameters.Add("@TwrNumer", SqlDbType.Int).Value = ca.TwrNumer;
                sqlCmd.Parameters.Add("@XLID", SqlDbType.Int).Value = ca.XLID;
                sqlCmd.Parameters.Add("@Magazyn", SqlDbType.VarChar, 10).Value = magazyn;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToDecimal(sqlDR[0]);
                }

                sqlConn.Close();
            }

            result -= ca.MinStock;
            if (result < 0.0m)
                result = 0.0m;

            return result;
        }

        internal static void CreateCommonArticleDoc(List<CommonArticleDoc> cadList)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    foreach (var cad in cadList)
                    {
                        var sqlCmd = new SqlCommand(@"insert into dbo.INT_CommonArticlesDocHead (SourceCompanyID,DestCompanyID,SourceXLID,DestXLID,RelatedID,GIDTyp,
                                                                                            SourceGIDTyp,SourceGIDNumer,SourceDocNumber,CreateDT,Status,Property1,Property2,Export)
                                                    values(@SourceCompanyID,@DestCompanyID,@SourceXLID,@DestXLID,@RelatedID,@GIDTyp,
                                                            @SourceGIDTyp,@SourceGIDNumer,@SourceDocNumber,getdate(),0,@Property1,@Property2,@Export)
                                                    select scope_identity()", sqlConn, sqlTran);
                        sqlCmd.Parameters.Add("@SourceCompanyID", SqlDbType.Int).Value = cad.Head.SourceCompanyID;
                        sqlCmd.Parameters.Add("@DestCompanyID", SqlDbType.Int).Value = cad.Head.DestCompanyID;
                        sqlCmd.Parameters.Add("@SourceXLID", SqlDbType.Int).Value = cad.Head.SourceXLID;
                        sqlCmd.Parameters.Add("@DestXLID", SqlDbType.Int).Value = cad.Head.DestXLID;
                        sqlCmd.Parameters.Add("@RelatedID", SqlDbType.Int).Value = cad.Head.RelatedID;
                        sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = cad.Head.GIDTyp;
                        sqlCmd.Parameters.Add("@SourceGIDTyp", SqlDbType.SmallInt).Value = cad.Head.SourceGIDTyp;
                        sqlCmd.Parameters.Add("@SourceGIDNumer", SqlDbType.Int).Value = cad.Head.SourceGIDNumer;
                        sqlCmd.Parameters.Add("@SourceDocNumber", SqlDbType.VarChar, 50).Value = cad.Head.SourceDocNumber;
                        sqlCmd.Parameters.Add("@Property1", SqlDbType.VarChar, 256).Value = cad.Head.Property1;
                        sqlCmd.Parameters.Add("@Property2", SqlDbType.VarChar, 256).Value = cad.Head.Property2;
                        sqlCmd.Parameters.Add("@Export", SqlDbType.Bit).Value = cad.Head.Export;
                        var id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                        sqlCmd = new SqlCommand(@"insert into dbo.INT_CommonArticlesDocPos (HeadID,Lp,CommonTwrNumer,CommonTwrCode,MappedTwrNumer,MappedTwrCode,Quantity,Price,Amount)
                                            values(@HeadID,@Lp,@CommonTwrNumer,@CommonTwrCode,@MappedTwrNumer,@MappedTwrCode,@Quantity,@Price,@Amount)", sqlConn, sqlTran);
                        sqlCmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = id;
                        sqlCmd.Parameters.Add("@Lp", SqlDbType.SmallInt);
                        sqlCmd.Parameters.Add("@CommonTwrNumer", SqlDbType.Int);
                        sqlCmd.Parameters.Add("@CommonTwrCode", SqlDbType.VarChar, 40);
                        sqlCmd.Parameters.Add("@MappedTwrNumer", SqlDbType.Int);
                        sqlCmd.Parameters.Add("@MappedTwrCode", SqlDbType.VarChar, 40);
                        sqlCmd.Parameters.Add("@Quantity", SqlDbType.Decimal);
                        sqlCmd.Parameters.Add("@Price", SqlDbType.Decimal);
                        sqlCmd.Parameters.Add("@Amount", SqlDbType.Decimal);
                        var lp = 0;
                        foreach (var p in cad.Pos)
                        {
                            lp++;
                            p.Lp = lp;
                            sqlCmd.Parameters["@Lp"].Value = lp;
                            sqlCmd.Parameters["@CommonTwrNumer"].Value = p.CommonTwrNumer;
                            sqlCmd.Parameters["@CommonTwrCode"].Value = p.CommonTwrCode;
                            sqlCmd.Parameters["@MappedTwrNumer"].Value = p.MappedTwrNumer;
                            sqlCmd.Parameters["@MappedTwrCode"].Value = p.MappedTwrCode;
                            sqlCmd.Parameters["@Quantity"].Value = p.Quantity;
                            sqlCmd.Parameters["@Price"].Value = p.Price;
                            sqlCmd.Parameters["@Amount"].Value = p.Amount;
                            sqlCmd.ExecuteNonQuery();
                        }

                        cad.Head.ID = id;
                    }

                    sqlTran.Commit();
                }

                sqlConn.Close();
            }
        }

        internal static void WyslijDoWMS(object companyId, int gIDType, int gIDFirm, int gIDNumber, object userAPI)
        {
            throw new NotImplementedException();
        }

        internal static void SaveCommonArticleDocsForZam(int id, List<int> docs)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    var sqlCmd = new SqlCommand(@"insert into dbo.GWZ_TowaryWspolneDokumenty (GWZID,INTID,Status) values(@GWZID,@INTID,0)", sqlConn, sqlTran);
                    sqlCmd.Parameters.Add("@GWZID", SqlDbType.Int).Value = id;
                    sqlCmd.Parameters.Add("@INTID", SqlDbType.Int);
                    foreach (var d in docs)
                    {
                        sqlCmd.Parameters["@INTID"].Value = d;
                        sqlCmd.ExecuteNonQuery();
                    }

                    sqlTran.Commit();
                }

                sqlConn.Close();
            }
        }

        internal static void CreateIntMessages(int gwzId, List<CommonArticleDoc> cadList, string zamNumer)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    var sqlCmd1 = new SqlCommand(@"select top 1 WMSID from dbo.INT_Mapping with(nolock) where CompanyID=@CompanyID and XLID=@XLID", sqlConn, sqlTran);
                    sqlCmd1.Parameters.Add("@CompanyID", SqlDbType.Int);
                    sqlCmd1.Parameters.Add("@XLID", SqlDbType.Int);

                    var sqlCmd2 = new SqlCommand(
                        @"insert into dbo.INT_Messages(CompanyID,INTID,XLID,WMSID,Type,GIDTyp,GIDNumer,GIDTyp2,GIDNumer2,MagZNumer,MagDNumer,CreateDT,TryCount,ServerName,Status,Info,Priority,Property1,Property2)
                                            values(@CompanyID,1,@XLID,@WMSID,'COMMON_ARTICLE_SALE_CONF',-1,@GIDNumer,0,@GIDNumer2,0,0,getdate(),0,@@servername,0,@Info,null,@Property1,@Property2)", sqlConn,
                        sqlTran);
                    sqlCmd2.Parameters.Add("@CompanyID", SqlDbType.Int);
                    sqlCmd2.Parameters.Add("@XLID", SqlDbType.Int);
                    sqlCmd2.Parameters.Add("@WMSID", SqlDbType.Int);
                    sqlCmd2.Parameters.Add("@GIDNumer", SqlDbType.Int);
                    sqlCmd2.Parameters.Add("@GIDNumer2", SqlDbType.Int);
                    sqlCmd2.Parameters.Add("@Info", SqlDbType.NVarChar, 500);
                    sqlCmd2.Parameters.Add("@Property1", SqlDbType.NVarChar, 256);
                    sqlCmd2.Parameters.Add("@Property2", SqlDbType.NVarChar, 256);
                    foreach (var cad in cadList)
                    {
                        var wmsid = 0;
                        sqlCmd1.Parameters["@CompanyID"].Value = cad.Head.DestCompanyID;
                        sqlCmd1.Parameters["@XLID"].Value = cad.Head.DestXLID;
                        using (var sqlDR = sqlCmd1.ExecuteReader())
                        {
                            if (sqlDR.Read())
                                wmsid = Convert.ToInt32(sqlDR[0]);
                        }

                        sqlCmd2.Parameters["@CompanyID"].Value = cad.Head.DestCompanyID;
                        sqlCmd2.Parameters["@XLID"].Value = cad.Head.DestXLID;
                        sqlCmd2.Parameters["@WMSID"].Value = wmsid;
                        sqlCmd2.Parameters["@GIDNumer"].Value = cad.Head.ID;
                        sqlCmd2.Parameters["@GIDNumer2"].Value = gwzId;
                        sqlCmd2.Parameters["@Info"].Value = zamNumer;
                        sqlCmd2.Parameters["@Property1"].Value = cad.Head.SourceCompanyID.ToString();
                        sqlCmd2.Parameters["@Property2"].Value = cad.Head.SourceXLID.ToString();
                        sqlCmd2.ExecuteNonQuery();
                    }

                    sqlTran.Commit();
                }

                sqlConn.Close();
            }
        }

        internal static int CheckCommonArticleDocsStatus(int zamId)
        {
            var result = 1;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    var sqlCmd = new SqlCommand(@"if not exists(select * from dbo.GWZ_TowaryWspolneDokumenty with(nolock) where GWZID=@ID)
                                                    select 0
                                                else if exists(select * from dbo.GWZ_TowaryWspolneDokumenty with(nolock) where GWZID=@ID and Status<>1) 
                                                    select -1 
                                                else
                                                    select 1", sqlConn, sqlTran);
                    sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = zamId;
                    result = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }

                sqlConn.Close();
            }

            return result;
        }

        internal static List<CommonArticleDoc.CommonArticleDocPos> GetCommonDocPos(int xlid, int zamNumer)
        {
            var result = new List<CommonArticleDoc.CommonArticleDocPos>();

            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"select p.MappedTwrNumer,p.MappedTwrCode,sum(p.Quantity) as Quantity from dbo.INT_CommonArticlesDocHead h with(nolock)
                                                    inner join dbo.INT_CommonArticlesDocPos p with(nolock) on p.HeadID=h.ID
                                                    where h.DestXLID=@XLID and h.GIDTyp=1521 and h.SourceGIDTyp=960 and h.SourceGIDNumer=@ZamNumer
                                                    group by p.MappedTwrNumer,p.MappedTwrCode", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@XLID", SqlDbType.Int).Value = xlid;
                    sqlDA.SelectCommand.Parameters.Add("@ZamNumer", SqlDbType.Int).Value = zamNumer;
                    sqlDA.Fill(dt);
                }
            }

            foreach (DataRow r in dt.Rows)
            {
                result.Add(new CommonArticleDoc.CommonArticleDocPos
                {
                    MappedTwrNumer = Convert.ToInt32(r["MappedTwrNumer"]),
                    MappedTwrCode = r["MappedTwrCode"].ToString(),
                    Quantity = Convert.ToDecimal(r["Quantity"])
                });
            }

            return result;
        }

        internal static bool SprawdzBrakKontroliLimitu(int opeNumer)
        {
            bool result = false;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"if exists(select 1 from cdn.opekarty with(nolock)
                                                                inner join cdn.atrybuty with(nolock) on atr_obityp=ope_gidtyp and atr_obinumer=ope_gidnumer
                                                                inner join cdn.atrybutyklasy with(nolock) on atk_id=atr_atkid
                                                                where ope_gidnumer=@OpeNumer and atk_nazwa='Brak kontroli limitu kredytowego' and atr_wartosc='Tak')
                                                            select 1
                                                        else
                                                            select 0", sqlConn);
                sqlCmd.Parameters.AddWithValue("@OpeNumer", opeNumer);
                result = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                sqlConn.Close();
            }

            return result;
        }

        //internal static List<XLAPI_Wrapper.ZamPoz> PobierzPozycjeDoPelnychOp(int zamNumer)
        //{
        //    var dt = new DataTable();
        //    using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
        //    {
        //        using (var sqlDA = new SqlDataAdapter(@"declare @opId int, @sprOpId int
        //                                                select @opId=AtK_Id from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa='Opakowanie całkowite'
        //                                                select @sprOpId=AtK_Id from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa='Sprzedaż w pełnych opakowaniach'

        //                                                select * from (
        //                                                select ZaE_GIDLp as GIDLp, ZaE_Rabat as Rabat, ZaE_Ilosc as Ilosc,
        //                                                iif(ZaE_Ilosc % (TwJ_PrzeliczL/TwJ_PrzeliczM)=0, ZaE_Ilosc, ceiling(ZaE_Ilosc/(TwJ_PrzeliczL/TwJ_PrzeliczM))*(TwJ_PrzeliczL/TwJ_PrzeliczM)) as Wymagana,
        //                                                ZaE_CenaKatalogowa as CenaP, ZaE_CenaUzgodniona as Cena, ZaE_GrupaPod as GrupaPod, ZaE_Waluta as Waluta, ZaE_StawkaPod as StawkaPod, ZaE_FlagaVat as FlagaVat,
        //                                                ZaE_WartoscPoRabacie as Wartosc
        //                                                from cdn.ZamElem with(nolock)
        //                                                inner join cdn.ZamNag with(nolock) on ZaN_GIDNumer=ZaE_GIDNumer
        //                                                inner join cdn.Atrybuty op with(nolock) on op.Atr_ObiTyp=16 and op.Atr_ObiNumer=ZaE_TwrNumer and op.Atr_ObiLp=0 and op.Atr_ObiSubLp=0 and op.Atr_AtkId=@opId
        //                                                inner join cdn.TwrJm with(nolock) on TwJ_TwrNumer=ZaE_TwrNumer and TwJ_JmZ=op.Atr_Wartosc
        //                                                where ZaE_GIDNumer=@GIDNumer
        //                                                and (exists(select * from cdn.Atrybuty spr with(nolock) where spr.Atr_ObiTyp=ZaN_KntTyp and spr.Atr_ObiNumer=ZaN_KntNumer and spr.Atr_ObiLp=0 and spr.Atr_ObiSubLp=0 and spr.Atr_AtkId=@sprOpId and spr.Atr_Wartosc='TAK')
	       //                                                 or exists(select * from cdn.Magazyny with(nolock)
		      //                                                  inner join cdn.Atrybuty spr with(nolock) on spr.Atr_ObiTyp=MAG_KntTyp and spr.Atr_ObiNumer=MAG_KntNumer and spr.Atr_ObiLp=0 and spr.Atr_ObiSubLp=0 and spr.Atr_AtkId=@sprOpId and spr.Atr_Wartosc='TAK'
		      //                                                  where MAG_GIDNumer=ZaN_MagDNumer))
        //                                                ) as P
        //                                                where P.Wymagana<>P.Ilosc", sqlConn))
        //        {
        //            sqlDA.SelectCommand.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = zamNumer;
        //            sqlDA.Fill(dt);
        //        }
        //    }

        //    if (dt.Rows.Count == 0)
        //        return null;

        //    var result = new List<XLAPI_Wrapper.ZamPoz>();
        //    foreach (DataRow r in dt.Rows)
        //    {
        //        var p = new XLAPI_Wrapper.ZamPoz
        //        {
        //            GIDLp = Convert.ToInt32(r["GIDLp"]),
        //            Rabat = Convert.ToDecimal(r["Rabat"]),
        //            Ilosc = Convert.ToDecimal(r["Wymagana"]),
        //            CenaPoczatkowa = Convert.ToDecimal(r["CenaP"]),
        //            Cena = Convert.ToDecimal(r["Cena"]),
        //            GrupaVat = r["GrupaPod"].ToString(),
        //            Waluta = r["Waluta"].ToString(),
        //            StawkaVat = Convert.ToDecimal(r["StawkaPod"]),
        //            Wartosc = Convert.ToDecimal(r["Wartosc"])
        //        };
        //        switch (Convert.ToInt32(r["FlagaVat"]))
        //        {
        //            case 0:
        //                p.FlagaVat = XLAPI_Wrapper.API.FlagaVat.Zwolniony;
        //                break;
        //            case 1:
        //                p.FlagaVat = XLAPI_Wrapper.API.FlagaVat.Podatek;
        //                break;
        //            case 2:
        //                p.FlagaVat = XLAPI_Wrapper.API.FlagaVat.NiePodlega;
        //                break;
        //            default:
        //                p.FlagaVat = XLAPI_Wrapper.API.FlagaVat.Podatek;
        //                break;
        //        }

        //        result.Add(p);
        //    }

        //    return result;
        //}

        internal static void KopiujDodatkoweRozliczenia(int zamTyp, int zamNumer, int dokTyp, int dokNumer)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"insert into dbo.INT_DodatkoweRozliczenia (TrnGIDTyp,TrnGIDNumer,DokNumer,DokGIDTyp,DokGIDNumer,Kwota)
                                            select @DokTyp,@DokNumer,DokNumer,DokGIDTyp,DokGIDNumer,Kwota
                                            from dbo.INT_DodatkoweRozliczenia with(nolock)
                                            where TrnGIDTyp=@ZamTyp and TrnGIDNumer=@ZamNumer", sqlConn);
                sqlCmd.Parameters.Add("@ZamTyp", SqlDbType.SmallInt).Value = (short)zamTyp;
                sqlCmd.Parameters.Add("@ZamNumer", SqlDbType.Int).Value = zamNumer;
                sqlCmd.Parameters.Add("@DokTyp", SqlDbType.SmallInt).Value = (short)dokTyp;
                sqlCmd.Parameters.Add("@DokNumer", SqlDbType.Int).Value = dokNumer;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static bool SprawdzCzyECKemon(int zamNumer)
        {
            bool result = false;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"if not exists(select * from 
				                                                (select ZaE_GIDLp,
					                                                case when exists(select * from cdn.TwrLinki with(nolock) 
										                                                inner join cdn.TwrGrupy with(nolock) on TwG_GIDTyp=TwL_GrOTyp and TwG_GIDNumer=TwL_GrONumer and TwG_GIDTyp=-16 and TwG_Kod='KEMON'
										                                                where TwL_GIDTyp=ZaE_TwrTyp and TwL_GIDNumer=ZaE_TwrNumer) 
					                                                then 1 else 0 end as Kemon 
					                                                from cdn.ZamElem with(nolock) 
					                                                where ZaE_GIDNumer=@GIDNumer) as k
				                                                where Kemon=0)
	                                            and exists(select * from cdn.ZamNag with(nolock)
				                                        inner join cdn.KntLinki with(nolock) on KnL_GIDTyp=ZaN_KntTyp and KnL_GIDNumer=ZaN_KntNumer and KnL_GrOTyp=-32
				                                        where ZaN_GIDNumer=@GIDNumer and cdn.KntGrupaPelnaNazwa(KnL_GrONumer) like 'SALON FRYZJERSKI/SALON PARTNERSKI/KEMON%')
	                                            select 1
                                            else
	                                            select 0", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", zamNumer);
                result = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                sqlConn.Close();
            }

            return result;
        }

        internal static ECSerieParams PobierzParametryDlaSeriiEC(int zamNumer)
        {
            var result = new ECSerieParams();

            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"declare @RejId int, @FrsId int, @AkwTyp smallint, @AkwFirma int, @AkwNumer int, @AkwLp smallint
                                                    select @RejId=Knt_RegionCRM, @AkwFirma=Knt_GIDFirma
                                                    from cdn.ZamNag with(nolock)
                                                    inner join cdn.KntKarty with(nolock) on Knt_GIDNumer=ZaN_KntNumer
                                                    where ZaN_GIDNumer=@GIDNumer

                                                    select @FrsId=Atr_AtrNumer
                                                    from cdn.Atrybuty with(nolock)
                                                    inner join cdn.AtrybutyKlasy with(nolock) on AtK_ID=Atr_AtkId
                                                    where Atr_ObiTyp=948 and Atr_ObiNumer=@RejId and AtK_Nazwa='Centrum regionu'

                                                    select @AkwTyp=944, @AkwNumer=KtO_PrcNumer, @AkwLp=0 from cdn.KntOpiekun with(nolock) where KtO_KntTyp=948 and KtO_KntNumer=@RejId and KtO_Glowny=1

                                                    select @FrsId as FrsId,@AkwTyp as AkwTyp,@AkwFirma as AkwFirma,@AkwNumer as AkwNumer,@AkwLp as AkwLp", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = zamNumer;
                    sqlDA.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
                throw new Exception("Nie znaleziono parametrów dla domyślnego rejonu kontrahenta!");

            var r = dt.Rows[0];
            if (r["FrsId"] == DBNull.Value)
                throw new Exception("Nie znaleziono centrum dla domyślnego rejonu kontrahenta!");
            if (r["AkwNumer"] == DBNull.Value)
                throw new Exception("Nie znaleziono akwizytora dla domyślnego rejonu kontrahenta!");

            result.FrsId = Convert.ToInt32(r["FrsId"]);
            result.AkwTyp = Convert.ToInt32(r["AkwTyp"]);
            result.AkwFirma = Convert.ToInt32(r["AkwFirma"]);
            result.AkwNumer = Convert.ToInt32(r["AkwNumer"]);
            result.AkwLp = Convert.ToInt32(r["AkwLp"]);

            return result;
        }

        internal static void AktualizujAkw(int dokTyp, int dokNumer, int akwTyp, int akwFirma, int akwNumer, int akwLp)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"update cdn.TraNag set TrN_AkwTyp=@AkwTyp, TrN_AkwFirma=@AkwFirma, TrN_AkwNumer=@AkwNumer, TrN_AkwLp=@AkwLp where TrN_GIDTyp=@DokTyp and TrN_GIDNumer=@DokNumer", sqlConn);
                sqlCmd.Parameters.Add("@AkwTyp", SqlDbType.SmallInt).Value = akwTyp;
                sqlCmd.Parameters.Add("@AkwFirma", SqlDbType.Int).Value = akwFirma;
                sqlCmd.Parameters.Add("@AkwNumer", SqlDbType.Int).Value = akwNumer;
                sqlCmd.Parameters.Add("@AkwLp", SqlDbType.SmallInt).Value = akwLp;
                sqlCmd.Parameters.Add("@DokTyp", SqlDbType.SmallInt).Value = dokTyp;
                sqlCmd.Parameters.Add("@DokNumer", SqlDbType.Int).Value = dokNumer;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static bool SprawdzCzyKlientToSalon(int zamNumer)
        {
            bool result = false;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"if exists(select * from cdn.ZamNag with(nolock)
				                                    inner join cdn.KntLinki with(nolock) on KnL_GIDTyp=ZaN_KntTyp and KnL_GIDNumer=ZaN_KntNumer and KnL_GrOTyp=-32
				                                    where ZaN_GIDNumer=@GIDNumer and cdn.KntGrupaPelnaNazwa(KnL_GrONumer) like 'SALON FRYZJERSKI%')
	                                        select 1
                                        else
	                                        select 0", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", zamNumer);
                result = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                sqlConn.Close();
            }

            return result;
        }

        internal static List<DocumentPos> PobierzGratisy(int zamNumer)
        {
            var dt = new DataTable();
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                using (var sqlDA = new SqlDataAdapter(@"create table #tmp (
	                                                        kod varchar(40),
	                                                        ilosc decimal(15,4),
	                                                        cena decimal(18,4),
	                                                        promocja int,
	                                                        CenaPoczatkowa decimal(18,4),
	                                                        CenaPoczatkowaId int
                                                        )
                                                        insert into #tmp
                                                        exec [CDN].[_polwell_GratisyDo_GeneratorWZ] @GIDNumer

                                                        select kod as Kod, Twr_GIDNumer as TwrNumer, ilosc as Ilosc, cena as Cena, promocja as Promocja, CenaPoczatkowa as CenaP, CenaPoczatkowaId as CenaSpr
                                                        from #tmp
                                                        inner join cdn.TwrKarty with(nolock) on Twr_Kod=kod

                                                        drop table #tmp", sqlConn))
                {
                    sqlDA.SelectCommand.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = zamNumer;
                    sqlDA.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
                return null;

            var result = new List<DocumentPos>();
            foreach (DataRow r in dt.Rows)
            {
                result.Add(new DocumentPos
                {
                    TwrCode = r["Kod"].ToString(),
                    TwrNumber = Convert.ToInt32(r["TwrNumer"]),
                    Quantity = Convert.ToDecimal(r["Ilosc"]),
                    Price = Convert.ToDecimal(r["Cena"]),
                    StartPrice = Convert.ToInt32(r["CenaSpr"]),
                    CenaP = Convert.ToDecimal(r["CenaP"])
                });
            }

            return result;
        }

        internal static void UpdateIncoterms(int gidTyp, int gidNumer, string incotermsSymbol, string incotermsMiejsce)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"update cdn.TraNag set TrN_IncotermsSymbol=@Symbol, TrN_IncotermsMiejsce=@Miejsce where TrN_GIDTyp=@GIDTyp and TrN_GIDNumer=@GIDNumer", sqlConn);
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = gidTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                sqlCmd.Parameters.Add("@Symbol", SqlDbType.VarChar, 4).Value = incotermsSymbol ?? "";
                sqlCmd.Parameters.Add("@Miejsce", SqlDbType.VarChar, 255).Value = incotermsMiejsce ?? "";
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static int GetTwrType(int twrNumer)
        {
            var result = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select Twr_Typ from cdn.TwrKarty with(nolock) where Twr_GIDNumer=@TwrNumer", sqlConn);
                sqlCmd.Parameters.AddWithValue("@TwrNumer", twrNumer);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToInt32(sqlDR[0]);
                }

                sqlConn.Close();
            }

            return result;
        }

        internal static int GetTwrType(string twrCode)
        {
            var result = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select Twr_Typ from cdn.TwrKarty with(nolock) where Twr_Kod=@Kod", sqlConn);
                sqlCmd.Parameters.AddWithValue("@Kod", twrCode);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToInt32(sqlDR[0]);
                }

                sqlConn.Close();
            }

            return result;
        }

        internal static int GetTwrGID(string twrCode)
        {
            var result = 0;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"select Twr_GIDNumer from cdn.TwrKarty with(nolock) where Twr_Kod=@Kod", sqlConn);
                sqlCmd.Parameters.AddWithValue("@Kod", twrCode);
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToInt32(sqlDR[0]);
                }

                sqlConn.Close();
            }

            return result;
        }

        internal static DateTime GetRealizationDateFromCalendar(DateTime date)
        {
            DateTime result = date;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.INTConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"set @Data=dateadd(day,-1,@Data)

                                            declare @Wolny bit
                                            set @Wolny=1
                                            while @Wolny=1
                                            begin
	                                            if exists(select 1 from dbo.INT_DaysOffCalendar where [Date]=@Data and IsDayOff=1)
	                                            or ((exists(select * from dbo.INT_DaysOffCalendar where DATEPART(dw,[Date])=datepart(dw,@Data) and DoRepeatWeekly=1 and IsDayOff=1 and [Date]<=@Data)
			                                            or exists(select * from dbo.INT_DaysOffCalendar where MONTH([Date])=MONTH(@Data) and DAY([Date])=DAY(@Data) and DoRepeatAnnually=1 and IsDayOff=1 and [Date]<=@Data))
		                                            and not exists(select * from dbo.INT_DaysOffCalendar where [Date]=@Data and IsDayOff=0))
		                                            set @Wolny=1
	                                            else
		                                            set @Wolny=0

	                                            if @Wolny=1
		                                            set @Data=DATEADD(day,-1,@Data)
	                                            else
		                                            break
                                            end

                                            select @Data", sqlConn);
                sqlCmd.Parameters.Add("@Data", SqlDbType.Date).Value = date.Date;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                        result = Convert.ToDateTime(sqlDR[0]);
                }
            }

            return result;
        }

        internal static void UpdateTreNagroda(int gidTyp, int gidNumer, int gidLp)
        {
            using(var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"update cdn.TraElem set TrE_Nagroda=1 where TrE_GIDTyp=@GIDTyp and TrE_GIDNumer=@GIDNumer and TrE_GIDLp=@GIDLp", sqlConn);
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = gidTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                sqlCmd.Parameters.Add("@GIDLp", SqlDbType.SmallInt).Value = gidLp;
                sqlCmd.ExecuteNonQuery();

                sqlConn.Close();
            }
        }

        internal static bool SprawdzCzyKlientToHurtownia(int zamNumer)
        {
            bool result = false;

            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"if exists(select * from cdn.ZamNag with(nolock)
				                                    inner join cdn.KntLinki with(nolock) on KnL_GIDTyp=ZaN_KntTyp and KnL_GIDNumer=ZaN_KntNumer and KnL_GrOTyp=-32
				                                    where ZaN_GIDNumer=@GIDNumer and cdn.KntGrupaPelnaNazwa(KnL_GrONumer) like 'HURTOWNIA FRYZJERSKA%')
	                                        select 1
                                        else
	                                        select 0", sqlConn);
                sqlCmd.Parameters.AddWithValue("@GIDNumer", zamNumer);
                result = Convert.ToBoolean(sqlCmd.ExecuteScalar());

                sqlConn.Close();
            }

            return result;
        }

        internal static void PobierzPlatnoscZPromocji(int gidTyp, int gidNumer, out string formaNazwa, out int formaNr, out int termin)
        {
            formaNazwa = "";
            formaNr = 0;
            termin = -1;
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                var sqlCmd = new SqlCommand(@"declare @formaId int, @terminId int
                                            select @formaId=AtK_Id from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa='Promocja - forma płatności'
                                            select @terminId=AtK_Id from cdn.AtrybutyKlasy with(nolock) where AtK_Nazwa='Promocja - termin płatności'

                                            select FormaNazwa,FormaNr,case when ISNUMERIC(Termin)=1 then convert(int,Termin) else -1 end as Termin from (
	                                            select top 1 f.Atr_Wartosc as FormaNazwa, isnull(Kon_Lp,0) as FormaNr, isnull(t.Atr_Wartosc,'') as Termin
	                                            from cdn.PrmHistoria with(nolock)
	                                            inner join cdn.PrmKarty with(nolock) on PRM_Id=PrH_IDPrm
	                                            inner join cdn.Atrybuty f with(nolock) on f.Atr_ObiTyp=5104 and f.Atr_ObiNumer=PRM_Id and f.Atr_ObiLp=0 and f.Atr_ObiSubLp=0 and f.Atr_AtkId=@formaId
	                                            left join cdn.Atrybuty t with(nolock) on t.Atr_ObiTyp=5104 and t.Atr_ObiNumer=PRM_Id and t.Atr_ObiLp=0 and t.Atr_ObiSubLp=0 and t.Atr_AtkId=@terminId
	                                            left join cdn.Konfig with(nolock) on Kon_Numer=736 and ltrim(rtrim(SUBSTRING(Kon_Wartosc,1,20)))=f.Atr_Wartosc
	                                            where PrH_GIDTyp=@GIDTyp and PrH_GIDNumer=@GIDNumer
	                                            union all
	                                            select top 1 f.Atr_Wartosc as FormaNazwa, isnull(Kon_Lp,0) as FormaNr, isnull(t.Atr_Wartosc,'') as Termin
	                                            from cdn.TraElem with(nolock)
	                                            inner join cdn.PrmKarty with(nolock) on PRM_Id=TrE_PakietId
	                                            inner join cdn.Atrybuty f with(nolock) on f.Atr_ObiTyp=5104 and f.Atr_ObiNumer=PRM_Id and f.Atr_ObiLp=0 and f.Atr_ObiSubLp=0 and f.Atr_AtkId=@formaId
	                                            left join cdn.Atrybuty t with(nolock) on t.Atr_ObiTyp=5104 and t.Atr_ObiNumer=PRM_Id and t.Atr_ObiLp=0 and t.Atr_ObiSubLp=0 and t.Atr_AtkId=@terminId
	                                            left join cdn.Konfig with(nolock) on Kon_Numer=736 and ltrim(rtrim(SUBSTRING(Kon_Wartosc,1,20)))=f.Atr_Wartosc
	                                            where TrE_GIDTyp=@GIDTyp and TrE_GIDNumer=@GIDNumer
                                            )
                                            as dane", sqlConn);
                sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = gidTyp;
                sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                using (var sqlDR = sqlCmd.ExecuteReader())
                {
                    if (sqlDR.Read())
                    {
                        formaNazwa = sqlDR["FormaNazwa"].ToString();
                        formaNr = Convert.ToInt32(sqlDR["FormaNr"]);
                        termin = Convert.ToInt32(sqlDR["Termin"]);
                    }
                }

                sqlConn.Close();
            }
        }

        internal static void AktualizujPlatnosci(int gidTyp, int gidNumer, string formaNazwa, int formaNr, int termin)
        {
            using (var sqlConn = new SqlConnection(Properties.Settings.Default.ConnStr))
            {
                sqlConn.Open();

                using (var sqlTran = sqlConn.BeginTransaction())
                {
                    var sqlCmd = new SqlCommand(@"update cdn.TraNag set TrN_FormaNr=@FormaNr, TrN_FormaNazwa=@FormaNazwa, TrN_Termin=TrN_Data2+@Termin where TrN_GIDTyp=@GIDTyp and TrN_GIDNumer=@GIDNumer

                                                update trp
                                                set trp.TrP_FormaNr=@FormaNr, trp.TrP_FormaNazwa=@FormaNazwa, trp.TrP_Termin=trn.TrN_Data2+@Termin
                                                from cdn.TraNag trn
                                                inner join cdn.TraPlat trp on trp.TrP_GIDTyp=trn.TrN_GIDTyp and trp.TrP_GIDNumer=trn.TrN_GIDNumer
                                                where trn.TrN_GIDTyp=@GIDTyp and trn.TrN_GIDNumer=@GIDNumer", sqlConn, sqlTran);
                    sqlCmd.Parameters.Add("@GIDTyp", SqlDbType.SmallInt).Value = gidTyp;
                    sqlCmd.Parameters.Add("@GIDNumer", SqlDbType.Int).Value = gidNumer;
                    sqlCmd.Parameters.Add("@FormaNazwa", SqlDbType.VarChar, 15).Value = formaNazwa;
                    sqlCmd.Parameters.Add("@FormaNr", SqlDbType.TinyInt).Value = formaNr;
                    sqlCmd.Parameters.Add("@Termin", SqlDbType.Int).Value = termin;
                    sqlCmd.ExecuteNonQuery();

                    sqlTran.Commit();
                }

                sqlConn.Close();
            }
        }
    }
}
