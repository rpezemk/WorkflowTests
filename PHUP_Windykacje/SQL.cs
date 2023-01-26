using PHUP_Windykacje.Model;
using PHUP_Windykacje.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows;

namespace PHUP_Windykacje
{
    internal static class SQL
    {

#if DEBUG

        private static string userName = "";
#else
        private static string userName = Environment.UserName == ""? "" : Environment.UserName;
#endif


#if DEBUG
        public static SqlConnection SqlConn
        {
            get
            {
                var csb = new SqlConnectionStringBuilder
                {
                    DataSource = @"",
                    InitialCatalog = "",
                    UserID = "",
                    Password = "",
                    ApplicationName = ""
                };

                return new SqlConnection(csb.ConnectionString);
            }
        }
#else
        public static SqlConnection SqlConn
        {
            get
            {
                var csb = new SqlConnectionStringBuilder
                {
                    DataSource = "",
                    InitialCatalog = "",
                    UserID = "",
                    Password = "",
                    ApplicationName = ""
                };

                return new SqlConnection(csb.ConnectionString);
            }
        }
#endif
        public static List<Model.RaportRow> GetRaportRows(DateTime dataOd, DateTime dataDo, string seria, out string outQuery)
        {
            var raportRows = new List<Model.RaportRow>();
            string query = @$"      
                            WITH Doc as 
                            (
                                select  
                                    TrN_GIDTyp,
                                    TrN_GIDNumer, 
                                    Dateadd(day, TrN_Data2, '1800-12-28') as [Data], 
                                    TrN_Termin,
                                    WDR_NumerDokumentu,
                                    TrN_KntNumer
                                FROM cdn.TraNag with (nolock)
                                INNER JOIN cdn.TraPlat with(nolock)
                                    on TrN_GIDTyp=TrP_GIDTyp 
                                   AND TrN_GIDNumer=TrP_GIDNumer
                                WHERE TrN_GIDTyp = 2033
                                AND TrN_TrNSeria LIKE @seria
                                AND TrP_Rozliczona = 0
                                AND (TrN_Data2 BETWEEN DATEDIFF(day, '1800-12-28', @dataOd)  
                                                   AND DATEDIFF(day, '1800-12-28', @dataDo))
                            ),
                            Res as (
                                select
                                    Doc.TrN_GIDNumer                as [GIDNumer],
                                    knt.Knt_Akronim                 as [AkronimKontrahenta], 
                                    knt.Knt_Nazwa1                  as [NazwaKontrahenta], 
                                    doc.WDR_NumerDokumentu              as [NumerDokumentu],
                                    sum(TrE_Cena * TrE_Ilosc)       as [Wartosc], 
                                    [Data]                          as [DataWyst],
                                    DATEADD(day, TrN_Termin, '1800-12-28') 
                                                                    as [TerminPlatnosci],
                                    ph.Knt_Akronim + '/' +
                                    ISNULL(STUFF((
                                            Select ',' + opeartor.Ope_Ident
                                            FROM CDN.KntOpiekun op with(nolock)
                                            inner join cdn.OpeKarty opeartor with(nolock)
                                                    on opeartor.Ope_GIDNumer = KtO_OpeNumer
                                            where knt.Knt_GIDNumer = KtO_KntNumer
                                            FOR XML PATH('')
                                    ),1, 1, ''), '') 
                                                                    as [PH/Opiekun], 
                                    ph.Knt_EMail                    as [PhEmail], 

                                    ( Select  Opis 
                                      from dbo.FSEleNierozliczoneOpisy opis 
                                      where 
                                      opis.NumerDokumentu = WDR_NumerDokumentu )                   
                                                                    as [Opis],
                                    ( Select ISNULL(FlagaN, '') 
                                      from dbo.FSEleNierozliczoneOpisy opis 
                                      where 
                                      opis.NumerDokumentu = WDR_NumerDokumentu )                   
                                                                    as [FlagaN]
                                                                     
                                from Doc
                                INNER JOIN CDN.TraElem with (nolock)
                                        on TrE_GIDNumer = Doc.TrN_GIDNumer
                                INNER JOIN CDN.KntKarty knt with(nolock)
                                        ON Knt_GIDNumer = Doc.TrN_KntNumer
                                LEFT JOIN CDN.KntKarty ph with(nolock)
                                        ON ph.Knt_GIDNumer = knt.Knt_AkwNumer
    
                                GROUP BY Doc.TrN_GIDNumer,knt.Knt_GIDNumer, knt.Knt_Akronim, knt.Knt_Nazwa1, 
                                        Doc.WDR_NumerDokumentu, [Data], TrN_Termin,  ph.Knt_Akronim, ph.[Knt_EMail]

                            )
                            select 
                                cast((ROW_NUMBER() over (order by Res.DataWyst)) as int) as [Lp],
                                Res.AkronimKontrahenta,
                                Res.NazwaKontrahenta,
                                Res.NumerDokumentu,
                                Res.Wartosc, 
                                convert(varchar, Res.DataWyst, 23) [DataWystawienia],
                                Convert(varchar, Res.TerminPlatnosci, 23) [TerminPlatnosci],
                                IIF(Charindex('/', Res.[PH/Opiekun]) = len(Res.[PH/Opiekun]), 
                                    STUFF(Res.[PH/Opiekun], len(Res.[PH/Opiekun]), 1, ''), 
                                    Res.[PH/Opiekun]) [PH/Opiekun], 
                                Res.[PhEmail],
                                Res.Opis,
                                Res.FlagaN,
                                Res.GIDNumer
                            from Res   
                            order by Res.[DataWyst]";

            outQuery = query;

            var dt = new DataTable();
            try
            {
                using (var sqlConn = SqlConn)
                {
                    sqlConn.Open();
                    using (SqlDataAdapter sqlCommand = new SqlDataAdapter(query, sqlConn))
                    {
                        sqlCommand.SelectCommand.Parameters.AddWithValue("@dataOd", dataOd);
                        sqlCommand.SelectCommand.Parameters.AddWithValue("@dataDo", dataDo);
                        sqlCommand.SelectCommand.Parameters.AddWithValue("@seria", seria);
                        sqlCommand.Fill(dt);
                    }
                }
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.ToString() + " " + ex.Message); }

            foreach (DataRow dataRow in dt.Rows)
            {
                var raportRow = new Model.RaportRow();
                raportRow.Lp = dataRow.Field<int>(0);
                raportRow.KntAronim = dataRow.Field<string>(1) ?? "";
                raportRow.KntNazwa = dataRow.Field<string>(2) ?? "";
                raportRow.NrDok = dataRow.Field<string>(3) ?? "";
                raportRow.ValueBrut = dataRow.Field<decimal>(4);
                raportRow.DataWyst = Convert.ToDateTime(dataRow.Field<String>(5));
                raportRow.TerminPlat = Convert.ToDateTime(dataRow.Field<string>(6));
                raportRow.PhOpiekun = dataRow.Field<string>(7) ?? "";
                raportRow.PhEmail = dataRow.Field<string>(8) ?? "";
                raportRow.Opis = dataRow.Field<string>(9) ?? "";
                raportRow.PreventExport = (dataRow.Field<string>(10) ?? "").Contains("N");
                raportRows.Add(raportRow);
            }
            return raportRows;
        }



        public static async void InsertUpdateOpisyRaportu1(List<RaportRow> raportRows, Action<int, int>? updateAction = null)
        {

            var nRows = raportRows.Count();
            var counter = 0;

            var query1 = $@" IF EXISTS ( SELECT 1 
                                        from dbo.FSEleNierozliczoneOpisy 
                                        where NumerDokumentu = @docNr
                                        )
                            BEGIN  
                                update dbo.FSEleNierozliczoneOpisy 
                                set Opis = @opis, FlagaN = @flaga
                                where NumerDokumentu = @docNr
                            END
                            ELSE 
                            BEGIN
                                INSERT INTO dbo.FSEleNierozliczoneOpisy
                                VALUES (@docNr, @opis, @flaga)
                            END
                            ";

            var query2 = $@" 
                                IF not exists(                         
                                                Select *               
                                                from  cdn.KntKontakty with(nolock) 
                                                where LTRIM(RTRIM(KnK_Notatki)) = LTRIM(RTRIM(@docNr))  + ' nierozliczone: ' + LTRIM(RTRIM( @opis ))
                                )

                                insert  into  cdn.kntkontakty (
                                                 knk_knttyp ,knk_kntfirma ,knk_kntnumer ,knk_kntlp
                                                ,knk_opetyp ,knk_opefirma ,knk_openumer ,knk_opelp 
                                                ,knk_termin ,knk_priorytet ,knk_zakonczone ,knk_kontaktza ,knk_kontaktjc ,knk_okresowy ,knk_licznik 
                                                ,knk_notatki, knk_knstyp ,knk_knsfirma ,knk_knsnumer ,knk_knslp
                                         )
                                select  p.knt_gidtyp ,p.knt_gidfirma ,p.knt_gidnumer
                                       ,isnull(
                                           (select  max(knk_kntlp) from  cdn.kntkontakty with(nolock) where  knk_kntnumer = p.knt_gidnumer),0) 
                                            + row_number() over(partition by p.knt_gidnumer order by TrN_gidnumer) 
                                       ,128 ,k.knt_gidfirma ,                                                                 -- gid operatora
                                       (select Ope_GIDNumer  from CDN.OpeKarty with(nolock) where Ope_Ident = @winUser) ,0     -- gid operatora  
                                       ,datediff(day ,'18001228' ,getdate()) -- termin
                                       ,1 ,0 ,0  ,1 ,0 ,0 
                                       , Nag.WDR_NumerDokumentu + ' nierozliczone: ' + LTRIM(RTRIM( @opis ))  AS Notatki          -- Notatki
                                        ,0 ,0  ,0  ,0                                   -- osoby powiązanej z kontaktem
                                from  cdn.TraNag Nag with(nolock)
                                inner  join  cdn.kntkarty k with(nolock)
                                         on  k.knt_gidnumer = TrN_kntnumer
                                inner  join  cdn.kntkarty p with(nolock) -- platnik
                                         on  p.knt_gidtyp = k.knt_knptyp
                                        and  p.knt_gidnumer = k.knt_knpnumer
                                where  WDR_NumerDokumentu = LTRIM(RTRIM(@docNr)) ";


            try
            {
                using (var sqlconn = SqlConn)
                {
                    sqlconn.Open();
                    foreach (var raportRow in raportRows)
                    {
                        counter++;
                        if (updateAction != null)
                            updateAction.Invoke(nRows, counter);

                        if (string.IsNullOrEmpty(raportRow.Opis))
                            continue;

                        using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
                        {
                            cmd.Parameters.AddWithValue("@docNr", raportRow.NrDok);
                            cmd.Parameters.AddWithValue("@opis", raportRow.Opis);
                            cmd.Parameters.AddWithValue("@flaga", raportRow.PreventExport ? "N" : "");
                            cmd.ExecuteNonQuery();
                        }


                        if (raportRow.PreventExport)
                            continue;


                        using (SqlCommand cmd = new SqlCommand(query2, sqlconn))
                        {
                            cmd.Parameters.AddWithValue("@docNr", raportRow.NrDok);
                            cmd.Parameters.AddWithValue("@opis", raportRow.Opis);
                            cmd.Parameters.AddWithValue("@winUser", userName);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static List<ViewModel.VM_ErrorDef> GetErrorDefinitions()
        {
            var raportRows = new List<ViewModel.VM_ErrorDef>();
            string query = @$"
                            select  ed.ID, ed.Name, ed.Tip, 
                                    efc.ID [efcID], efc.ErrorID, efc.TableName, efc.ColumnName, 
                                    efc.[VALUE], efc.IsWildcard, efc.IsRegex from dbo.ErrorDefs ed
                                left Join dbo.ErrorFilterColumns efc
                                on ed.ID = efc.ErrorID
                            ";



            var dt = new DataTable();
            try
            {
                using (var sqlConn = SqlConn)
                {
                    sqlConn.Open();
                    using (SqlDataAdapter sqlCommand = new SqlDataAdapter(query, sqlConn))
                    {
                        sqlCommand.Fill(dt);
                    }
                }
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.ToString() + " " + ex.Message); }

            foreach (DataRow dataRow in dt.Rows)
            {
                var id = dataRow.Field<int>("ID");
                if (raportRows.Where(r => r.ID == id).Any())
                    continue;

                var raportRow = new ViewModel.VM_ErrorDef();
                raportRow.ID = id;
                raportRow.Name = dataRow.Field<string>("Name") ?? "";
                raportRow.Tip = dataRow.Field<string>("Tip") ?? "";
                raportRows.Add(raportRow);
            }

            foreach(VM_ErrorDef vM_ErrorDef in raportRows)
            {
                var group = dt.AsEnumerable().Where(r => r.Field<int>("ErrorID") == vM_ErrorDef.ID);
                vM_ErrorDef.Filters = new ObservableCollection<VM_ErrorFilterColumn>(group.Select(r => new VM_ErrorFilterColumn()
                { 
                    ID = r.Field<int>("efcID"), 
                    ErrorID = r.Field<int>("ErrorID"),
                    TableName = r.Field<string>("TableName"),
                    ColumnName = r.Field<string>("ColumnName"),
                    Value = r.Field<string>("Value"),
                    IsWildCard = r.Field<bool>("IsWildCard"),
                    IsRegex = r.Field<bool>("IsRegex")
                }));
            }


            return raportRows;
        }




    }



}
