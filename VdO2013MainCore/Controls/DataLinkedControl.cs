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
using VdO2013Data;

using MPLogHelper;
using VdO2013DataCore;
using DevExpress.XtraLayout;
using DevExpress.Utils.Serializing;

namespace VdO2013MainCore
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataLinkedControl : LayoutSupportedControl, IDataLinkedControl
    {
        /// <summary>
        /// 
        /// </summary>
        public DataLinkedControl() : base()
        {
            InitializeComponent();
        }

        public override IEnumerable<ISupportXtraSerializer> GetLayoutReferences()
        {
            return new ISupportXtraSerializer[] { };
        }

        public override string GetLayoutName(ISupportXtraSerializer reference)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataLinkName"></param>
        /// <returns></returns>
        public virtual Boolean DeleteRows(String dataLinkName)
        {
            if (this.DataLink.Count == 0) return true;
            var v = this.DataLink[dataLinkName].DataCache;
            if (v == null) return false;
            //var r = v == null ? false : v.DeleteAll(out error) == 0;
            var r = v.Definition.DeleteAll(v, out Exception error);
            Logger.WriteError(error);
            InternalDataLinkChanged(this.DataLink[dataLinkName]);
            return r;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="columnName"></param>
        ///// <returns></returns>
        //public Object[] GetColumnValueList(String columnName) { return Data.GetColumnValues(columnName); }
        //#endregion Membri di IVDataViewLinked

        #region Membri di IDataLinkedControl
        private VDataViewGridLinkCollection _dataLink = new VDataViewGridLinkCollection();
        
        /// <summary>
        /// 
        /// </summary>
        [Category("Dati")]
        [DXCategory("Dati")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[EditorAttribute(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [EditorAttribute(typeof(DataLinkCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VDataViewGridLinkCollection DataLink
        {
            get { return _dataLink; }
        }
        public GridVDataViewLinkChanged DataLinkChanged { get; set; }

        protected virtual void InternalDataLinkChanged(VDataViewGridLink link)
        {
            if (link == null) return;
            DataLinkChanged?.Invoke(link);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Boolean ClearData()
        {
            if (this.DataLink.Count == 0) return true;
            this.DataLink.Clear();
            return this.DataLink.Count == 0;
        }

        /// <summary>             
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Boolean LoadData()
        {
            if (this.DataLink.Count == 0) return false;
            var i = 0;
            foreach (var bsn in this.DataLink.Names)
            {
                if (LoadData(bsn)) i++;
            }// foreach (var bsn in this.DataLink.Names)
            return i == this.DataLink.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataLinkName"></param>
        /// <returns></returns>
        public virtual Boolean LoadData(String dataLinkName)
        {
            if (this.DataLink.Count == 0) return false;
            //var v = this.DataLink[dataLinkName].DataCache;
            if (this.DataLink[dataLinkName] == null || this.DataLink[dataLinkName].Definition == null) return false;
            this.DataLink[dataLinkName].Binding.DataSource = this.DataLink[dataLinkName].Definition.GetData(out Exception error);
            Logger.WriteError(error);
            InternalDataLinkChanged(this.DataLink[dataLinkName]);
            return this.DataLink[dataLinkName].DataCache != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Boolean SaveData()
        {
            if (this.DataLink.Count == 0) return false;
            var i = 0;
            foreach (var bsn in this.DataLink.Names)
            {
                if (SaveData(bsn)) i++;
            }// foreach (var bsn in this.DataLink.Names)
            return i == this.DataLink.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataLinkName"></param>
        /// <returns></returns>
        public virtual Boolean SaveData(String dataLinkName)
        {
            if (this.DataLink.Count == 0) return false;
            if (this.DataLink[dataLinkName] == null || this.DataLink[dataLinkName].Definition == null || this.DataLink[dataLinkName].DataCache == null) return false;
            var r = this.DataLink[dataLinkName].Definition.SetData(this.DataLink[dataLinkName].DataCache, out Exception error);
            Logger.WriteError(error);
            return r;
        }

        public bool LoadDataAndLayout() => LoadData() && LoadLayout();
        public bool SaveDataAndLayout() => SaveData() && SaveLayout();

        public IBindingSourceChanges AttachData(BindingSource source) { BindingSourceCentral.Default.Add(source); return BindingSourceCentral.Default.Find(source); }
        public bool DetachData(BindingSource source) { var changes = BindingSourceCentral.Default.Find(source); if (changes == null) return false; return BindingSourceCentral.Default.Remove(source); }
        public bool? HasChangedData(BindingSource source) { var changes = BindingSourceCentral.Default.Find(source); return changes == null ? null : BindingSourceCentral.Default.HasChanges(source); }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VDataViewGridLink MainDataLink { get { return DataLink.Count > 0 ? DataLink[0] : null; } }

        public virtual bool AllowAppendData { get => true; }
        public virtual bool AllowUpdateData { get => true; }
        public virtual bool AllowDeleteData { get => true; }

        /// <summary>
        /// Gets or sets whether the filtering functionality is enabled.
        /// </summary>
        [DefaultValue(true)]
        [DXCategory("Behavior")]
        public virtual Boolean IsDataFiltered
        {
            get { return false; }
            set { }
        }

        [Browsable(false)]
        [DefaultValue("")]
        public virtual String DataFilterString
        { 
            get { return String.Empty; }
            set { }
        }

        [Browsable(false)]
        [DefaultValue("")]
        public virtual String DataFilterDisplayText { get { return String.Empty; } }
        #endregion Membri di IDataLinkedControl

        protected override LayoutControl GetLayoutControl() => null;
    }
}
