using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public interface ISiteReaderLinkedCallBack : ISiteReaderLinked
    {
        ReadListCallBack ReadListCallBack { get; }
        SaveListCallBack SaveListCallBack { get; }
    }
}
