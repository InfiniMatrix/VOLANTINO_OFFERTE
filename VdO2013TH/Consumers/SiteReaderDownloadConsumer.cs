using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using MPLogHelper;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;
using VdO2013THCore;
using VdO2013TH;

namespace VdO2013TH.Consumers
{
    //[Obsolete("Test!")]
    //public class SiteReaderDownloadConsumer : ProgressListConsumerBase<ISiteReaderOnLineCheckDataList>, ISiteReaderOnLineCheckDataListConsumer
    //{
    //    public const String KJobName = "SiteReader.Download";
    //    public const String KJobFillDBName = "SiteReader.Download.FillDB";
    //    public const String KJobFillXMLName = "SiteReader.Download.FillXML";

    //    public override String JobName { get { return KJobName; } }
    //    public String JobFillDBName { get { return KJobFillDBName; } }
    //    public String JobFillXMLName { get { return KJobFillXMLName; } }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyStart(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
    //    {
    //        doStart(info);
    //        //if (info.ShowProgress)
    //        //{
    //        //    SplashMgr.WaitFormShow(Iinfo.Reader.MainForm);
    //        //    SplashMgr.SetCaption(TextRes.readerDownloadStep);
    //        //}

    //        //Log.WriteWarn("Inizio scaricamento dati(Reader:{0};\tModalità:{1};\tJob:{2})", info.Reader.Name, info.Reader.Mode, info.Job);
    //        //info.Reader.MainForm.doEnableButtons();
    //    }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyProgress(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, int index)
    //    {
    //        doProgress(info, index);
    //        //var li = info.LastItem;
    //        //if (li != null && li.Status != ProgressItemStatus.Accomplished)
    //        //{
    //        //    info.Reader.MainForm.ctlLogAdd(li.Percentage, li.Step, li.Description, li.IsError ? -1 : 0);
    //        //    if (info.ShowProgress)
    //        //    {
    //        //        SplashMgr.SetDescription(li.Description);
    //        //        SplashMgr.SetProgress((int)li.Percentage);
    //        //    }

    //        //    if (li.IsError)
    //        //    {
    //        //        if (!String.IsNullOrEmpty(info.DataInfo.SqlCommand))
    //        //        {
    //        //            info.Reader.MainForm.ctlLogAdd(li.Percentage, li.Step, "->" + info.DataInfo.SqlCommand);
    //        //            info.Reader.MainForm.ctlLogAdd(li.Percentage, li.Step, "->" + info.DataInfo.SqlResult.ToString());
    //        //        }
    //        //    }

    //        //    li.Status = ProgressItemStatus.Accomplished;
    //        //}
    //    }

    //    #region Implementation
    //    private void downloadCompleted(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
    //    {
    //        doCompleted(info);
    //        //if (info.ShowProgress)
    //        //    SplashMgr.WaitFormHide(info.Reader.MainForm);

    //        //info.Reader.MainForm.ctlLogAdd(info.Percentage, info.Step, String.Format(TextRes.readerDownloadedInfoAgenziaFormat, info.Reader.Agenzia));
    //        //info.Reader.MainForm.ctlLogAdd(info.Percentage, info.Step, String.Format(TextRes.readerDownloadedInfoRagioneSocialeFormat, info.Reader.RagioneSociale));
    //        //info.Reader.MainForm.ctlLogAdd(info.Percentage, info.Step, String.Format(TextRes.readerDownloadedInfoTipologiaFormat, info.Reader.Tipologia));
    //        //info.Reader.MainForm.ctlLogAdd(info.Percentage, info.Step, String.Format(TextRes.readerDownloadedInfoElementiFormat, info.Reader.Elementi));

    //        //if (info.Cancel)
    //        //{
    //        //    info.Reader.MainForm.ctlLogAdd(info.Percentage, info.Step, TextRes.readerDownloadedInfoCompletedErrors, -1);
    //        //    foreach (var er in info.Errors)
    //        //    {
    //        //        if (er.Status != ProgressItemStatus.Accomplished)
    //        //            info.Reader.MainForm.ctlLogAdd(info.Percentage, info.Step, er.ToString(), -1);
    //        //        er.Status = ProgressItemStatus.Accomplished;
    //        //    }//foreach (var er in info.Errors)
    //        //    return;
    //        //}//if (info.Cancel)


    //        //Exception error = null;
    //        //if (info.DataInfo.IsDataBound)
    //        //{
    //        //    String fSql = String.Format("SELECT COUNT(*) FROM [{0}] WHERE [Description]='Offerta'", DataHelpers.SiteReaderDownloadLog.Table.GetName());
    //        //    int? rCount = (int)DataHelpers.Reader.ExecuteScalar(ProgramArgsHelper.ConnectionString, fSql, out error);

    //        //    if (rCount != null && error == null)
    //        //        info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, String.Format(TextRes.readerDownloadedInfoElementiScrittiFormat, rCount));

    //        //    info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, TextRes.readerDownloadedInfoCompleted);

    //        //    if (Global.DebugLevel > 0 || MsgMgr.ShowQuestion(info.Reader.MainForm, TextRes.readerDownloadedInfoCompletedContinue) == System.Windows.Forms.DialogResult.Yes)
    //        //    {
    //        //        info.Job = JobFillDBName;
    //        //        Workers.DownloadItemsFillDbAsync(info, this);
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    DataHelpers.SiteReaderDownloadLog.Table.Save(info.DataInfo.ReportTable, info.Reader, out error);
    //        //    if (error != null)
    //        //    {
    //        //        info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, error.ToString());
    //        //        MsgMgr.ShowError(info.Reader.MainForm, error);
    //        //    }

    //        //    info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, String.Format(TextRes.readerDownloadedInfoElementiScrittiFormat, info.DataInfo.ReportTableCount));
    //        //    info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, TextRes.readerDownloadedInfoCompleted);

    //        //    if (Global.DebugLevel > 0 || MsgMgr.ShowQuestion(info.Reader.MainForm, TextRes.readerDownloadedInfoCompletedContinue) == System.Windows.Forms.DialogResult.Yes)
    //        //    {
    //        //        info.Job = JobFillXMLName;
    //        //        Workers.DownloadItemsFillXmlAsync(info, this);
    //        //    }
    //        //}//if (info.DataInfo.IsDataBound)

    //        //if (error != null)
    //        //{
    //        //    info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, error.ToString());
    //        //    MsgMgr.ShowError(info.Reader.MainForm, error);
    //        //}

    //        //info.Reader.MainForm.ctlLogSave();
    //        //info.Reader.MainForm.doEnableButtons();
    //    }
    //    private void fillDBCompleted(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
    //    {
    //        doCompleted(info);
    //        //if (info.ShowProgress)
    //        //    SplashMgr.WaitFormHide(info.Reader.MainForm);

    //        //var ee = info.Errors;
    //        //if (ee != null && ee.Count() > 0)
    //        //{
    //        //    var le = ee.Last();
    //        //    if (le != null && le.Status != ProgressItemStatus.Accomplished)
    //        //    {
    //        //        info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, TextRes.readerFilledDBInfoCompletedErrors, -1);
    //        //        info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, le.ToString(), -1);
    //        //    }
    //        //    else
    //        //        info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, TextRes.readerFilledDBInfoCompleted);
    //        //}

    //        //info.Reader.MainForm.ctlLogSave();
    //        //info.Reader.MainForm.doEnableButtons();

    //        //if (Log.ConsoleCaptured)
    //        //{
    //        //    String[] console = null;
    //        //    Log.ConsoleRelease(out console);
    //        //}
    //    }
    //    private void fillXMLCompleted(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
    //    {
    //        doCompleted(info);
    //        //if (info.ShowProgress)
    //        //    SplashMgr.WaitFormHide(info.Reader.MainForm);

    //        //var ee = info.Errors;
    //        //if (ee != null && ee.Count() > 0)
    //        //{
    //        //    info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, TextRes.readerFilledXMLInfoCompletedErrors, -1);
    //        //    foreach (var le in ee)
    //        //    {
    //        //        if (le != null)// && le.Status != Threading.ProgressItemStatus.Accomplished
    //        //        {
    //        //            info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, le.ToString(), -1);
    //        //        }
    //        //    }//foreach (var le in ee)
    //        //}

    //        //Exception error = null;
    //        //switch (info.Reader.Mode)
    //        //{
    //        //    case SiteReaderMode.Riscarica:
    //        //        DataHelpers.SiteReaderDownloadResults.Table.Save(info.DataInfo.ResultTable, info.Reader.MainForm.Reader, out error);
    //        //        DataHelpers.SiteReaderDownloadResultDetails.Table.Save(info.DataInfo.DetailsTable, info.Reader.MainForm.Reader, out error);
    //        //        break;
    //        //    case SiteReaderMode.Aggiorna:
    //        //    case SiteReaderMode.Aggiungi:
    //        //    case SiteReaderMode.Aggiungi_E_Aggiorna:

    //        //        DataTable currentResult = DataHelpers.SiteReaderDownloadResults.Table.Create(info.Reader, out error);
    //        //        if (currentResult != null)
    //        //        {
    //        //            var rowExclusions = new Dictionary<String, Object>() { { DataHelpers.SiteReaderDownloadResults.Columns.Status, ResultTableItemStatus.Locked.ToString() } };
    //        //            var colExclusions = new List<String>() { DataHelpers.SiteReaderDownloadResults.Columns.Ordinal };
    //        //            DataHelpers.Reader.TableBase.TableMerge(ref currentResult
    //        //                , info.DataInfo.ResultTable
    //        //                , DataHelpers.SiteReaderDownloadResults.Columns.Codice
    //        //                , info.Reader.Mode == SiteReaderMode.Aggiorna || info.Reader.Mode == SiteReaderMode.Aggiungi_E_Aggiorna
    //        //                , info.Reader.Mode == SiteReaderMode.Aggiungi || info.Reader.Mode == SiteReaderMode.Aggiungi_E_Aggiorna
    //        //                , rowExclusions
    //        //                , colExclusions);

    //        //            var changes = 0;
    //        //            for (int i = 0; i < currentResult.Rows.Count; i++)
    //        //            {
    //        //                var sLink = "{0}".FormatWith(currentResult.Rows[i][DataHelpers.SiteReaderDownloadResults.Columns.Link]);
    //        //                sLink = sLink.Replace(info.Reader.MobileSiteRootUrl, info.Reader.StandardSiteRootUrl);
    //        //                if (!String.IsNullOrEmpty(sLink) && Uri.IsWellFormedUriString(sLink, UriKind.Absolute))
    //        //                {
    //        //                    var currentLink = new Uri(sLink);
    //        //                    String sStatus = "{0}".FormatWith(currentResult.Rows[i][DataHelpers.SiteReaderDownloadResults.Columns.Status]);
    //        //                    OnLineStatus currentStatus = OnLineStatus.None;
    //        //                    var found = info.List.Keys.Contains(currentLink);
    //        //                    var parsed = Enum.TryParse<OnLineStatus>(sStatus, out currentStatus);
    //        //                    if (found && parsed && info.List[currentLink] != currentStatus)
    //        //                    {
    //        //                        currentResult.Rows[i][DataHelpers.SiteReaderDownloadResults.Columns.Status] = info.List[currentLink].ToString();
    //        //                        changes++;
    //        //                    }
    //        //                }
    //        //            }// for (int i = 0; i < currentResult.Rows.Count; i++)

    //        //            if (changes > 0)
    //        //                currentResult.AcceptChanges();
    //        //        }

    //        //        DataHelpers.SiteReaderDownloadResults.Table.Save(currentResult, info.Reader.MainForm.Reader, out error);

    //        //        DataTable currentDetails = DataHelpers.SiteReaderDownloadResultDetails.Table.Create(info.Reader, out error);
    //        //        if (currentDetails != null)
    //        //        {
    //        //            var rowExclusions = new Dictionary<String, Object>() { };
    //        //            var colExclusions = new List<String>() { };
    //        //            DataHelpers.Reader.TableBase.TableMerge(ref currentDetails
    //        //                , info.DataInfo.DetailsTable
    //        //                , new String[] { DataHelpers.SiteReaderDownloadResultDetails.Columns.CodiceOfferta, DataHelpers.SiteReaderDownloadResultDetails.Columns.Descrizione }
    //        //                , true
    //        //                , true
    //        //                , rowExclusions
    //        //                , colExclusions);
    //        //        }
    //        //        DataHelpers.SiteReaderDownloadResultDetails.Table.Save(currentDetails, info.Reader.MainForm.Reader, out error);
    //        //        break;
    //        //    default:
    //        //        break;
    //        //}

    //        //if (error != null)
    //        //{
    //        //    MsgMgr.ShowError(info.Reader.MainForm, error);
    //        //    return;
    //        //}

    //        //info.Reader.MainForm.ctlLogAdd(info.Percentage, info.LastItem.Step, TextRes.readerFilledXMLInfoCompleted);
    //        //info.Reader.MainForm.doSelectResultsTab();
    //        //info.Reader.MainForm.ctlReport.Reader = info.Reader.MainForm.Reader;
    //        //info.Reader.MainForm.ctlReport.Restore();

    //        //info.Reader.MainForm.ctlResults.Reader = info.Reader.MainForm.Reader;
    //        //info.Reader.MainForm.ctlResults.Restore();

    //        ////if (ProgramArgsHelper.ReportEditorEnabled)
    //        ////    info.Reader.MainForm.doLoadDataIntoReportDesigner();

    //        //info.Reader.MainForm.ctlLogSave();
    //        //info.Reader.MainForm.doEnableButtons();

    //        //if (Log.ConsoleCaptured)
    //        //{
    //        //    String[] console = null;
    //        //    Log.ConsoleRelease(out console);
    //        //}
    //    }
    //    #endregion

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyCompleted(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info)
    //    {
    //        switch (info.JobName)
    //        {
    //            case KJobName:
    //                downloadCompleted(info);
    //                break;
    //            case KJobFillDBName:
    //                fillDBCompleted(info);
    //                break;
    //            case KJobFillXMLName:
    //                fillXMLCompleted(info);
    //                break;
    //            default:
    //                break;
    //        }

    //        FileLog.WriteWarn("Completed(Reader:{0};\tModalità:{1};\tJob:{2})", info.Reader.Name, info.Reader.Mode, info.JobName);
    //    }

    //    //public override void Notify(ISiteReader2 reader, ProgressNotifyKind kind, object info, int index)
    //    //{
    //    //    switch (kind)
    //    //    {
    //    //        case ProgressNotifyKind.None:
    //    //            break;
    //    //        case ProgressNotifyKind.Start:
    //    //            NotifyStart(reader, info as ISiteReaderOnLineDataList);
    //    //            break;
    //    //        case ProgressNotifyKind.Progress:
    //    //            NotifyProgress(reader, info as ISiteReaderOnLineDataList, index);
    //    //            break;
    //    //        case ProgressNotifyKind.Completed:
    //    //            NotifyCompleted(reader, info as ISiteReaderOnLineDataList);
    //    //            break;
    //    //        default:
    //    //            break;
    //    //    }
    //    //}

    //    protected override void doStart(Object info)
    //    {
    //        if (onStart != null) onStart(this, info);
    //    }

    //    protected override void doProgress(Object info, int index)
    //    {
    //        if (onProgress != null) onProgress(this, info, index);
    //    }

    //    protected override void doCompleted(Object info)
    //    {
    //        if (onCompleted != null) onCompleted(this, info);
    //    }
    //}

    public class SiteReaderDownloadConsumer2 : ProgressListConsumer2, ISiteReaderOnLineCheckDataListConsumer
    {
        public const string KJobName = "SiteReader.Download";

        public override string JobName { get { return KJobName; } }

        #region Membri di IProgressListConsumerBase<IMailSendList>
        public void Notify(ProgressNotifyKind kind, ISiteReaderOnLineCheckDataList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }

    public class SiteReaderDownloadConsumer2FillDB : ProgressListConsumer2, ISiteReaderOnLineCheckDataListConsumer
    {
        public const string KJobName = "SiteReader.Download.FillDB";

        public override string JobName { get { return KJobName; } }

        #region Membri di IProgressListConsumerBase<IMailSendList>
        public void Notify(ProgressNotifyKind kind, ISiteReaderOnLineCheckDataList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }

    public class SiteReaderDownloadConsumer2FillXML : ProgressListConsumer2, ISiteReaderOnLineCheckDataListConsumer
    {
        public const string KJobName = "SiteReader.Download.FillXML";

        public override string JobName { get { return KJobName; } }

        #region Membri di IProgressListConsumerBase<IMailSendList>
        public void Notify(ProgressNotifyKind kind, ISiteReaderOnLineCheckDataList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }
}
