using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using VdO2013Core;

namespace VdO2013TH
{
    public class ProgressInfoAddEventArgs : EventArgs
    {
        private IProgressItem _info;
        public ProgressInfoAddEventArgs(ProgressItem info)
        {
            _info = info;
        }

        public ProgressInfoAddEventArgs(IProgressItem info)
        {
            _info = info;
        }

        public IProgressItem Info { get { return _info; } }
    }

    public delegate void ProgressItemAddDelegate(ProgressInfoAddEventArgs arg);
}
