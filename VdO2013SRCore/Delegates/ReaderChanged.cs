using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public class ReaderChangedEventArgs : EventArgs
    {
        private ISiteReader2 _reader;
        public ReaderChangedEventArgs(ISiteReader2 reader)
        {
            _reader = reader;
        }
        public ISiteReader2 Reader { get { return _reader; } }
    }
    public delegate void ReaderChangedDelegate(ReaderChangedEventArgs e);
}
