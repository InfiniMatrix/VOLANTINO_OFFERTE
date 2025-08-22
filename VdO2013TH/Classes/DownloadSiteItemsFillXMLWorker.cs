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
using VdO2013DataCore;

using MPExtensionMethods;

namespace VdO2013TH
{
    internal class DownloadSiteItemsFillXMLWorker : BackgroundWorkerEx
    {
        public DownloadSiteItemsFillXMLWorker()
            : base(BackgroundWorkerBlocking.UserInterface)
        {
            this.WorkerReportsProgress = true;
            this.DoWork += new DoWorkEventHandler(DownloadSiteItemsFillXML);
            this.ProgressChanged += new ProgressChangedEventHandler(DownloadSiteItemsFillXMLProgressChanged);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadSiteItemsFillXMLCompleted);
        }

        private static int? DownloadSiteItemsCheckHeader(BackgroundWorkerEx worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
        {
            int? result = null;
            // ATTENZIONE: le righe iniziali DEVONO essere fisse.....
            const int ROWCODICEAGENZIA = 0;
            const int ROWRAGIONESOCIALE = 1;
            const int ROWTIPOLOGIA = 2;
            const int ROWTOTALCOUNT = 3;

            string repDescription;
            string codiceAgenzia = null;
            string ragioneSociale = null;
            string tipologia = null;
            int? totale = null;

            Func<int, string> getReportJob = (index) => info.DataInfo.ReportData[index][DownloadData.Columns.Job.ColumnName].ToString();
            Func<int, string> getReportDescription = (index) => info.DataInfo.ReportData[index][DownloadData.Columns.Description.ColumnName].ToString();
            Func<int, string> getReportValue = (index) => info.DataInfo.ReportData[index][DownloadData.Columns.Value.ColumnName].ToString();
            Func<int, DateTime> getReportTimeStamp = (index) => (DateTime)info.DataInfo.ReportData[index][DownloadData.Columns.InsertDate.ColumnName];

            // Leggo la riga "Codice agenzia";
            if (ROWCODICEAGENZIA >= info.DataInfo.ReportData.Count)
            {
                info.AddError(TextRes.Reader.FillXML.ERRORCODICEAGENZIANOTFOUND);
                result = ROWCODICEAGENZIA;
            }
            else
            {
                repDescription = getReportDescription(ROWCODICEAGENZIA);
                codiceAgenzia = getReportValue(ROWCODICEAGENZIA);
                info.AddInfo(String.Format("{0}={1}", repDescription, codiceAgenzia));
            }
            worker.ReportProgress((int)info.Percentage, info);

            // Leggo la riga "Ragione sociale";
            if (ROWRAGIONESOCIALE >= info.DataInfo.ReportData.Count)
            {
                info.AddError(TextRes.Reader.FillXML.ERRORRAGIONESOCIALENOTFOUND);
                result = ROWRAGIONESOCIALE;
            }
            else
            {
                repDescription = getReportDescription(ROWRAGIONESOCIALE);
                ragioneSociale = getReportValue(ROWRAGIONESOCIALE);
                info.AddInfo(String.Format("{0}={1}", repDescription, ragioneSociale));
            }
            worker.ReportProgress((int)info.Percentage, info);

            // Leggo la riga "Tipologia proposta";
            if (ROWTIPOLOGIA >= info.DataInfo.ReportData.Count)
            {
                info.AddError(TextRes.Reader.FillXML.ERRORTIPOLOGIANOTFOUND);
                result = ROWTIPOLOGIA;
            }
            else
            {
                repDescription = getReportDescription(ROWTIPOLOGIA);
                tipologia = getReportValue(ROWTIPOLOGIA);
                info.AddInfo(String.Format("{0}={1}", repDescription, tipologia));
            }
            worker.ReportProgress((int)info.Percentage, info);

            // Leggo la riga "Totale elementi";
            if (ROWTOTALCOUNT >= info.DataInfo.ReportData.Count)
            {
                info.AddError(TextRes.Reader.FillXML.ERRORTOTALCOUNTNOTFOUND);
                result = ROWTOTALCOUNT;
            }
            else
            {
                repDescription = getReportDescription(ROWTOTALCOUNT);
                totale = int.Parse(getReportValue(ROWTOTALCOUNT));
                info.AddInfo(String.Format("{0}={1}", repDescription, totale));
            }
            worker.ReportProgress((int)info.Percentage, info);

            return result;
        }

        private static bool DownloadSiteItemsGetDetailMapping(BackgroundWorkerEx worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info
            , string detail
            , int index
            , ref string value
            , ref int imageIndex
            , out string mappingName
            , out Type mappingType)
        {
            mappingName = null;
            mappingType = null;
            switch (detail)
            {
                case OfferteData.Columns.cCodice:
                    if (value.Contains('-'))
                        value = value.Left(value.IndexOf('-'));

                    if (value.Contains(info.Reader.StandardSiteRootUrl))
                        value = value.Replace(info.Reader.StandardSiteRootUrl, string.Empty);

                    if (String.IsNullOrEmpty(value))
                        value = "VDO{0}_{1}".FormatWith(index, new Random(index));

                    mappingName = OfferteData.Columns.Codice.ColumnName;
                    mappingType = typeof(String);
                    break;
                case OfferteData.Columns.cOrdinal:
                    mappingName = OfferteData.Columns.Ordinal.ColumnName;
                    mappingType = typeof(int);
                    break;
                case OfferteData.Columns.cLink:
                    mappingName = OfferteData.Columns.Link.ColumnName;

                    if (value.StartsWith(info.Reader.StandardSiteRootUrl) || value.StartsWith(info.Reader.MobileSiteRootUrl))
                        value.Replace(info.Reader.StandardSiteRootUrl, info.Reader.MobileSiteRootUrl + @"/");
                    else
                        value = info.Reader.MobileSiteRootUrl + @"/" + value;

                    while (value.Contains(@"//"))
                        value = value.Replace(@"//", @"/");

                    if (value.StartsWith(@"http:/"))
                        value = value.Replace(@"http:/", @"http://");
                    else
                        if (value.StartsWith(@"https:/"))
                        value = value.Replace(@"https:/", @"https://");
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
                                mappingName = OfferteData.Columns.ImageFileName1.ColumnName;
                                break;
                            case 1:
                                mappingName = OfferteData.Columns.ImageFileName2.ColumnName;
                                break;
                            case 2:
                                mappingName = OfferteData.Columns.ImageFileName3.ColumnName;
                                break;
                            case 3:
                                mappingName = OfferteData.Columns.ImageFileName4.ColumnName;
                                break;
                            case 4:
                                mappingName = OfferteData.Columns.ImageFileName5.ColumnName;
                                break;
                            default:
                                break;
                        }
                        mappingType = typeof(String);
                        imageIndex++;
                    }
                    break;
                default:
                    break;
            }// switch (repDetail)

            return !string.IsNullOrEmpty(mappingName) && mappingType != null;
        }

        private static bool DownloadSiteItemsAddDetail(BackgroundWorkerEx worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info
            , string codice
            , int detailIndex
            , string repDetail
            , string repValue)
        {
            if (!String.IsNullOrEmpty(repDetail) && !String.IsNullOrEmpty(repValue))
                try
                {
                    if (repDetail.StartsWith("{") && repDetail.EndsWith("}"))
                        repDetail = repDetail.Substring("{".Length, repDetail.Length - ("{" + "}").Length);
                    repDetail = repDetail.Replace('_', ' ');
                    repDetail = repDetail.ToTitleCase();
                    repValue = repValue.ToTitleCase();

                    var newDetailsRow = info.DataInfo.DetailData.AddNew();
                    newDetailsRow[OfferteDetailsData.Columns.CodiceOfferta.ColumnName] = codice;
                    newDetailsRow[OfferteDetailsData.Columns.Ordinal.ColumnName] = detailIndex;
                    newDetailsRow[OfferteDetailsData.Columns.Descrizione.ColumnName] = repDetail;
                    newDetailsRow[OfferteDetailsData.Columns.Valore.ColumnName] = repValue;
                    newDetailsRow.EndEdit();

                    return true;
                }
                catch (Exception ex)
                {
                    info.AddError(ex);
                }
            return false;
        }
        private static bool DownloadSiteItemsFillXMLCreateQRCode(BackgroundWorkerEx worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, DataRowView row)
        {
            String qrCodeFile = String.Empty;
            var qrh = info.Reader.QRCode as QRCodeHelper;
            try
            {
                String link = row[OfferteData.Columns.Link.ColumnName].ToString();
                String codice = row[OfferteData.Columns.Codice.ColumnName].ToString();

                var img = QRCodeHelper.Encode(qrh, link, out Exception error);

                if (error != null)
                    throw new Exception(String.Format("Error creating QR image for code '{0}' with link '{1}'", codice, link), error);

                if (img != null && !img.Size.IsEmpty)
                {
                    qrCodeFile = Path.Combine(info.Reader.ImagesPath, info.Reader.Agenzia.Replace('/', '\\'));
                    if (!Directory.Exists(qrCodeFile)) Directory.CreateDirectory(qrCodeFile);

                    qrCodeFile = Path.Combine(qrCodeFile, String.Format("{0}{1}", codice.Replace('/', '\\'), qrh.ImageFormatFileExtension()));
                    if (!Directory.Exists(Path.GetDirectoryName(qrCodeFile))) Directory.CreateDirectory(Path.GetDirectoryName(qrCodeFile));

                    qrCodeFile = qrCodeFile.Replace(@" \", @"\").Replace(@"\ ", @"\");
                    qrCodeFile = Path.Combine( Path.GetDirectoryName(qrCodeFile), Path.GetFileNameWithoutExtension(qrCodeFile).Replace(".", "").Replace(" ", "_").Replace(@":", @"_").Replace(@",", @"").Left(240) + Path.GetExtension(qrCodeFile) );
                    row[OfferteData.Columns.QRCodeFileName.ColumnName] = qrCodeFile;

                    if (File.Exists(qrCodeFile)) File.Delete(qrCodeFile);
                    if (!File.Exists(qrCodeFile)) img.Save(qrCodeFile, qrh.ImageFormat);
                }

                return true;
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                info.AddError(new Exception(String.Format(TextRes.Reader.FillXML.SAVEQRCODEERRORINTEROPFORMAT, qrCodeFile, qrh.ImageFormat.ToString(), ex.ErrorCode, ex.StackTrace), ex));
                worker.ReportProgress((int)info.Percentage, info);
            }
            catch (IOException ex)
            {
                info.AddError(new Exception(String.Format(TextRes.Reader.FillXML.SAVEQRCODEERRORINTEROPFORMAT, qrCodeFile, qrh.ImageFormat.ToString(), 0, ex.StackTrace), ex));
                worker.ReportProgress((int)info.Percentage, info);
            }
            catch (Exception ex)
            {
                worker.Logger.WriteError(TextRes.Reader.FillXML.SAVEQRCODEERRORFORMAT, ex.ToString());
                worker.ReportProgress((int)info.Percentage, info);
            }

            return false;
        }

        private static void DownloadSiteItemsFillXML(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            worker.Logger.WriteMethod(System.Reflection.MethodBase.GetCurrentMethod(), sender, e);
            var info = e.Argument as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;

            info.AddInfo(TextRes.Reader.FillXML.INFO, TextRes.Reader.FillXML.STEP);
            worker.ReportProgress((int)info.Percentage, info);

            if (info.DataInfo.ReportData == null || info.DataInfo.ReportDataCount == 0)
            {
                info.AddWarning(TextRes.Reader.FillXML.STEP, TextRes.Reader.FillXML.INFONOROWS);
                worker.ReportProgress((int)info.Percentage, info);
                e.Result = info;
                return;
            }

            Func<DataView, int, string> getJob = (view, index) => view[index][DownloadData.Columns.Job.ColumnName].ToString();
            Func<DataView, int, string> getDescription = (view, index) => view[index][DownloadData.Columns.Description.ColumnName].ToString();
            Func<DataView, int, string> getValue = (view, index) => view[index][DownloadData.Columns.Value.ColumnName].ToString();
            Func<DataView, int, DateTime> getTimeStamp = (view, index) => (DateTime)view[index][DownloadData.Columns.InsertDate.ColumnName];

            String repDescription = String.Empty;
            try
            {
                int? rowError = DownloadSiteItemsCheckHeader(worker, info);

                if (rowError != null)
                    throw new Exception(TextRes.Reader.FillXML.ERRORHEADERDATAMISSINGFORMAT.FormatWith(rowError.Value));

                if (info.Reader.FeatureCount < 0)
                    throw new Exception("Feature non inizializzate ({0})".FormatWith(info.Reader.FeatureCount));

                String repValue = String.Empty;
                // Filtro l'elenco delle righe di offerta scaricate
                var dvOfferte = new DataView(info.DataInfo.ReportData.Table
                    , info.DataInfo.ReportData.FixedRowFilter + " AND (" + String.Format("[{0}]={1}", DownloadData.Columns.Description, VdO2013SRCore.TextRes.tagOfferta.Quote()) + ")"
                    , String.Format("[{0}] ASC", DownloadData.Columns.ID)
                    , DataViewRowState.CurrentRows);


                info.AddInfo(TextRes.Reader.FillXML.INFOCOUNTFORMAT.FormatWith(dvOfferte.Count));
                worker.ReportProgress((int)info.Percentage, info);

                info.AddInfo(TextRes.Reader.FillXML.INFOLOADING);
                worker.ReportProgress((int)info.Percentage, info);

                Double itemProgressPercentage = 33.0 / (Double)dvOfferte.Count; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare

                for (int i = 0; i < dvOfferte.Count; i++)
                {
                    repDescription = getDescription(dvOfferte, i);
                    repValue = getValue(dvOfferte, i);
                    String currentOfferta = String.Format("{0}{1}", repDescription, repValue);

                    info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, String.Format(TextRes.Reader.FillXML.READOFFERTAFORMAT, i + 1, dvOfferte.Count));
                    worker.ReportProgress((int)info.Percentage, info);
                    info.AddInfo(TextRes.Reader.FillXML.INFOLOADINGOFFERTAFORMAT.FormatWith(currentOfferta));
                    worker.ReportProgress((int)info.Percentage, info);

                    DataView dvOfferta = new DataView(info.DataInfo.ReportData.Table
                        , info.DataInfo.ReportData.FixedRowFilter + " AND (" + String.Format("[{0}] LIKE {1}", DownloadData.Columns.Description.ColumnName, (currentOfferta + ".%").Quote()) + ")"
                        , String.Format("[{0}] ASC", DownloadData.Columns.ID)
                        , DataViewRowState.CurrentRows);

                    var newResultRow = info.DataInfo.ResultData.AddNew();
                    int imageIndex = 0;
                    int detailIndex = 0;
                    for (int j = 0; j < dvOfferta.Count; j++)
                    {
                        repDescription = getDescription(dvOfferta, j);
                        repValue = getValue(dvOfferta, j);

                        String repDetail = repDescription.Right(repDescription.ReverseIndexOf('.'));
                        if (repDetail.StartsWith(VdO2013SRCore.TextRes.tagDettaglioB) && repDetail.EndsWith(VdO2013SRCore.TextRes.tagDettaglioE))
                            repDetail = repDetail.Substring(VdO2013SRCore.TextRes.tagDettaglioB.Length, repDetail.Length - (VdO2013SRCore.TextRes.tagDettaglioB + VdO2013SRCore.TextRes.tagDettaglioE).Length);

                        String mappingField = String.Empty;
                        Object mappingValue = null;

                        if (!DownloadSiteItemsGetDetailMapping(worker, info, repDetail, detailIndex, ref repValue, ref imageIndex, out mappingField, out Type mappingType))
                        {
                            int ki = info.Reader.Features.IndexOf(repDetail);
                            if (ki >= 0)
                            {
                                mappingField = info.Reader.Features[ki].Mapping;
                                mappingType = Type.GetType(info.Reader.Features[ki].TypeName);
                            }
                            else
                            {
                                if (DownloadSiteItemsAddDetail(worker, info, newResultRow[OfferteData.Columns.Codice.ColumnName].ToString(), detailIndex, repDetail, repValue))
                                    detailIndex++;
                            }
                        }// if

                        if (!String.IsNullOrEmpty(mappingField))
                        {
                            int ki = info.Reader.Features.IndexOf(repDetail);
                            if (ki >= 0)
                            {
                                info.Reader.Features[ki].Apply(repValue, out repValue, out Exception error);
                                if (error != null)
                                {
                                    info.AddError(error);
                                    worker.ReportProgress((int)info.Percentage, info);
                                }

                                // Attenzione: questa parte ha senso solo se la feature di riferimento è già
                                // stata valorizzata... Dipende dal config quindi!!
                                if (String.IsNullOrEmpty(repValue) && !String.IsNullOrEmpty(info.Reader.Features[ki].IfMissingUseFeature))
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
                                worker.Logger.WriteError("Error converting value '{0}' for '{1}' into '{2}': {3}", repValue, repDetail, mappingType, ex.Message);
                                mappingValue = DBNull.Value;
                            }

#if DEBUG
                            Console.WriteLine("+-->newResultRow[{0}]={1}", mappingField, mappingValue);
#endif

                            newResultRow[mappingField] = mappingValue;
                        }// if
                    }//for (int j = 0; j < dvOfferta.Count; j++)

                    DownloadSiteItemsFillXMLCreateQRCode(worker, info, newResultRow);

                    newResultRow.EndEdit();
                    info.AddInfo(TextRes.Reader.FillXML.INFOCOMPLETED);
                    worker.ReportProgress((int)info.Percentage, info);
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
        private static void DownloadSiteItemsFillXMLProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.UserState as VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList;
            if (worker.Consumer != null)
                worker.Consumer.Notify(ProgressNotifyKind.Progress, info, info.Items.IndexOf(info.LastItem));
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
                MPLogHelper.FileLog.ConsoleRelease(out string[] console);
            }
        }
    }
}
