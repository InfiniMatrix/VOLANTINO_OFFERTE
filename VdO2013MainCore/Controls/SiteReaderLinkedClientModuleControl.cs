using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdO2013SRCore;

namespace VdO2013MainCore.Controls
{
    public abstract class SiteReaderLinkedClientModuleControl : ClientModuleControl, ISiteReaderLinked
    {
        public virtual bool LoadData() => false;
        public virtual bool LoadLayout() => false;

        #region Membri di ISiteReaderLinked
        /// <inheritdoc />
        /// <summary>
        /// Loads the data when the reader is changed.
        /// </summary>
        [DefaultValue(true)]
        public bool RestoreDataOnReaderChanged { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Loads the layout when the reader is changed.
        /// </summary>
        [DefaultValue(true)]
        public bool RestoreLayoutOnReaderChanged { get; set; }

        private ISiteReader3 _reader;
        /// <summary>
        /// 
        /// </summary>
        public virtual ISiteReader3 Reader
        {
            get { return this._reader; }
            set
            {
                Boolean canChange = (this._reader == null || (this._reader != null && !this._reader.Equals(value)));

                if (canChange)
                    InternalReaderChanging(value, out canChange);

                if (canChange)
                {
                    this._reader = value;
                    InternalReaderChanged();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newReader"></param>
        /// <param name="canChange"></param>
        protected virtual void InternalReaderChanging(ISiteReader3 newReader, out Boolean canChange)
        {
            canChange = true;
            if (ReaderChanging != null)
            {
                var e = new ReaderChangingEventArgs(_reader, newReader) { Cancel = false };
                ReaderChanging(e);
                canChange = !e.Cancel;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void InternalReaderChanged()
        {
            ReaderChanged?.Invoke(new ReaderChangedEventArgs(_reader));

            if (RestoreDataOnReaderChanged) LoadData();
            if (RestoreLayoutOnReaderChanged) LoadLayout();
        }
        /// <summary>
        /// 
        /// </summary>
        public ReaderChangedDelegate ReaderChanged { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ReaderChangingDelegate ReaderChanging { get; set; }
        #endregion
    }
}
