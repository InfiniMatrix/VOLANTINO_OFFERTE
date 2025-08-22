using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013THCore.Specialized;

namespace VdO2013TH.Consumers
{
    //[Obsolete("Test!")]
    //public class ActivationServiceCheckStatusConsumer : ProgressListConsumerBase<IOnLineCheckList>, IProgressListConsumerBase<IProgressItemCollection>, IOnLineCheckListConsumer
    //{
    //    public const String KJobName = "ActivationWebService.Status.Check";

    //    public override String JobName { get { return KJobName; } }

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

    //    #region IProgressListConsumerBase<IProgressItemCollection> Membri di

    //    public void Notify(ProgressNotifyKind kind, IProgressItemCollection info, int index)
    //    {
    //        switch (kind)
    //        {
    //            case ProgressNotifyKind.None:
    //                Console.Write("Consumer notified {0} for {1} with index {2}.", kind, info, index);
    //                break;
    //            case ProgressNotifyKind.Start:
    //                doStart(info);
    //                break;
    //            case ProgressNotifyKind.Progress:
    //                doProgress(info, index);
    //                break;
    //            case ProgressNotifyKind.Completed:
    //                doCompleted(info);
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    #endregion
    //}

    public class ActivationServiceCheckStatusConsumer2 : ProgressListConsumer2, IOnLineCheckListConsumer
    {
        public const String KJobName = "ActivationWebService.Status.Check";

        public override String JobName { get { return KJobName; } }

        #region IProgressListConsumerBase<IOnLineCheckList> Membri di
        public void Notify(ProgressNotifyKind kind, IOnLineCheckList info, int index)
        {
            base.Notify(kind, info, index);
        }
        #endregion
    }
}
