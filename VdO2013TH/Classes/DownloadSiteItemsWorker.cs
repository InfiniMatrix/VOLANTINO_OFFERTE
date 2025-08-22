using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using VdO2013Core;

using MPLogHelper;

namespace VdO2013TH
{
    internal class DownloadSiteItemsWorker : BackgroundWorkerEx
    {
        public DownloadSiteItemsWorker()
            : base(BackgroundWorkerBlocking.UserInterface)
        {
            this.WorkerReportsProgress = true;
            this.DoWork += new DoWorkEventHandler(DownloadSiteItems);
            this.ProgressChanged += new ProgressChangedEventHandler(DownloadSiteItemsProgressChanged);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadSiteItemsCompleted);
        }

        private static void DownloadSiteItems(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Argument as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

            info.AddInfo(0, TextRes.Reader.Download.INFO, TextRes.Reader.Download.STEP);
            worker.ReportProgress((int)info.Percentage, info);
            String webPart = String.Empty;
            try
            {
                info.AddInfo(TextRes.Reader.Download.INFOWEBSITE);
                worker.ReportProgress((int)info.Percentage, info);

                info.Reader.ReadList(worker, info, info.ReadListCallBack);

                info.AddInfo(info.LastItem.Percentage + 0.1, VdO2013RS.TextRes.Reader.Download.INFOWEBSITEDONE);
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

                info.AddInfo(info.LastItem.Percentage, TextRes.Reader.Download.INFOWEBSITESAVE);
                worker.ReportProgress((int)info.Percentage, info);

                info.Reader.SaveList(worker, info, info.SaveListCallBack);

                info.AddInfo(info.LastItem.Percentage, TextRes.Reader.Download.INFOWEBSITESAVEDONE);
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
                MPLogHelper.FileLog.ConsoleRelease(out string[] console);
            }
        }

        #region FillDB
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
        #endregion FillDB
    }
}
