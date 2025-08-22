using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using VdO2013SRCore;
using VdO2013WS;

namespace VdO2013SR
{
    public class SiteReader2Info : ISiteReader2Info
    {
        public Type Type { get; set; }
        public String ReaderName { get; set;  }
        public String ReaderVersion { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Image Logo { get; set; }
        public Image FullLogo { get; set; }
        
        //[Obsolete("Dovrebbe trovarsi solo in Global")]
        //public Boolean IsDemo { get { return CustomerMgr.VolantinoSuiteMainProductActivationPrivilege == ProductPrivilege.Demo; } }

        public override String ToString()
        {
            return String.Format("Type:{0}, Name:{1}, Version:{2}, Title:{3}, Description:{4}, Logo:{5}, FullLogo:{6}"
                , Type
                , ReaderName
                , ReaderVersion
                , Title
                , Description
                , Logo != null ? "Assigned" : "Empty"
                , FullLogo != null ? "Assigned" : "Empty");
        }

    }
}
