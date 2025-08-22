using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public class ReaderChangingEventArgs : ReaderChangedEventArgs
    {
        private ISiteReader2 _newReader;
        public ReaderChangingEventArgs(ISiteReader2 reader, ISiteReader2 newReader)
            : base(reader)
        {
            _newReader = newReader;
        }
        public ISiteReader2 NewReader { get { return _newReader; } }
        public Boolean Cancel { get; set; }
    }
    public delegate void ReaderChangingDelegate(ReaderChangingEventArgs e);
}
