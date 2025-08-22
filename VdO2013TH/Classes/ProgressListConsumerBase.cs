using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013THCore;

namespace VdO2013TH
{
    //public abstract class ProgressListConsumerBase<TProgressList> : IProgressListConsumerBase<TProgressList> where TProgressList : IProgressItemCollection
    //{
    //    public const String NotifyStartMethodName = "NotifyStart";
    //    public const String NotifyProgressMethodName = "NotifyProgress";
    //    public const String NotifyCompletedMethodName = "NotifyCompleted";

    //    public abstract String JobName { get; }

    //    public void Notify(ProgressNotifyKind kind, TProgressList info, int index)
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

    //    protected abstract void doStart(Object info);
    //    protected abstract void doProgress(Object info, int index);
    //    protected abstract void doCompleted(Object info);


    //    #region Membri di IProgressListConsumerBase
    //    protected ProgressListConsumerStartedDelegate onStart;
    //    event ProgressListConsumerStartedDelegate IProgressListConsumerBase<TProgressList>.OnStart
    //    {
    //        add { onStart += value; }
    //        remove { onStart -= value; }
    //    }
    //    public event ProgressListConsumerStartedDelegate OnStart
    //    {
    //        add { onStart += value; }
    //        remove { onStart -= value; }
    //    }
        
    //    protected ProgressListConsumerProgressDelegate onProgress;
    //    event ProgressListConsumerProgressDelegate IProgressListConsumerBase<TProgressList>.OnProgress
    //    {
    //        add { onProgress += value; }
    //        remove { onProgress -= value; }
    //    }
    //    public event ProgressListConsumerProgressDelegate OnProgress
    //    {
    //        add { onProgress += value; }
    //        remove { onProgress -= value; }
    //    }

    //    protected ProgressListConsumerCompletedDelegate onCompleted;
    //    event ProgressListConsumerCompletedDelegate IProgressListConsumerBase<TProgressList>.OnCompleted
    //    {
    //        add { onCompleted += value; }
    //        remove { onCompleted -= value; }
    //    }
    //    public event ProgressListConsumerCompletedDelegate OnCompleted
    //    {
    //        add { onCompleted += value; }
    //        remove { onCompleted -= value; }
    //    }
    //    #endregion Membri di IProgressListConsumerBase
    //}//public abstract class ProgressListConsumerBase

    public abstract class ProgressListConsumer2 : IProgressListConsumer<IProgressItemCollection>
    {
        public const String NotifyStartMethodName = "NotifyStart";
        public const String NotifyProgressMethodName = "NotifyProgress";
        public const String NotifyCompletedMethodName = "NotifyCompleted";

        public abstract String JobName { get; }

        protected virtual void InternalDoStarted(IProgressItemCollection info) { Started?.Invoke(this, info); }
        protected virtual void InternalDoProgress(IProgressItemCollection info, int index) { Progress?.Invoke(this, info, index); }
        protected virtual void InternalDoCompleted(IProgressItemCollection info) { Completed?.Invoke(this, info); }

        #region Membri di IProgressListConsumer<IProgressItemCollection>
        public void Notify(ProgressNotifyKind kind, IProgressItemCollection info, int index)
        {
            switch (kind)
            {
                case ProgressNotifyKind.Start:
                    InternalDoStarted(info);
                    break;
                case ProgressNotifyKind.Progress:
                    InternalDoProgress(info, index);
                    break;
                case ProgressNotifyKind.Completed:
                    InternalDoCompleted(info);
                    break;
                case ProgressNotifyKind.None:
                default:
                    Console.Write("Consumer notified {0} for {1} with index {2}.", kind, info, index);
                    break;
            }
        }

        public event ProgressListConsumerStartedDelegate Started;
        public event ProgressListConsumerProgressDelegate Progress;
        public event ProgressListConsumerCompletedDelegate Completed;
        #endregion Membri di IProgressListConsumer<IProgressItemCollection>
    }//public abstract class ProgressListConsumer2
}
