using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MPLogHelper;
using VdO2013Core;
using VdO2013THCore;
using VdO2013THCore.Specialized;
using VdO2013SRCore;

namespace VdO2013TH.Consumers
{
    //[Obsolete("Test!")]
    //public class SiteReaderResultTableLinkCheckStatusConsumer : ProgressListConsumerBase<IOnLineCheckList>, IOnLineCheckListConsumer
    //{
    //    public const String KJobName = "SiteReaderResultTableLink.Status.Check";
    //    public override String JobName { get { return KJobName; } }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyStart(IOnLineCheckList info)
    //    {
    //        doStart(info);
    //    }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyProgress(IOnLineCheckList info, int index)
    //    {
    //        doProgress(info, index);
    //    }

    //    [Obsolete("Usare eventi!!")]
    //    public void NotifyCompleted(IOnLineCheckList info)
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

    public class SiteReaderResultTableLinkCheckStatusConsumer2 : ProgressListConsumer2, IOnLineCheckListConsumer
    {
        public const String KJobName = "SiteReaderResultTableLink.Status.Check";

        public override String JobName { get { return KJobName; } }

        #region Membri di IProgressListConsumerBase<IOnLineCheckList>
        public void Notify(ProgressNotifyKind kind, IOnLineCheckList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }
}
