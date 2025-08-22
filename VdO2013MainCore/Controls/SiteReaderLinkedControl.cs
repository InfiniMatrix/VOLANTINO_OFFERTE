using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using VdO2013Core;  
using VdO2013SRCore;

namespace VdO2013MainCore
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public partial class SiteReaderLinkedControl : DataLinkedControl, ISiteReaderLinked
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public SiteReaderLinkedControl() : base()
        {
            InitializeComponent();
        }

        #region Membri di ISiteReaderLinked
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        [DefaultValue(true)]
        public bool RestoreDataOnReaderChanged { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        [DefaultValue(true)]
        public bool RestoreLayoutOnReaderChanged { get; set; }

        private ISiteReader3 _reader;
        /// <summary>
        /// 
        /// </summary>
        public virtual ISiteReader3 Reader
        {
            get => this._reader;
            set
            {
                bool canChange = (this._reader == null || (this._reader != null && !this._reader.Equals(value)));

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
        protected virtual void InternalReaderChanging(ISiteReader3 newReader, out bool canChange)
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
