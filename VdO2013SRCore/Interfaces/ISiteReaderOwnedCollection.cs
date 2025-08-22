using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPUtils;

namespace VdO2013SRCore
{
    public interface ISiteReaderOwnedCollectionItem : ISiteReaderLinked, IOwnedCollectionItem
    {
    }
    public interface ISiteReaderOwnedCollection<T> : ISiteReaderLinked, IOwnedCollection<T> where T : ISiteReaderOwnedCollectionItem
    {
    }
}
