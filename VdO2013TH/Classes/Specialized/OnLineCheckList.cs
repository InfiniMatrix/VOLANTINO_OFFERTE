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
    public class OnLineCheckList : ProgressItemCollection, IOnLineCheckList
    {
        private Dictionary<Uri, OnLineStatus> _urls;
        public OnLineCheckList(String job, List<Uri> urls)
            : base(job)
        {
            _urls = new Dictionary<Uri, OnLineStatus>();
            foreach (var u in urls)
            {
                _urls.Add(u, OnLineStatus.None);
            }//foreach (var u in urls)
        }
        public OnLineCheckList(String job, Uri url)
            : this(job, new List<Uri>() { url })
        {
            ShowProgress = true;
        }

        public Dictionary<Uri, OnLineStatus> List { get { return _urls; } }
        public Boolean ShowProgress { get; set; }
    }
}
