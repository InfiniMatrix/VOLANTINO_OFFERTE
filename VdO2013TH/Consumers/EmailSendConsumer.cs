using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013TH;
using VdO2013THCore;
using VdO2013THCore.Specialized;

namespace VdO2013TH.Consumers
{
    //[Obsolete("Test!")]
    //public class MailSendConsumer : ProgressListConsumerBase<VdO2013THCore.Specialized.IMailSendList>, VdO2013THCore.Specialized.IMailSendListConsumer
    //{
    //    public const String KJobName = "Mail.Send";

    //    public override String JobName { get { return KJobName; } }

    //    public void NotifyStart(VdO2013THCore.Specialized.IMailSendList info)
    //    {
    //        doStart(info);
    //    }

    //    public void NotifyProgress(VdO2013THCore.Specialized.IMailSendList info, int index)
    //    {
    //        doProgress(info, index);
    //    }

    //    [Obsolete("Usare eventi!!", true)]
    //    public void NotifyCompleted(VdO2013THCore.Specialized.IMailSendList info)
    //    {
    //        doCompleted(info);
    //    }

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

    public class MailSendConsumer2 : ProgressListConsumer2, IMailSendListConsumer
    {
        public const String KJobName = "Mail.Send";

        public override String JobName { get { return KJobName; } }

        #region Membri di IProgressListConsumerBase<IMailSendList>
        public void Notify(ProgressNotifyKind kind, IMailSendList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }
}
