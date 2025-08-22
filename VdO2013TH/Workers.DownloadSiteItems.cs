using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using MPLogHelper;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013THCore;
using VdO2013RS;
using VdO2013QRCode;
using VdO2013Data;

namespace VdO2013TH
{
    public partial class Workers
    {
        #region DownloadSiteItems
        public static void SiteReaderDownloadCallBack(BackgroundWorker worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
        {
            if (worker != null && info != null && info.LastItem != null)
            {
                var li = info.LastItem;
                worker.ReportProgress((int)li.Percentage, info);

                if (li.Status != ProgressItemStatus.Accomplished)
                {
                    String format = "Step:{0} {1}%\tStatus:{2}\tDescription:{3}";
                    switch (li.Kind)
                    {
                        case ProgressItemKind.None:
                            break;
                        case ProgressItemKind.Information:
                            MPLogHelper.FileLog.WriteInfo(format, li.Step.PadRight(25), (int)li.Percentage, li.Status, li.Description);
                            break;
                        case ProgressItemKind.Warning:
                            MPLogHelper.FileLog.WriteWarn(format, li.Step.PadRight(25), (int)li.Percentage, li.Status, li.Description);
                            break;
                        case ProgressItemKind.Error:
                            MPLogHelper.FileLog.WriteError(format, li.Step.PadRight(25), (int)li.Percentage, li.Status, li.Description);
                            break;
                        default:
                            break;
                    }

                    li.Status = ProgressItemStatus.Accomplished;
                }//if (li.Status != ProgressItemStatus.Accomplished)
            }
        }

        private static void DownloadSiteItems(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Argument as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

            info.AddInfo(0, TextRes.readerDownloadInfo, TextRes.readerDownloadStep);
            worker.ReportProgress((int)info.Percentage, info);
            String webPart = String.Empty;
            try
            {
                info.AddInfo(TextRes.readerDownloadInfoWebSite);
                worker.ReportProgress((int)info.Percentage, info);

                info.Reader.ReadList(worker, info, info.ReadListCallBack);

                info.AddInfo(info.LastItem.Percentage + 0.1, VdO2013RS.TextRes.readerDownloadInfoWebSiteDone);
                worker.ReportProgress((int)info.Percentage, info);

                var ee = info.Errors;
                if (ee != null && ee.Count() > 0)
                {
                    foreach (var le in ee)
                    {
                        if (le != null && le.Status != ProgressItemStatus.Accomplished)
                        {
                            le.Status = ProgressItemStatus.Accomplished;
                            worker.ReportProgress((int)info.Percentage, info);
                        }
                    }//foreach (var le in ee)
                }

                info.AddInfo(info.LastItem.Percentage, TextRes.readerDownloadInfoWebSiteSave);
                worker.ReportProgress((int)info.Percentage, info);

                info.Reader.SaveList(worker, info, info.SaveListCallBack);

                info.AddInfo(info.LastItem.Percentage, TextRes.readerDownloadInfoWebSiteSaveDone);
                worker.ReportProgress((int)info.Percentage, info);

                ee = info.Errors;
                if (ee != null && ee.Count() > 0)
                {
                    foreach (var le in ee)
                    {
                        if (le != null && le.Status != ProgressItemStatus.Accomplished)
                        {
                            le.Status = ProgressItemStatus.Accomplished;
                            worker.ReportProgress((int)info.Percentage, info);
                        }
                    }//foreach (var le in ee)
                }
            }
            catch (Exception ex)
            {
                info.AddError(ex);
                worker.ReportProgress((int)info.Percentage, info);
            }
            finally
            {
                e.Result = info;
            }
        }

        private static void DownloadSiteItemsProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.UserState as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;
            if (worker.Consumer != null)
                worker.Consumer.Notify(ProgressNotifyKind.Progress, info, info.Items.IndexOf(info.LastItem));
        }

        private static void DownloadSiteItemsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Result as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

            if (worker.Consumer != null)
            {
                worker.Consumer.Notify(ProgressNotifyKind.Completed, info, -1);
                worker.Reset();
            }
            if (MPLogHelper.FileLog.ConsoleCaptured)
            {
                String[] console = null;
                MPLogHelper.FileLog.ConsoleRelease(out console);
            }
        }

        //#region FillDB
        //[Obsolete("Va riscritta per il nuovo db...")]
        //private static void DownloadSiteItemsFillDB(object sender, DoWorkEventArgs e)
        //{
        //    var worker = sender as BackgroundWorkerEx;
        //    var info = e.Argument as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;
        //    info.AddInfo(TextRes.readerFillDBStep, TextRes.readerFillDBInfo);
        //    worker.ReportProgress((int)info.Percentage, info);
        //    String webPart = String.Empty;
        //    Exception error = null;
        //    try
        //    {
        //        String sqlTable = "Immagini";
        //        String sql = null;
        //        int? sqlResult = null;
        //        sql = String.Format("DELETE FROM [{0}] WHERE [CDAgenzia]={1}", sqlTable, info.Reader.Agenzia.Quote());
        //        info.AddInfo(String.Format(TextRes.readerFillDBDeletingFormat, sqlTable), TextRes.readerFillDBInfoImages);
        //        info.DataInfo.ExecuteNonQuery(sql, out error);
        //        info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult));
        //        worker.ReportProgress((int)info.Percentage, info);
        //        if (error != null) throw error;

        //        sqlTable = "Allegato";
        //        sql = String.Format("DELETE FROM [{0}] WHERE [IDOfferta] IN (SELECT [ID] FROM [Offerte] WHERE [Agenzia]={1})", sqlTable, info.Reader.Agenzia.Quote());
        //        info.AddInfo(String.Format(TextRes.readerFillDBDeletingFormat, sqlTable), TextRes.readerFillDBInfoAllegati);
        //        info.DataInfo.ExecuteNonQuery(sql, out error);
        //        info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult));
        //        worker.ReportProgress((int)info.Percentage, info);
        //        if (error != null) throw error;

        //        sqlTable = "OfferteCorrelate";
        //        sql = String.Format("DELETE FROM [{0}] WHERE CDAgenzia={1} OR CDAgenziaCorr={2}", sqlTable, info.Reader.Agenzia.Quote(), info.Reader.Agenzia.Quote());
        //        info.AddInfo(String.Format(TextRes.readerFillDBDeletingFormat, sqlTable), TextRes.readerFillDBInfoOfferteCorrelate);
        //        info.DataInfo.ExecuteNonQuery(sql, out error);
        //        info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult));
        //        worker.ReportProgress((int)info.Percentage, info);
        //        if (error != null) throw error;

        //        sqlTable = "Offerte";
        //        sql = String.Format("DELETE FROM [{0}] WHERE [Agenzia]={1}", sqlTable, info.Reader.Agenzia.Quote());
        //        info.AddInfo(String.Format(TextRes.readerFillDBDeletingFormat, sqlTable), TextRes.readerFillDBInfoAgenzie);
        //        info.DataInfo.ExecuteNonQuery(sql, out error);
        //        info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult));
        //        worker.ReportProgress((int)info.Percentage, info);
        //        if (error != null) throw error;

        //        // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----
        //        info.AddInfo(info.LastItem.Percentage, String.Empty, TextRes.readerFillDBInsertStep);
        //        worker.ReportProgress((int)info.Percentage, info);
        //        // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

        //        sqlTable = "Agenzie";
        //        sqlResult = (int)info.DataInfo.ExecuteScalar(String.Format("SELECT COUNT(*) FROM [{0}] WHERE [Agenzia]={1}", sqlTable, info.Reader.Agenzia.Quote()), out error);
        //        if (sqlResult == 0)
        //        {
        //            sql = String.Format("INSERT INTO [{0}]([Agenzia], [RagSoc]) VALUES({1},{2})", sqlTable, info.Reader.Agenzia.Quote(), info.Reader.Agenzia.Quote());
        //            info.AddInfo(info.LastItem.Percentage + 0.1, String.Format(TextRes.readerFillDBInsertingFormat, sqlTable));
        //            info.DataInfo.ExecuteNonQuery(sql, out error);
        //            info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult), info.LastItem.Step);
        //            worker.ReportProgress((int)info.Percentage, info);
        //        }
        //        if (error != null) throw error;

        //        sqlTable = new FeatureMappingData().Name;
        //        sql = String.Format("SELECT [ID], [Gruppo], [Feature], [TipoMapping], [Mapping], [Info] FROM [{0}] WHERE [TipoMapping]<>'{1}'", sqlTable, "{none}");
        //        DataTable tMappings = info.DataInfo.ExecuteTable(sql, out error);
        //        // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----
        //        var logData = new DownloadData(info.Reader);
        //        sqlTable = logData.Name;
        //        // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----
        //        sql = String.Format("SELECT * FROM [{0}] WHERE [Job]={1} AND [Description]='Offerta' ORDER BY [Value]", sqlTable, info.JobName.Quote());
        //        DataTable tOfferte = info.DataInfo.ExecuteTable(sql, out error);
        //        foreach (DataRow rOfferta in tOfferte.Rows)
        //        {
        //            String fID = rOfferta["ID"].ToString();
        //            String fJob = rOfferta["Job"].ToString();
        //            DateTime fTimeStamp = DateTime.Parse(rOfferta["TimeStamp"].ToString());
        //            String fDescription = rOfferta["Description"].ToString();
        //            String fValue = rOfferta["Value"].ToString();
        //            String fCodice = String.Empty;
        //            String fLink = String.Empty;

        //            sqlTable = logData.Name;
        //            String fSqlDettaglioLike = String.Format("Offerta{0}.", fValue);

        //            sql = String.Format("SELECT A.[ID], A.[Job], A.[TimeStamp], Replace(A.[Description], '{2}', '' ) AS [Description], A.[Value] "
        //                + "FROM [{0}] AS A "
        //                + "WHERE A.[Job]={1} AND A.[Description] LIKE '{2}%' AND LEN(A.[Value]) > 0 ORDER BY A.[ID]", sqlTable, info.JobName.Quote(), fSqlDettaglioLike);
        //            Dictionary<String, String> fDettagli = new Dictionary<String, String>();
        //            Dictionary<String, System.IO.FileInfo> fImmagini = new Dictionary<String, System.IO.FileInfo>();

        //            DataTable tDettagli = info.DataInfo.ExecuteTable(sql, out error);
        //            foreach (DataRow rDettaglio in tDettagli.Rows)
        //            {
        //                if (rDettaglio["Description"].ToString().ToLower() == "codice")
        //                {
        //                    fCodice = rDettaglio["Value"].ToString();
        //                }
        //                else
        //                    if (rDettaglio["Description"].ToString().ToLower() == "link")
        //                    {
        //                        fLink = rDettaglio["Value"].ToString();
        //                    }
        //                    else
        //                        if (rDettaglio["Description"].ToString().ToLower() == "id")
        //                        {
        //                            //fLink = rDettaglio["Value"].ToString();
        //                        }
        //                        else
        //                        {
        //                            String fFeatureKey = rDettaglio["Description"].ToString().Replace("Dettaglio[", "").Replace("]", "");
        //                            String fFeatureValue = rDettaglio["Value"].ToString();

        //                            DataView vMappings = new DataView(tMappings, String.Format("[Feature]={0}", fFeatureKey.Quote()), String.Empty, DataViewRowState.CurrentRows);
        //                            if (vMappings.Count > 0)
        //                            {
        //                                String fTipoMapping = vMappings[0]["TipoMapping"].ToString().ToLower();
        //                                String fMapping = vMappings[0]["Mapping"].ToString();
        //                                switch (fTipoMapping)
        //                                {
        //                                    case "{column}":
        //                                        fFeatureKey = fMapping;
        //                                        //fFeatureValue = qs(fFeatureValue);
        //                                        break;
        //                                    case "{formula}":
        //                                        fFeatureKey = fMapping.Left(fMapping.IndexOf('='));
        //                                        fFeatureValue = fMapping.Right(fMapping.Length - (fMapping.IndexOf('=') + 1)).Replace("{value}", fMapping.Contains("'{value}'") ? fFeatureValue.Replace("'", "''") : fFeatureValue).Replace("{agenzia}", info.Reader.Agenzia);
        //                                        if (fFeatureValue.StartsWith("SELECT"))
        //                                        {
        //                                            Object oScalar = info.DataInfo.ExecuteScalar(fFeatureValue, out error);
        //                                            if (oScalar == null || DBNull.Value.Equals(oScalar))
        //                                            {
        //                                                fFeatureValue = "NULL";
        //                                            }
        //                                            else
        //                                                if (oScalar is String)
        //                                                {
        //                                                    fFeatureValue = ((String)oScalar).Quote();
        //                                                }
        //                                                else
        //                                                {
        //                                                    fFeatureValue = oScalar.ToString();
        //                                                }

        //                                        }
        //                                        break;
        //                                    case "{image}":
        //                                        fFeatureKey = String.Empty;
        //                                        fImmagini.Add(System.IO.Path.GetFileName(fFeatureValue), new System.IO.FileInfo(fFeatureValue));
        //                                        break;
        //                                    case "{none}":
        //                                        fFeatureKey = String.Empty;
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }
        //                            }
        //                            else
        //                                fFeatureKey = String.Empty;

        //                            if (!String.IsNullOrEmpty(fFeatureKey)) fDettagli.Add(fFeatureKey, fFeatureValue);
        //                        }
        //            }//foreach (DataRow rDettaglio in tDettagli.Rows)

        //            String fFeatureFields = ("[CDArticolo]\n\t, [Agenzia]\n\t, " + String.Join("\n\t, ", fDettagli.Keys.ToArray<String>())).Replace("[Offerte].", "");
        //            String fFeatureValues = String.Format("{0}\n\t, {1}\n\t, ", fCodice.Quote(), info.Reader.Agenzia.Quote()) + String.Join("\n\t, ", fDettagli.Values.ToArray<String>());

        //            if (fDettagli.Keys.ToList<String>().IndexOf("[Offerte].[DSArticolo]") < 0 && fDettagli.Keys.ToList<String>().IndexOf("[Offerte].[DSScheda]") >= 0)
        //            {
        //                fFeatureFields += "\n\t, [DSArticolo]";
        //                fFeatureValues += String.Format("\n\t, {0}", fDettagli["[Offerte].[DSScheda]"]);
        //            }

        //            sqlTable = "Offerte";
        //            sql = String.Format("INSERT INTO [{0}]({1}) SELECT {2}", sqlTable, fFeatureFields, fFeatureValues);
        //            info.AddInfo(info.LastItem.Percentage + 0.1, String.Format(TextRes.readerFillDBInsertingOffertaFormat, fCodice, fValue));
        //            info.DataInfo.ExecuteNonQuery(sql, out error);
        //            info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult), info.LastItem.Step);
        //            worker.ReportProgress((int)info.Percentage, info);
        //            if (error != null) throw error;

        //            sqlTable = "Immagini";
        //            int i = 0;
        //            foreach (KeyValuePair<String, System.IO.FileInfo> fImmagine in fImmagini)
        //            {
        //                sql = String.Format("INSERT INTO [{0}]([CDTipoImmagine], [CDAgenzia], [CDArticolo], [Ordine], [OrdineStampa], [FileName], [Didascalia])" +
        //                    "VALUES({1},{2},{3},{4},{5},{6},{7})",
        //                    sqlTable,
        //                    "offerta".Quote(),
        //                    info.Reader.Agenzia.Quote(),
        //                    fCodice.Quote(),
        //                    i,
        //                    i,
        //                    fImmagine.Value.FullName.Quote(),
        //                    fImmagine.Key.Quote());
        //                info.AddInfo(info.LastItem.Percentage + 0.1, String.Format(TextRes.readerFillDBInsertingImmagineFormat, fImmagine.Key, i));
        //                info.DataInfo.ExecuteNonQuery(sql, out error);
        //                info.AddInfo(String.Format("\t" + TextRes.readerFillDBRowsAffectedFormat, info.DataInfo.SqlResult), info.LastItem.Step);
        //                worker.ReportProgress((int)info.Percentage, info);
        //                if (error != null) throw error;

        //                i++;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        info.AddError(Ex);
        //    }
        //    finally
        //    {
        //        e.Result = info;
        //    }
        //}

        //private static void DownloadSiteItemsFillDBProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    var worker = sender as BackgroundWorkerEx;
        //    var info = e.UserState as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;
        //    if (worker.Consumer != null)
        //        worker.Consumer.Notify(ProgressNotifyKind.Progress, info, info.Items.ToList().IndexOf(info.LastItem));
        //}

        //private static void DownloadSiteItemsFillDBCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    var worker = sender as BackgroundWorkerEx;
        //    var info = e.Result as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

        //    if (worker.Consumer != null)
        //    {
        //        worker.Consumer.Notify(ProgressNotifyKind.Completed, info, -1);
        //        worker.Reset();
        //    }
        //    if (MPLogHelper.FileLog.ConsoleCaptured)
        //    {
        //        String[] console = null;
        //        MPLogHelper.FileLog.ConsoleRelease(out console);
        //    }
        //}
        //#endregion FillDB

        #region FillXML
        private static void DownloadSiteItemsFillXML(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Argument as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

            info.AddInfo(TextRes.readerFillXMLInfo, TextRes.readerFillXMLStep);
            worker.ReportProgress((int)info.Percentage, info);

            if (info.DataInfo.ReportData == null || info.DataInfo.ReportDataCount == 0)
            {
                info.AddInfo(TextRes.readerFillXMLStep, TextRes.readerFillXMLInfoNoRows);
                worker.ReportProgress((int)info.Percentage, info);
                e.Result = info;
                return;
            }
            worker.ReportProgress((int)info.Percentage, info);

            String repDescription = String.Empty;
            String repValue = String.Empty;
            try
            {
                // ATTENZIONE: le righe iniziali DEVONO essere fisse.....
                int rowIndex = 0;
                int rowError = 0;

                // Leggo la riga "Codice agenzia";
                if (rowIndex >= info.DataInfo.ReportData.Count)
                {
                    info.AddError("Impossibile trovare il codice di agenzia.");
                    rowError++;
                }
                else
                {
                    repDescription = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Description.ColumnName].ToString();
                    repValue = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Value.ColumnName].ToString();
                    info.AddInfo(String.Format("{0}|{1}", repDescription, repValue));
                }
                worker.ReportProgress((int)info.Percentage, info);

                // Leggo la riga "Ragione sociale";
                rowIndex++;
                if (rowIndex >= info.DataInfo.ReportData.Count)
                {
                    info.AddError("Impossibile trovare la ragione sociale.");
                    rowError++;
                }
                else
                {
                    repDescription = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Description.ColumnName].ToString();
                    repValue = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Value.ColumnName].ToString();
                    info.AddInfo(String.Format("{0}|{1}", repDescription, repValue));
                }
                worker.ReportProgress((int)info.Percentage, info);

                // Leggo la riga "Tipologia proposta";
                rowIndex++;
                if (rowIndex >= info.DataInfo.ReportData.Count)
                {
                    info.AddError("Impossibile trovare la tipologia delle proposte.");
                    rowError++;
                }
                else
                {
                    repDescription = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Description.ColumnName].ToString();
                    repValue = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Value.ColumnName].ToString();
                    info.AddInfo(String.Format("{0}|{1}", repDescription, repValue));
                }
                worker.ReportProgress((int)info.Percentage, info);

                // Leggo la riga "Totale elementi";
                rowIndex++;
                if (rowIndex >= info.DataInfo.ReportData.Count)
                {
                    info.AddError("Impossibile trovare il totale degli elementi.");
                    rowError++;
                }
                else
                {
                    repDescription = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Description.ColumnName].ToString();
                    repValue = info.DataInfo.ReportData[rowIndex][DownloadData.Columns.Value.ColumnName].ToString();
                    info.AddInfo(String.Format("{0}|{1}", repDescription, repValue));
                }
                worker.ReportProgress((int)info.Percentage, info);

                if (rowError > 0)
                    throw new Exception("Errore nel formato dei dati scaricati.");

                // Ottengo l'elenco delle offerte caricate
                var dvOfferte = info.DataInfo.ReportData;
                dvOfferte.RowFilter = String.Format("[{0}]={1} AND [{2}]='{3}'", DownloadData.Columns.ReaderName.ColumnName, info.Reader.Name.Quote(), DownloadData.Columns.Description, VdO2013SRCore.TextRes.tagOfferta);
                dvOfferte.Sort = String.Format("[{0}] ASC", DownloadData.Columns.Value);

                info.AddInfo(TextRes.readerFillXMLInfoLoading);
                Double itemProgressPercentage = 33.0 / (Double)dvOfferte.Count; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare

                for (int i = 0; i < dvOfferte.Count; i++)
                {
                    info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, String.Format(TextRes.readerFillXMLReadOffertaFormat, i + 1, dvOfferte.Count));
                    worker.ReportProgress((int)info.Percentage, info);

                    repDescription = dvOfferte[i][DownloadData.Columns.Description.ColumnName].ToString();
                    repValue = dvOfferte[i][DownloadData.Columns.Value.ColumnName].ToString();
                    String currentOfferta = String.Format("{0}{1}", repDescription, repValue);
                                                              
                    DataView dvOfferta = new DataView(info.DataInfo.ReportData.Table
                        , String.Format("[{0}]={1} AND [{2}] LIKE '{3}.%'", DownloadData.Columns.ReaderName.ColumnName, info.Reader.Name.Quote(), DownloadData.Columns.Description.ColumnName, currentOfferta)
                        , String.Format("[{0}] ASC", DownloadData.Columns.ID)
                        , DataViewRowState.CurrentRows);

                    var newResultRow = info.DataInfo.ResultData.AddNew();
                    int imageIndex = 0;
                    int detailIndex = 0;
                    for (int j = 0; j < dvOfferta.Count; j++)
                    {
                        repDescription = dvOfferta[j][DownloadData.Columns.Description.ColumnName].ToString();
                        repValue = dvOfferta[j][DownloadData.Columns.Value.ColumnName].ToString();

                        String repDetail = repDescription.Right(repDescription.ReverseIndexOf('.'));
                        if (repDetail.StartsWith(VdO2013SRCore.TextRes.tagDettaglioB) && repDetail.EndsWith(VdO2013SRCore.TextRes.tagDettaglioE))
                            repDetail = repDetail.Substring(VdO2013SRCore.TextRes.tagDettaglioB.Length, repDetail.Length - (VdO2013SRCore.TextRes.tagDettaglioB + VdO2013SRCore.TextRes.tagDettaglioE).Length);

                        String mappingField = String.Empty;
                        Type mappingType = null;
                        Object mappingValue = null;
                        int ki = info.Reader.Features.IndexOf(repDetail);
                        switch (repDetail)
                        {
                            case OfferteData.Columns.cCodice:
                                if (repValue.Contains('-'))
                                    repValue = repValue.Left(repValue.IndexOf('-'));
                                mappingField = OfferteData.Columns.Codice.ColumnName;
                                mappingType = typeof(String);
                                break;
                            case OfferteData.Columns.cOrdinal:
                                mappingField = OfferteData.Columns.Ordinal.ColumnName;
                                mappingType = typeof(int);
                                break;
                            case OfferteData.Columns.cLink:
                                mappingField = OfferteData.Columns.Link.ColumnName;

                                if (repValue.StartsWith(info.Reader.StandardSiteRootUrl) || repValue.StartsWith(info.Reader.MobileSiteRootUrl))
                                    repValue.Replace(info.Reader.StandardSiteRootUrl, info.Reader.MobileSiteRootUrl + @"/");
                                else
                                    repValue = info.Reader.MobileSiteRootUrl + @"/" + repValue;

                                while (repValue.Contains(@"//"))
                                    repValue = repValue.Replace(@"//", @"/");

                                if (repValue.StartsWith(@"http:/"))
                                    repValue = repValue.Replace(@"http:/", @"http://");
                                else
                                    if (repValue.StartsWith(@"https:/"))
                                        repValue = repValue.Replace(@"https:/", @"https://");
                                mappingType = typeof(String);
                                break;
                            case VdO2013SRCore.TextRes.tagImmagineB + "00" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "01" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "02" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "03" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "04" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "05" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "06" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "07" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "08" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "09" + VdO2013SRCore.TextRes.tagImmagineE:
                            case VdO2013SRCore.TextRes.tagImmagineB + "10" + VdO2013SRCore.TextRes.tagImmagineE:
                                if (imageIndex < 5)
                                {
                                    switch (imageIndex)
                                    {
                                        case 0:
                                            mappingField = OfferteData.Columns.ImageFileName1.ColumnName;
                                            break;
                                        case 1:
                                            mappingField = OfferteData.Columns.ImageFileName2.ColumnName;
                                            break;
                                        case 2:
                                            mappingField = OfferteData.Columns.ImageFileName3.ColumnName;
                                            break;
                                        case 3:
                                            mappingField = OfferteData.Columns.ImageFileName4.ColumnName;
                                            break;
                                        case 4:
                                            mappingField = OfferteData.Columns.ImageFileName5.ColumnName;
                                            break;
                                        default:
                                            break;
                                    }
                                    mappingType = typeof(String);
                                    imageIndex++;
                                }
                                break;
                            default:
                                if (ki >= 0)
                                {
                                    mappingField = info.Reader.Features[ki].Mapping;
                                    mappingType = Type.GetType(info.Reader.Features[ki].TypeName);
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(repDetail) && !String.IsNullOrEmpty(repValue))
                                        try
                                        {
                                            if (repDetail.StartsWith("{") && repDetail.EndsWith("}"))
                                                repDetail = repDetail.Substring("{".Length, repDetail.Length - ("{" + "}").Length);
                                            repDetail = repDetail.Replace('_', ' ');
                                            repDetail = repDetail.ToTitleCase();
                                            repValue = repValue.ToTitleCase();

                                            var newDetailsRow = info.DataInfo.DetailsData.AddNew();
                                            newDetailsRow[OfferteDetailsData.Columns.CodiceOfferta.ColumnName] = newResultRow[OfferteData.Columns.Codice.ColumnName];
                                            newDetailsRow[OfferteDetailsData.Columns.Ordinal.ColumnName] = detailIndex;
                                            newDetailsRow[OfferteDetailsData.Columns.Descrizione.ColumnName] = repDetail;
                                            newDetailsRow[OfferteDetailsData.Columns.Valore.ColumnName] = repValue;
                                            //info.DataInfo.DetailsData.Rows.Add(newDetailsRow);
                                            newDetailsRow.EndEdit();
                                            detailIndex++;
                                        }
                                        catch (Exception ex)
                                        {
                                            info.AddError(ex);
                                        }
                                }
                                break;
                        }

                        if (!String.IsNullOrEmpty(mappingField))
                        {
                            if (ki >= 0)
                            {
                                Exception error = null;
                                info.Reader.Features[ki].Apply(repValue, out repValue, out error);
                                if (error != null)
                                {
                                    info.AddError(error);
                                    worker.ReportProgress((int)info.Percentage, info);
                                }

                                // Attenzione: questa parte ha senso solo se la feature di riferimento è già
                                // stata valorizzata... Dipende dal config quindi!!
                                if (String.IsNullOrEmpty(repValue) && !String.IsNullOrEmpty( info.Reader.Features[ki].IfMissingUseFeature))
                                {
                                    int referTo = info.Reader.Features.IndexOf(info.Reader.Features[ki].IfMissingUseFeature);
                                    if (referTo >= 0)
                                    {
                                        repValue = info.Reader.Features[referTo].LastValue;
                                    }
                                }
                            }

                            // Valido il dato in base al tipo specificato
                            try
                            {
                                mappingValue = Convert.ChangeType(repValue, mappingType);
                            }
                            catch (Exception ex)
                            {
                                MPLogHelper.FileLog.WriteError("Error converting value '{0}' for '{1}' into '{2}': {3}", repValue, repDetail, mappingType, ex.Message);
                                mappingValue = DBNull.Value;
                            }

                            newResultRow[mappingField] = mappingValue;
                        }
                    }//for (int j = 0; j < dvOfferta.Count; j++)

                    if (!String.IsNullOrEmpty("{0}".FormatWith(newResultRow[OfferteData.Columns.Link.ColumnName])))
                    {
                        String qrCodeFile = String.Empty;
                        var qrh = info.Reader.QRCodeHelper as QRCodeHelper;
                        try
                        {
                            String link = newResultRow[OfferteData.Columns.Link.ColumnName].ToString();
                            String codice = newResultRow[OfferteData.Columns.Codice.ColumnName].ToString();

                            Exception error = null;
                            var img = QRCodeHelper.Encode(qrh, link, out error);

                            if (error != null)
                                throw new Exception(String.Format("Error creating QR image for code '{0}' with link '{1}'", codice, link), error);

                            if (img != null && !img.Size.IsEmpty)
                            {
                                qrCodeFile = Path.Combine(info.Reader.ImagesPath, info.Reader.Agenzia.Replace('/', '\\'));
                                if (!Directory.Exists(qrCodeFile)) Directory.CreateDirectory(qrCodeFile);

                                qrCodeFile = Path.Combine(qrCodeFile, String.Format("{0}{1}", codice.Replace('/', '\\'), qrh.ImageFormatFileExtension()));
                                if (!Directory.Exists(Path.GetDirectoryName(qrCodeFile))) Directory.CreateDirectory(Path.GetDirectoryName(qrCodeFile));

                                newResultRow[OfferteData.Columns.QRCodeFileName.ColumnName] = qrCodeFile;

                                if (File.Exists(qrCodeFile)) File.Delete(qrCodeFile);
                                if (!File.Exists(qrCodeFile)) img.Save(qrCodeFile, qrh.ImageFormat);
                            }
                        }
                        catch (System.Runtime.InteropServices.ExternalException ex)
                        {
                            info.AddError(new Exception(String.Format(TextRes.readerFillXMLSaveQRCodeErrorInteropFormat, qrCodeFile, qrh.ImageFormat.ToString(), ex.ErrorCode, ex.StackTrace), ex));
                            worker.ReportProgress((int)info.Percentage, info);
                        }
                        catch (IOException ex)
                        {
                            info.AddError(new Exception(String.Format(TextRes.readerFillXMLSaveQRCodeErrorInteropFormat, qrCodeFile, qrh.ImageFormat.ToString(), 0, ex.StackTrace), ex));
                            worker.ReportProgress((int)info.Percentage, info);
                        }
                        catch (Exception ex)
                        {
                            MPLogHelper.FileLog.WriteError(TextRes.readerFillXMLSaveQRCodeErrorFormat, ex.ToString());
                            worker.ReportProgress((int)info.Percentage, info);
                        }
                    }

                    //info.DataInfo.ResultData.Rows.Add(newResultRow);
                    newResultRow.EndEdit();
                    info.AddInfo(TextRes.readerFillXMLInfoCompleted);
                    worker.ReportProgress((int)info.Percentage, info);
                }
            }
            catch (Exception Ex)
            {
                info.AddError(Ex);
                worker.ReportProgress((int)info.Percentage, info);
            }
            finally
            {
                e.Result = info;
            }
        }

        private static void DownloadSiteItemsFillXMLProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.UserState as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;
            if (worker.Consumer != null)
                worker.Consumer.Notify(ProgressNotifyKind.Progress, info, info.Items.ToList().IndexOf(info.LastItem));
        }

        private static void DownloadSiteItemsFillXMLCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Result as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

            if (worker.Consumer != null)
            {
                worker.Consumer.Notify(ProgressNotifyKind.Completed, info, -1);
                worker.Reset();
            }
            if (MPLogHelper.FileLog.ConsoleCaptured)
            {
                String[] console = null;
                MPLogHelper.FileLog.ConsoleRelease(out console);
            }
        }
        #endregion FillXML

        #endregion DownloadSiteItems
    }
}
