using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VdO2013THCore.Specialized;

namespace VdO2013SRCore.Specialized
{
    public interface ISiteReaderOnLineCheckList : IOnLineCheckList, ISiteReaderLinkedCallBack
    {
        ReaderMode Mode { get; }
    }// interface IOnLineCheckList
}
