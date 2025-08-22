using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;

using VdO2013Core;
using VdO2013Data;
using VdO2013DataCore;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;
using VdO2013THCore;
using VdO2013THCore.Specialized;

namespace VdO2013TH.Specialized
{
    public class SiteReaderOnLineCheckDataList : SiteReaderOnLineCheckList, ISiteReaderOnLineCheckDataList
    {
        public SiteReaderOnLineCheckDataList(string job, ISiteReader3 reader, ReaderMode mode, ReadListCallBack readListCallBack, SaveListCallBack saveListCallBack, IVDataView reportData = null, IVDataView resultData = null, IVDataView detailsData = null)
            : base(job, reader, mode, readListCallBack, saveListCallBack)
        {
            DataInfo = new DataInfo(Global.ConnectionString, reportData, resultData, detailsData);
            DataInfo.OnError += AddError;
        }
        public SiteReaderOnLineCheckDataList(string job, ISiteReader3 reader, ReaderMode mode, IVDataView reportData = null, IVDataView resultData = null, IVDataView detailsData = null)
            : this(job, reader, mode, null, null, reportData, resultData, detailsData) { }

        public DataInfo DataInfo { get; }

        public override ReaderChangedDelegate ReaderChanged { get => null; set { } }
        public override ReaderChangingDelegate ReaderChanging { get => null; set { } }
    }
}
