using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPUtils;

namespace VdO2013SRCore
{
    public class SiteReaderOwnedCollectionItem : OwnedCollectionItem, ISiteReaderOwnedCollectionItem
    {
        public SiteReaderOwnedCollectionItem()
        {
        }

        private bool InternalGetCollection(out ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> collection)
        {
            collection = null;
            try
            {
                collection = this.Collection as ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem>;
                return collection != null;
            }
#if DEBUG
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
#else
            catch
            {

#endif
            }
            return false;
        }

        #region ISiteReaderLinked Members
        public ISiteReader3 Reader => InternalGetCollection(out ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> c) ? c.Reader : null;

        public ReaderChangedDelegate ReaderChanged
        {
            get => InternalGetCollection(out ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> c) ? c.ReaderChanged : null;
            set
            {
                if (InternalGetCollection(out ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> c)) { c.ReaderChanged = value; };
            }
        }
        public ReaderChangingDelegate ReaderChanging
        {
            get => InternalGetCollection(out ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> c) ? c.ReaderChanging : null;
            set
            {
                if (InternalGetCollection(out ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> c)) { c.ReaderChanging = value; };
            }
        }

        public bool RestoreDataOnReaderChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RestoreLayoutOnReaderChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
    }

    public class SiteReaderOwnedCollection<T> : OwnedCollection<T>, ISiteReaderOwnedCollection<T> where T : ISiteReaderOwnedCollectionItem
    {
        public ISiteReader3 Reader { get; } = null;

        public SiteReaderOwnedCollection(ISiteReader3 reader)
        {
            Reader = reader;
        }

        #region ISiteReaderLinked Members
        private event ReaderChangedDelegate _readerChanged;
        public ReaderChangedDelegate ReaderChanged
        {
            get => _readerChanged;
            set => _readerChanged = value;
        }

        private event ReaderChangingDelegate _readerChanging;
        public ReaderChangingDelegate ReaderChanging
        {
            get => _readerChanging;
            set => _readerChanging = value;
        }
        #endregion

        #region IEnumerable Members
        public new System.Collections.IEnumerator GetEnumerator()
        {
            return BaseGetEnumerator();
        }
        #endregion

        #region IList Members
        public int Add(object value)
        {
            return ((IList)this).Add(value);
        }
        public bool Contains(object value)
        {
            return ((IList)this).Contains(value);
        }

        public int IndexOf(object value)
        {
            return ((IList)this).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)this).Insert(index, value);
        }
        public bool IsFixedSize
        {
            get { return BaseGetIsFixedSize(); }
        }

        public bool RestoreDataOnReaderChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RestoreLayoutOnReaderChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Remove(object value)
        {
            ((IList)this).Remove(value);
        }
        public new object this[int index]
        {
            get
            {
                return ((IList)this)[index];
            }
            set
            {
                ((IList)this)[index] = value;
            }
        }
        #endregion
    }
}
