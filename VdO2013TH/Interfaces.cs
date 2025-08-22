using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013THCore;
using VdO2013SRCore;

namespace VdO2013TH
{
    public interface IProgressItem
    {
        DateTime TimeStamp { get; }
        ProgressItemKind Kind { get; }
        ProgressItemStatus Status { get; set; }
        String Step { get; }
        String Description { get; }
        Double Percentage { get; }
        Boolean IsError { get; }
    }// interface IProgressInfo

    public interface IProgressItemCollection
    {
        String Job { get; set; }
        List<IProgressItem> Items { get; }
        int Count { get; }
        
        IProgressItem LastItem { get; }

        //IProgressItem LastInfo { get; }
        //IProgressItem LastError { get; }
        //IProgressItem LastWarning { get; }

        IProgressItem this[int index] { get; }
        IEnumerable<IProgressItem> Infos { get; }
        IEnumerable<IProgressItem> Errors { get; }
        IEnumerable<IProgressItem> Warnings { get; }
        Boolean Cancel { get; set; }

        int AddItem(ProgressItem info);

        int AddInfo(Double percentage, String description, String step = null);
        int AddInfo(String description, String step = null);

        int AddWarning(Double percentage, String description, String step = null);
        int AddWarning(String description, String step = null);

        int AddError(Exception error);
        int AddError(String message, Exception innerException = null);

        Double Percentage { get; }
        String Description { get; }
        String Step { get; }
        ProgressItemKind Kind { get; }
        ProgressItemStatus Status { get; }
    }// interface IProgressInfos

    public interface IProgressList<TKey, TValue> : IProgressItemCollection
    {
        Dictionary<TKey, TValue> List { get; }
        Boolean ShowProgress { get; set; }
    }// interface IProgressList

    public interface IProgressListConsumerBase
    {
        void Notify(ProgressNotifyKind kind, Object info, int index);
    }//public interface IProgressListConsumerBase

    public interface IProgressListConsumer<TProgressList> : IProgressListConsumerBase where TProgressList : IProgressItemCollection
    {
        void NotifyStart(TProgressList info);
        void NotifyProgress(TProgressList info, int index);
        void NotifyCompleted(TProgressList info);
    }//public interface IProgressListConsumer<TProgressList> where TProgressList : IProgressItemCollection

    public interface ISiteReaderProgressItemCollection : IProgressItemCollection, ISiteReaderLinked
    {

    }//public interface ISiteReaderProgressItemCollection : IProgressItemCollection, ISiteReaderLinked
}
