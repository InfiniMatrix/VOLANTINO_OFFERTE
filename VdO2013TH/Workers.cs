using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;

using System.Net.Mail;

using MPMailHelper;

using VdO2013Core;
using VdO2013THCore;

namespace VdO2013TH
{
    public abstract partial class Workers
    {
        private static MPLogHelper.FileLog Logger = new MPLogHelper.FileLog(System.Reflection.MethodBase.GetCurrentMethod());
        public const String CheckOnLineWorkerName = "CheckOnLine";
        public const String DownloadSiteItemsWorkerName = "DownloadSiteItems";
        public const String DownloadItemsFillXmlWorkerName = DownloadSiteItemsWorkerName + ".FillXML";
        public const String DownloadItemsFillDbWorkerName = DownloadSiteItemsWorkerName + ".FillDB";
        public const String MailSendWorkerName = "MailSend";

        private static void LogConsoleOutput(Double percentage = 0, String step = "console", int status = -1)
        {
            if (MPLogHelper.FileLog.ConsoleCaptured)
            {
                Boolean replicated = MPLogHelper.FileLog.ReplicateOnConsole;
                try
                {
                    MPLogHelper.FileLog.ReplicateOnConsole = false;

                    if (MPLogHelper.FileLog.ConsoleRelease(out string[] console))
                    {
                        foreach (var s in console)
                        {
                            Logger.WriteInfo("Percentage: {0}\tStep: {1}\tStatus: {2}\tMessage: {3}", percentage, step, status, s);
                        }
                    }
                }
                finally
                {
                    MPLogHelper.FileLog.ReplicateOnConsole = replicated;
                }// try
            }
        }

        /// <summary>
        /// Costruttore statico che inizializza tutti i BackgroundWorker e li registra
        /// </summary>
        /// <param name="main"></param>
        [Obsolete("Rimettere DownloadSiteItems!")]
        static Workers()
        {
#if DEBUG
            MPLogHelper.FileLog.Default.WriteCtor(System.Reflection.MethodBase.GetCurrentMethod());
#endif
            //IBackgroundWorkerEx w = null;
            
            //w = new BackgroundWorkerEx(BackgroundWorkerBlocking.None);
            //w.WorkerReportsProgress = true;
            //w.DoWork += new DoWorkEventHandler(CheckOnLine);
            //w.ProgressChanged += new ProgressChangedEventHandler(CheckOnLineProgressChanged);
            //w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CheckOnLineCompleted);
            //Global.AddWorker(CheckOnLineWorkerName, w);
            Global.AddWorker(Workers.CheckOnLineWorkerName, new CheckOnLineWorker());

            //w = new BackgroundWorkerEx(BackgroundWorkerBlocking.UserInterface);
            //w.WorkerReportsProgress = true;
            //w.DoWork += new DoWorkEventHandler(DownloadSiteItems);
            //w.ProgressChanged += new ProgressChangedEventHandler(DownloadSiteItemsProgressChanged);
            //w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadSiteItemsCompleted);
            //Global.AddWorker(DownloadSiteItemsWorkerName, w);
            Global.AddWorker(Workers.DownloadSiteItemsWorkerName, new DownloadSiteItemsWorker());

            //w = new BackgroundWorkerEx(BGWorkerBlocking.UserInterface);
            //w.WorkerReportsProgress = true;
            //w.DoWork += new DoWorkEventHandler(DownloadSiteItemsFillDB);
            //w.ProgressChanged += new ProgressChangedEventHandler(DownloadSiteItemsFillDBProgressChanged);
            //w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadSiteItemsFillDBCompleted);
            //Global.AddWorker(DownloadItemsFillDbName, w);
            Global.AddWorker(Workers.DownloadItemsFillDbWorkerName, new DownloadSiteItemsFillDBWorker());

            //w = new BackgroundWorkerEx(BackgroundWorkerBlocking.UserInterface);
            //w.WorkerReportsProgress = true;
            //w.DoWork += new DoWorkEventHandler(DownloadSiteItemsFillXML);
            //w.ProgressChanged += new ProgressChangedEventHandler(DownloadSiteItemsFillXMLProgressChanged);
            //w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadSiteItemsFillXMLCompleted);
            //Global.AddWorker(DownloadItemsFillXmlWorkerName, w);
            Global.AddWorker(Workers.DownloadItemsFillXmlWorkerName, new DownloadSiteItemsFillXMLWorker());

            //w = new BackgroundWorkerEx(BackgroundWorkerBlocking.None);
            //w.WorkerReportsProgress = true;
            //w.DoWork += new DoWorkEventHandler(MailSend);
            //w.ProgressChanged += new ProgressChangedEventHandler(MailSendProgressChanged);
            //w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(MailSendCompleted);
            //Global.AddWorker(MailSendWorkerName, w);
            Global.AddWorker(Workers.MailSendWorkerName, new MailSendWorker());
        }

        private static void Consumer_Started(IProgressListConsumerBase consumer, IProgressItemCollection info)
        {
            Logger.WriteWarn("Starting thread [Consumer:{0,-30}(Job:{1,-30}), Info:{2,-30}(Job:{3,-30})]"
                , consumer
                , consumer.JobName
                , info
                , info.JobName
            );
        }

        private static void Consumer_Completed(IProgressListConsumerBase consumer, IProgressItemCollection info)
        {
            Logger.WriteWarn("Exiting thread [Consumer:{0,-30}(Job:{1,-30}), Info:{2,-30}(Job:{3,-30})]"
                , consumer
                , consumer.JobName
                , info
                , info.JobName
            );
        }

        public static int CheckOnLineAsync(VdO2013THCore.Specialized.IOnLineCheckList info, VdO2013THCore.Specialized.IOnLineCheckListConsumer consumer)
        {
            var w = Global.GetWorker(Workers.CheckOnLineWorkerName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection> ?? throw new Exception("Consumer missing.");

            MPLogHelper.FileLog.ConsoleCapture();

            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;

            w.Consumer.Notify(ProgressNotifyKind.Start, info, -1);

            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;

            w.RunWorkerAsync(info);
            return 0;
        }

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
                            Logger.WriteInfo(format, li.Step.PadRight(25), (int)li.Percentage, li.Status, li.Description);
                            break;
                        case ProgressItemKind.Warning:
                            Logger.WriteWarn(format, li.Step.PadRight(25), (int)li.Percentage, li.Status, li.Description);
                            break;
                        case ProgressItemKind.Error:
                            Logger.WriteError(format, li.Step.PadRight(25), (int)li.Percentage, li.Status, li.Description);
                            break;
                        default:
                            break;
                    }

                    li.Status = ProgressItemStatus.Accomplished;
                }//if (li.Status != ProgressItemStatus.Accomplished)
            }
        }
        public static int DownloadItemsAsync(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataListConsumer consumer)
        {
            var w = Global.GetWorker(DownloadSiteItemsWorkerName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection> ?? throw new Exception("Consumer missing.");

            MPLogHelper.FileLog.ConsoleCapture();
            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;

            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;

            w.RunWorkerAsync(info);

            return 0;
        }
        public static int DownloadItemsFillDBAsync(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataListConsumer consumer)
        {
            var w = Global.GetWorker(DownloadItemsFillDbWorkerName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection> ?? throw new Exception("Consumer missing.");

            MPLogHelper.FileLog.ConsoleCapture();
            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;

            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;

            w.RunWorkerAsync(info);

            return 0;
        }
        public static int DownloadItemsFillXmlAsync(VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataListConsumer consumer)
        {
            var w = Global.GetWorker(DownloadItemsFillXmlWorkerName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection> ?? throw new Exception("Consumer missing.");

            MPLogHelper.FileLog.ConsoleCapture();
            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;

            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;

            w.RunWorkerAsync(info);

            return 0;
        }

        public static int MailSendAsync(VdO2013THCore.Specialized.IMailSendList info, VdO2013THCore.Specialized.IMailSendListConsumer consumer)
        {
            var w = Global.GetWorker(MailSendWorkerName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection> ?? throw new Exception("Consumer missing.");

            MPLogHelper.FileLog.ConsoleCapture();
            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;

            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;

            w.RunWorkerAsync(info);
            return 0;
        }
    }
}
