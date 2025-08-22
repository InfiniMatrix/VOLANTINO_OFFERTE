using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013THCore;
using VdO2013THCore.Specialized;

namespace VdO2013TH.Consumers
{
    //[Obsolete("Test!")]
    //public class SiteReaderWebSiteCheckStatusConsumer : ProgressListConsumerBase<IOnLineCheckList>, IOnLineCheckListConsumer
    //{
    //    public const String KJobName = "SiteReaderWebSite.Status.Check";

    //    public override String JobName { get { return KJobName; } }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyStart(VdO2013THCore.Specialized.IOnLineCheckList info)
    //    {
    //        doStart(info);
    //    }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyProgress(VdO2013THCore.Specialized.IOnLineCheckList info, int index)
    //    {
    //        doProgress(info, index);
    //    }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyCompleted(VdO2013THCore.Specialized.IOnLineCheckList info)
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

    public class SiteReaderWebSiteCheckStatusConsumer2 : ProgressListConsumer2, IOnLineCheckListConsumer
    {
        public const String KJobName = "SiteReaderWebSite.Status.Check";

        public override String JobName { get { return KJobName; } }

        #region IProgressListConsumerBase<IOnLineCheckList> Membri di
        public void Notify(ProgressNotifyKind kind, IOnLineCheckList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }
}
