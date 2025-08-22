using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using VdO2013Core;
using VdO2013SRCore;

using MPLogHelper;

namespace VdO2013TH
{
    internal class CheckOnLineWorker : BackgroundWorkerEx
    {
        public CheckOnLineWorker()
            : base(BackgroundWorkerBlocking.None)
        {
            this.WorkerReportsProgress = true;
            this.DoWork += CheckOnLine;
            this.ProgressChanged += CheckOnLineProgressChanged;
            this.RunWorkerCompleted += CheckOnLineCompleted;
        }

        private static void CheckOnLine(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Argument as VdO2013THCore.Specialized.IOnLineCheckList;
            if (worker == null) return;
            if (info == null) return;

            info.AddInfo(0, "Verifica online", "Verifica");
            worker.ReportProgress((int)info.Percentage, info);

            var urlCount = info.List.Count;
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
                double percentageStep = 99.0 / urlCount; //L'ultimo step avverrà fuori dal thread
                var urls = info.List.Keys.ToList<Uri>();
                for (var i = 0; i < urlCount; i++)
                {
                    var url = urls[i];
                    info.AddInfo(info.LastItem.Percentage + percentageStep, $"Apertura {url}...");
                    worker.ReportProgress((int)info.Percentage, info);

                    var newStatus = string.Empty;
                    error = null;
                    if (HtmlHelper.OpenUrl(url, out error, HtmlHelper.UriPart.Query) != System.Net.HttpStatusCode.OK || error != null)
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
            worker?.Consumer?.Notify(ProgressNotifyKind.Progress, info, info.Items.IndexOf(info.LastItem));
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
                FileLog.ConsoleRelease(out var console);
            }
        }
    }
}
