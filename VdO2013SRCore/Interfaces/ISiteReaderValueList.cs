using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public interface ISiteReaderValueList<T> : IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        List<T> List { get; }
    }
}
