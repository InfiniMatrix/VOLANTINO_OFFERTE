using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

namespace VdO2013SRCore
{
    public interface ISiteReader2Info
    {
        Type Type { get; }
        String ReaderName { get; }
        String ReaderVersion { get; }
        String Title { get; }
        String Description { get; }
        Image Logo { get; }
        Image FullLogo { get; }
        //[Obsolete("Dovrebbe trovarsi solo in Global")]
        //Boolean IsDemo { get; }
        String ToString();
    }

}
