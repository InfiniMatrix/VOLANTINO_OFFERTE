using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using VdO2013Core;
using VdO2013THCore;

namespace VdO2013SRCore
{
    public delegate void ReadListCallBack(BackgroundWorker worker, Specialized.ISiteReaderOnLineCheckDataList info);
    public delegate void SaveListCallBack(BackgroundWorker worker, Specialized.ISiteReaderOnLineCheckDataList info);
}
