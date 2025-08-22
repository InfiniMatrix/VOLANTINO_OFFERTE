using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Mail;

using MPLogHelper;
using MPMailHelper;

using VdO2013Core;
using VdO2013THCore;

namespace VdO2013TH
{
    public partial class Workers
    {

        #region MailSend
        private static void MailSend(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Argument as VdO2013THCore.Specialized.IMailSendList;

            info.AddInfo(0, "Invio email", "Inizializzazione");
            worker.ReportProgress((int)info.Percentage, info);

            int msgCount = info.List.Count;
            if (msgCount == 0)
            {
                info.AddError(new Exception("Nessun messaggio nella coda!"));
                worker.ReportProgress((int)info.Percentage, info);
                e.Result = info;
                return;
            }//if (rowCount == 0)


            worker.ReportProgress((int)info.Percentage, info);
            try
            {
                Exception error = null;
                Double percentageStep = 100 / msgCount;
                var messages = info.List.Keys.ToList<MailMessage>();
                for (int i = 0; i < msgCount; i++)
                {
                    var message = messages[i];
                    info.AddInfo(info.LastItem.Percentage + percentageStep, String.Format("Invio '{0}'...", message.Subject));
                    worker.ReportProgress((int)info.Percentage, info);

                    error = null;

                    MPMailHelper.MailSend.Send(info.SmtpServer, message, out error, info.Credential, info.SmtpServerPort, info.EnableSSL, false, info.RetryCount, info.RetryDelay);

                    // Attendo un minuto prima di uscire per consentire il completamento
                    // dell'invio: stranamente MailSend anche sincrono sembra impiegare
                    // più tempo del comando stesso, come se fosse sempre async.... mah...
                    System.Threading.Thread.Sleep(60000);

                    if (error != null)
                    {
                        info.List[message] = MPMailHelper.MailSend.SmtpStatusCode;
                        info.AddError(error);
                        worker.ReportProgress((int)info.Percentage, info);
                    }
                    else
                    {
                        info.List[message] = MPMailHelper.MailSend.SmtpStatusCode;
                        worker.ReportProgress((int)info.Percentage, info);
                    }//if (error != null)

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

        private static void MailSendProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.UserState as VdO2013THCore.Specialized.IMailSendList;
            if (worker.Consumer != null)
                worker.Consumer.Notify(ProgressNotifyKind.Progress, info, info.Items.ToList().IndexOf(info.LastItem));
        }

        private static void MailSendCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender as BackgroundWorkerEx;
            var info = e.Result as VdO2013THCore.Specialized.IMailSendList;

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

        #endregion MailSend

        public static int MailSendAsync(VdO2013THCore.Specialized.IMailSendList info, VdO2013THCore.Specialized.IMailSendListConsumer consumer)
        {
            var w = Global.GetWorker(MailSendWorkerName);
            if (w == null || w.IsBusy) return -1;

            if (info == null) { throw new Exception("Info missing."); }
            if (consumer == null) { throw new Exception("Consumer missing."); }

            FileLog.ConsoleCapture();
            if (info.ShowProgress) w.Blocking = BackgroundWorkerBlocking.UserInterface;
            w.Consumer = consumer as IProgressListConsumer<IProgressItemCollection>;
            
            w.Consumer.Started -= Consumer_Started;
            w.Consumer.Started += Consumer_Started;

            w.Consumer.Completed -= Consumer_Completed;
            w.Consumer.Completed += Consumer_Completed;
            
            w.RunWorkerAsync(info);
            return 0;
        }
    }
}
