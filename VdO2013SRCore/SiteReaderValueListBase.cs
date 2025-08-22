using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013SRCore
{
    public abstract class SiteReaderValueListBase<T> : ISiteReaderValueList<T>
    {
        protected readonly List<T> _List = new List<T>();
        protected abstract int Populate();

        public IEnumerator<T> GetEnumerator()
        {
            return _List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _List.GetEnumerator();
        }

        public List<T> List { get { if (_List.Count == 0) Populate(); return _List; } }
        public Int32 Count { get { if (_List.Count == 0) Populate(); return _List.Count; } }

        public T this[int index] { get => _List[index]; }
    }
}
