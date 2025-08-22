using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DevExpress.XtraBars.Ribbon.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;

using VdO2013Core;
using VdO2013MainCore;
using VdO2013DataCore;
using System.Linq;
using System.Reflection;

namespace VdO2013MainCore.Controls.Ribbon
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ApplicationBackstageBase : LayoutSupportedControl, IApplicationBackstage
    {
        //protected readonly MPLogHelper.FileLog Logger;

        /// <summary>
        /// 
        /// </summary>
        public ApplicationBackstageBase()
        {
            InitializeComponent();
            //Logger = new MPLogHelper.FileLog(this);

            //this.Load += (o, e) => { AddLayout(); };
            HideOriginalSplitter();
        }

        /// <summary>
        /// 
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return GetBackgroundColor();
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Color GetBackgroundColor()
        {
            BackstageViewClientControl parent = Parent as BackstageViewClientControl;
            if (parent == null)
                return Color.Transparent;
            return parent.GetBackgroundColor();
        }

        /// <summary>
        /// 
        /// </summary>
        public BackstageViewControl BackstageView
        {
            get
            {
                if (Parent == null)
                    return null;
                return Parent.Parent as BackstageViewControl;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BackstageView != null)
                BackstageViewPainter.DrawBackstageViewImage(e, this, BackstageView);
        }

        #region Membri di IApplicationBackstage

        /// <summary>
        /// Removes all data from control
        /// </summary>
        /// <returns></returns>
        public virtual bool ClearData() { ThrowForMethodToBeImplemented(typeof(ApplicationBackstageBase).Name, nameof(ClearData)); return false; }
        /// <summary>
        /// Load data in control
        /// </summary>
        /// <returns></returns>
        public virtual bool LoadData() { ThrowForMethodToBeImplemented(typeof(ApplicationBackstageBase).Name, nameof(LoadData)); return false; }
        /// <summary>
        /// Save data in control
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveData() { ThrowForMethodToBeImplemented(typeof(ApplicationBackstageBase).Name, nameof(SaveData)); return false; }

        public bool LoadDataAndLayout()
        {
            var mi = MethodBase.GetCurrentMethod();
            Logger.WriteMethod(mi);
            var result = false;

            result = LoadData();
            Logger.WriteDebug("::{0}={1}", nameof(LoadData), result);
            result = LoadLayout();
            Logger.WriteDebug("::{0}={1}", nameof(LoadLayout), result);

            Logger.WriteMethodResult(mi, result);
            return result;
        }
        public bool SaveDataAndLayout()
        {
            var mi = MethodBase.GetCurrentMethod();
            Logger.WriteMethod(mi);
            var result = false;

            result = SaveData();
            Logger.WriteDebug("::{0}={1}", nameof(SaveData), result);
            result = SaveLayout();
            Logger.WriteDebug("::{0}={1}", nameof(SaveLayout), result);

            Logger.WriteMethodResult(mi, result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual LayoutControl LayoutControl { get { ThrowForPropertyToBeImplemented(typeof(ApplicationBackstageBase).Name, nameof(LayoutControl)); return null; } }
        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual LayoutGroup RootGroup { get { ThrowForPropertyToBeImplemented(typeof(ApplicationBackstageBase).Name, nameof(RootGroup)); return null; } }

        public virtual bool AllowAppendData { get => true; }
        public virtual bool AllowUpdateData { get => true; }
        public virtual bool AllowDeleteData { get => true; }

        public virtual bool IsDataFiltered { get; } = false;

        public virtual string DataFilterString { get; } = null;

        public virtual string DataFilterDisplayText { get; } = null;

        public IBindingSourceChanges AttachData(BindingSource source) { BindingSourceCentral.Default.Add(source); return BindingSourceCentral.Default.Find(source); }
        public bool DetachData(BindingSource source) { var changes = BindingSourceCentral.Default.Find(source); if (changes == null) return false; return BindingSourceCentral.Default.Remove(source); }
        public bool? HasChangedData(BindingSource source) { var changes = BindingSourceCentral.Default.Find(source); return changes == null ? null : BindingSourceCentral.Default.HasChanges(source); }

        #endregion Membri di IApplicationBackstage
    }
}
