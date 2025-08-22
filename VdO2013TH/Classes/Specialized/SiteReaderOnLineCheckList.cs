using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;
using VdO2013THCore;
using VdO2013THCore.Specialized;

namespace VdO2013TH.Specialized
{
    public abstract class SiteReaderOnLineCheckList : OnLineCheckList, ISiteReaderOnLineCheckList
    {
        public SiteReaderOnLineCheckList(string job, ISiteReader3 reader, ReaderMode mode, ReadListCallBack readListCallBack, SaveListCallBack saveListCallBack)
            : base(job, new Uri(reader.StandardSiteRootUrl))
        {
            Reader = reader;
            Mode = mode;
            ReadListCallBack = readListCallBack;
            SaveListCallBack = saveListCallBack;
        }

        public SiteReaderOnLineCheckList(string job, ISiteReader3 reader, ReaderMode mode)
            : this(job, reader, mode, null, null) { }

        public ISiteReader3 Reader { get; }
        public ReaderMode Mode { get; }
        public ReadListCallBack ReadListCallBack { get; } = null;
        public SaveListCallBack SaveListCallBack { get; } = null;

        #region Membri di ISiteReaderLinked
        public abstract ReaderChangedDelegate ReaderChanged { get; set; }
        public abstract ReaderChangingDelegate ReaderChanging { get; set; }

        public bool RestoreDataOnReaderChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RestoreLayoutOnReaderChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion Membri di ISiteReaderLinked
    }
}
