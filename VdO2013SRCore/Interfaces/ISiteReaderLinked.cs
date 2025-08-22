using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public interface ISiteReaderLinked
    {
        /// <summary>
        /// Loads the data when the reader is changed.
        /// </summary>
        bool RestoreDataOnReaderChanged { get; set; }
        /// <summary>
        /// Loads the layout when the reader is changed.
        /// </summary>
        bool RestoreLayoutOnReaderChanged { get; set; }

        ISiteReader3 Reader { get; }
        ReaderChangedDelegate ReaderChanged { get; set; }
        ReaderChangingDelegate ReaderChanging { get; set; }
    }// interface ISiteReaderLinked
}
