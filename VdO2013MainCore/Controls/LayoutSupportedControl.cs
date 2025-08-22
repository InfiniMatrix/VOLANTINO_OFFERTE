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
using System.Runtime.InteropServices;
using DevExpress.Utils.Serializing;
using DevExpress.Utils.Design;

namespace VdO2013MainCore
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LayoutSupportedControl : XtraUserControl, IDevExpressLayoutSupported, IPropertyViewerSupport
    {
        protected readonly MPLogHelper.FileLog Logger;

        protected void ThrowForMethodToBeImplemented(string typeName, string methodName)
        {
            if (Global.Initialized)
                throw new NotImplementedException("Method {0}.{1}() must be overridden.".FormatWith(typeName, methodName));
        }
        protected void ThrowForPropertyToBeImplemented(string typeName, string propertyName)
        {
            if (Global.Initialized)
                throw new NotImplementedException("Property {0}.{1} must be overridden.".FormatWith(typeName, propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        public LayoutSupportedControl() : base()
        {
            InitializeComponent();
            Logger = new MPLogHelper.FileLog(this);

            this.KeyPreview = KeyPreviewKind.KeyDown;
            this.KeyDown += (o, e) => { if (e.Control && e.KeyCode == Keys.F10) { ToggleLayoutCustomization(!IsLayoutCustomizationActive); } };
            this.Load += (o, e) => { AddLayout(); };
        }

        //protected virtual SplitContainerControl GetSplitter() => this.ctlSplit;

        protected void DesignModeInvalidate() { if (this.DesignMode) this.Invalidate(); }
        protected void HideOriginalSplitter() { this.ctlSplit.Visible = false; DesignModeInvalidate(); }
        protected void ShowOriginalSplitter() { this.ctlSplit.Visible = true; DesignModeInvalidate(); }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [DXCategory("Layout")]
        public Boolean SplitterFixed { get => this.ctlSplit.IsSplitterFixed; set { this.ctlSplit.IsSplitterFixed = value; DesignModeInvalidate(); } }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [DXCategory("Layout")]
        [XtraSerializableProperty]
        public Boolean SplitterCollapsed { get => this.ctlSplit.Collapsed; set { this.ctlSplit.Collapsed = value; DesignModeInvalidate(); } }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(SplitPanelVisibility.Both)]
        [DXCategory("Layout")]
        [SmartTagProperty("Panel Visibility", "", SmartTagActionType.RefreshAfterExecute)]
        public SplitPanelVisibility SplitterVisibility { get => this.ctlSplit.PanelVisibility;  set { this.ctlSplit.PanelVisibility = value; DesignModeInvalidate(); } }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(40)]
        [DXCategory("Layout")]
        [SmartTagProperty("Panel Position", "", SmartTagActionType.RefreshAfterExecute)]
        public int SplitterPosition { get => this.ctlSplit.SplitterPosition; set { this.ctlSplit.SplitterPosition = value; DesignModeInvalidate(); } }
        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DXCategory("Layout")]
        [SmartTagProperty("FixedPane", "")]
        public SplitGroupPanel SplitterFixedPane { get => this.ctlSplit.Panel1; }
        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DXCategory("Layout")]
        [SmartTagProperty("CustomPane", "")]
        public SplitGroupPanel SplitterCustomPane { get => this.ctlSplit.Panel2; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [DXCategory("Layout")]
        [SmartTagProperty("Splitter direction", "", SmartTagActionType.RefreshAfterExecute)]
        public bool SplitterHorizontal { get => this.ctlSplit.Horizontal; set { this.ctlSplit.Horizontal = value; DesignModeInvalidate(); } }

        ///// <summary>
        ///// 
        ///// </summary>
        //[DefaultValue(true)]
        //[DXCategory("Layout")]
        //[SmartTagProperty("Splitter control visibile", "", SmartTagActionType.RefreshAfterExecute)]
        //public bool SplittedControlVisible { get => this.ctlSplit.Visible; set { this.ctlSplit.Visible = value; DesignModeInvalidate(); } }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(SplitCollapsePanel.Panel2)]
        [DXCategory("Layout")]
        [SmartTagProperty("Splitter control visibile", "", SmartTagActionType.RefreshAfterExecute)]
        public SplitCollapsePanel SplitterCollapseMode { get => this.ctlSplit.CollapsePanel; set { this.ctlSplit.CollapsePanel = value; DesignModeInvalidate(); } }

        #region Membri di IDevExpressLayoutSupported
        /// <summary>
        /// Retrieves the Xml file for the layout of the specified reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public virtual string GetLayoutName(ISupportXtraSerializer reference) { ThrowForMethodToBeImplemented(typeof(LayoutSupportedControl).Name, nameof(GetLayoutName)); return null; }
        ///// <summary>
        ///// Adds the reference to the LayoutSupport system
        ///// </summary>
        ///// <param name="reference"></param>
        ///// <returns></returns>
        //private LayoutSupportReferenceBase AddLayout(object reference, bool loadAlways = false, bool saveAlways = true)
        //{
        //    if (reference == null) return null;
        //    if (!LayoutCentral.Default.ContainsReference(reference))
        //    {
        //        return LayoutCentral.Default.AddXmlFile(reference, GetLayoutName, loadAlways, saveAlways);
        //    }
        //    return null;
        //}

        protected int AddLayout()
        {
            var layoutReferences = GetLayoutReferences();
            if (layoutReferences == null) return -1;
            var count = 0;
            foreach (var layoutReference in layoutReferences)
            {
                if (layoutReference != null && !LayoutCentral.Default.ContainsReference(layoutReference))
                {
                    Logger.WriteDebug("Register Xml layout support for {0}.", layoutReference);
                    LayoutCentral.Default.AddXmlFile(layoutReference, GetLayoutName);
                    count++;
                }
            }
            return count;
        }
        public virtual IEnumerable<ISupportXtraSerializer> GetLayoutReferences() { ThrowForMethodToBeImplemented(typeof(LayoutSupportedControl).Name, nameof(GetLayoutReferences)); return null; }

        /// <summary>
        /// Retrieves the LayoutSupport for the specified reference
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public LayoutSupportReferenceBase GetLayout(ISupportXtraSerializer reference)
        {
            return LayoutCentral.Default.FindByReference(reference);
        }
        /// <summary>
        /// Saves the layout using <see cref="GetLayoutSupport"/>.
        /// </summary>
        /// <returns>True if all layout references are saved.</returns>
        public virtual bool SaveLayout()
        {
            var layoutReferences = GetLayoutReferences();
            if (layoutReferences == null) return false;
            var saved = new List<object>();
            foreach (var layoutReference in layoutReferences)
            {
                if (!saved.Contains(layoutReference) && LayoutCentral.Default.Save(LayoutCentral.Default.FindByReference(layoutReference)))
                    saved.Add(layoutReference);
            }
            return layoutReferences.Count() == saved.Count;
        }
        /// <summary>
        /// Restores the layout using <see cref="GetLayoutSupport"/>.
        /// </summary>
        /// <returns>True if all layout references are restored.</returns>
        public virtual bool LoadLayout()
        {
            var layoutReferences = GetLayoutReferences();
            if (layoutReferences == null) return false;
            var loaded = new List<object>();
            foreach (var layoutReference in layoutReferences)
            {
                if (!loaded.Contains(layoutReference) && layoutReference != null && LayoutCentral.Default.Load(LayoutCentral.Default.FindByReference(layoutReference)))
                    loaded.Add(layoutReference);
            }
            return layoutReferences.Count() == loaded.Count;
        }
        #endregion Membri di IDevExpressLayoutSupported

        #region KeyPreview Trapping
        //----------------------------------------------
        // Define the PeekMessage API call
        //----------------------------------------------

        private struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum KeyPreviewKind
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// 
            /// </summary>
            KeyDown = 1,
            /// <summary>
            /// 
            /// </summary>
            KeyUp = 2,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="hwnd"></param>
        /// <param name="msgMin"></param>
        /// <param name="msgMax"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Boolean PeekMessage([In, Out] ref MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(KeyPreviewKind.None)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public KeyPreviewKind KeyPreview = KeyPreviewKind.None;

        //----------------------------------------------
        // <span class="code-SummaryComment"><summary> </span>
        // Trap any keypress before child controls get hold of them
        // <span class="code-SummaryComment"></summary></span>
        // <span class="code-SummaryComment"><param name="m">Windows message</param></span>
        // <span class="code-SummaryComment"><returns>True if the keypress is handled</returns></span>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Boolean ProcessKeyPreview(ref Message m)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_SYSKEYUP = 0x105;
            const int WM_IME_CHAR = 0x286;

            KeyEventArgs e = null;

            if ((m.Msg != WM_CHAR) && (m.Msg != WM_SYSCHAR) && (m.Msg != WM_IME_CHAR))
            {
                e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | ModifierKeys);
                switch (m.Msg)
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        if (KeyPreview.HasFlag(KeyPreviewKind.KeyDown)) TrappedKeyDown(e);
                        break;
                    case WM_KEYUP:
                    case WM_SYSKEYUP:
                        if (KeyPreview.HasFlag(KeyPreviewKind.KeyUp)) TrappedKeyUp(e);
                        break;
                    default:
                        break;
                }

                // Remove any WM_CHAR type messages if supresskeypress is true.
                if (e.SuppressKeyPress)
                {
                    this.RemovePendingMessages(WM_CHAR, WM_CHAR);
                    this.RemovePendingMessages(WM_SYSCHAR, WM_SYSCHAR);
                    this.RemovePendingMessages(WM_IME_CHAR, WM_IME_CHAR);
                }

                if (e.Handled)
                {
                    return e.Handled;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        private void RemovePendingMessages(int msgMin, int msgMax)
        {
            if (!this.IsDisposed)
            {
                MSG msg = new MSG();
                IntPtr handle = this.Handle;
                while (PeekMessage(ref msg, new HandleRef(this, handle), msgMin, msgMax, 1)) { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        new public event KeyEventHandler KeyDown;
        /// <summary>
        /// 
        /// </summary>
        new public event KeyEventHandler KeyUp;

        // <span class="code-SummaryComment"><summary></span>
        // This routine gets called if a keydown has been trapped 
        // before a child control can get it.
        // <span class="code-SummaryComment"></summary></span>
        // <span class="code-SummaryComment"><param name="e"></param></span>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void TrappedKeyDown(KeyEventArgs e)
        {
            if (KeyDown != null)
                this.KeyDown(this, e);
        }

        // <span class="code-SummaryComment"><summary></span>
        // This routine gets called if a keyup has been trapped 
        // before a child control can get it.
        // <span class="code-SummaryComment"></summary></span>
        // <span class="code-SummaryComment"><param name="e"></param></span>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void TrappedKeyUp(KeyEventArgs e)
        {
            if (this.KeyUp != null)
                KeyUp(this, e);
        }
        #endregion

        protected virtual DevExpress.XtraLayout.LayoutControl GetLayoutControl() { ThrowForMethodToBeImplemented(typeof(LayoutSupportedControl).Name, nameof(GetLayoutControl)); return null; }

        public bool IsLayoutCustomizationActive { get { var c = GetLayoutControl(); return c != null && c.CustomizationForm.Visible; } }
        public bool ToggleLayoutCustomization(bool active)
        {
            var c = GetLayoutControl();
            if (c == null || active == IsLayoutCustomizationActive) return false;
            if (active)
            {
                c.ShowCustomizationForm();
                c.CustomizationForm.FormClosed += (s, e) => { this.SaveLayout(); };
            }
            else
                c.HideCustomizationForm();

            return true;
        }

        public virtual InvokePropertyViewerDelegate InvokePropertyViewer()
        {
            return null;
        }
    }
}
