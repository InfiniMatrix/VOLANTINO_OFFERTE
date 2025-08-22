using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013THCore;

using MPLogHelper;

namespace VdO2013TH
{
    public partial class Workers
    {
        public const String CheckOnLineName = "CheckOnLine";
        #region CheckOnLine
        private static void CheckOnLine(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Argument as VdO2013THCore.Specialized.IOnLineCheckList;

            info.AddInfo(0, "Verifica online", "Verifica");
            worker.ReportProgress((int)info.Percentage, info);

            int urlCount = info.List.Count;
            if (urlCount == 0)
            {
                info.AddError(new Exception("Nessuna url selezionata!"));
                worker.ReportProgress((int)info.Percentage, info);
                e.Result = info;
                return;
            }//if (rowCount == 0)


            worker.ReportProgress((int)info.Percentage, info);
            try
            {
                Exception error = null;
                Double percentageStep = 99 / urlCount; //L'ultimo step avverrà fuori dal thread
                var urls = info.List.Keys.ToList<Uri>();
                for (int i = 0; i < urlCount; i++)
                {
                    var url = urls[i];
                    info.AddInfo(info.LastItem.Percentage + percentageStep, String.Format("Apertura {0}...", url));
                    worker.ReportProgress((int)info.Percentage, info);

                    String newStatus = String.Empty;
                    error = null;
                    if (HtmlHelper.OpenUrl(url, out error, true) != System.Net.HttpStatusCode.OK || error != null)
                    {
                        info.AddError(error);
                        info.List[url] = error is HttpRedirectNotAllowedException ? OnLineStatus.Redirected : OnLineStatus.Offline;
                        worker.ReportProgress((int)info.Percentage, info);
                    }
                    else
                    {
                        info.List[url] = OnLineStatus.OnLine;
                    }//if (!HtmlHelper.OpenUrl(url, out error))

                }//for (int i = 0; i < urlCount; i++)
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
        private static void CheckOnLineProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.UserState as VdO2013THCore.Specialized.IOnLineCheckList;
            if (worker.Consumer != null)
                worker.Consumer.Notify(ProgressNotifyKind.Progress, info, info.Items.IndexOf(info.LastItem));
        }
        private static void CheckOnLineCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Result as VdO2013THCore.Specialized.IOnLineCheckList;

            if (worker.Consumer != null)
            {
                worker.Consumer.Notify(ProgressNotifyKind.Completed, info, -1);
                worker.Reset();
            }
            if (FileLog.ConsoleCaptured)
            {
                String[] console = null;
                FileLog.ConsoleRelease(out console);
            }
        }
        #endregion CheckOnLine

        public static int CheckOnLineAsync(VdO2013THCore.Specialized.IOnLineCheckList info, VdO2013THCore.Specialized.IOnLineCheckListConsumer consumer)
        {
            var w = Global.GetWorker(CheckOnLineName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            if (consumer == null) { throw new Exception("Consumer missing."); }

            FileLog.ConsoleCapture();

            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;

            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection>;
            w.Consumer.Notify(ProgressNotifyKind.Start, info, -1);

            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;

            w.RunWorkerAsync(info);
            return 0;
        }
    }
}
