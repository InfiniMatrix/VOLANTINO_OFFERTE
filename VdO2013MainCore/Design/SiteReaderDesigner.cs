using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using VdO2013Core;
using VdO2013Core.Config;
using VdO2013SRCore;

using MPExtensionMethods;
using MPLogHelper;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;

namespace VdO2013MainCore.Design
{
    public partial class SiteReaderDesigner : DevExpress.XtraEditors.XtraUserControl, ISiteReaderLinked, IClientModuleControl
    {
        public SiteReaderDesigner(ISiteReader3 reader)
        {
            InitializeComponent();
            SetReader(reader);
        }

        public SiteReaderDesigner() : this(null) { }

        #region ISiteReaderLinked Members
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

        public ISiteReader3 Reader
        {
            get { return grdSiteReaderProperties.SelectedObject as ISiteReader3; }
        }

        public ReaderChangedDelegate ReaderChanged { get; set; }
        public ReaderChangingDelegate ReaderChanging { get; set; }

#pragma warning disable IDE1006 // Naming Styles
        private void doReaderChanged()
#pragma warning restore IDE1006 // Naming Styles
        {
            ReaderChanged?.Invoke(new ReaderChangedEventArgs(grdSiteReaderProperties.SelectedObject as ISiteReader3));
        }
#pragma warning disable IDE1006 // Naming Styles
        private bool doReaderChanging(ISiteReader3 newReader)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (ReaderChanging == null) return true;
            var e = new ReaderChangingEventArgs(grdSiteReaderProperties.SelectedObject as ISiteReader3, newReader);
            ReaderChanging(e);
            return !e.Cancel;
        }

        #endregion

        public bool SetReader(ISiteReader3 reader)
        {
            if (doReaderChanging(reader))
            {
                try
                {
                    grdSiteReaderProperties.SelectedObject = reader;
                    cmbSiteReaderFeatures.SelectedIndexChanged -= cmbSiteReaderFeatures_SelectedIndexChanged;
                    cmbSiteReaderFeatures.Properties.Items.Clear();
                    if (Reader != null && Reader.Features != null)
                        cmbSiteReaderFeatures.Properties.Items.AddRange(Reader.Features.Cast<object>().ToArray());
                    cmbSiteReaderFeatures.SelectedIndexChanged += cmbSiteReaderFeatures_SelectedIndexChanged;
                    if (cmbSiteReaderFeatures.Properties.Items.Count > 0) cmbSiteReaderFeatures.SelectedIndex = 0;
                    doReaderChanged();
                    return true;
                }
                catch (Exception ex)
                {
                    var mf = Global.GetApplicationMainForm();
                    if (mf != null)
                        mf.ShowError(ex);
                    else
                        FileLog.Default.WriteError(ex);
                }
            }
            return false;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void cmbSiteReaderFeatures_SelectedIndexChanged(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            featureElementDesigner1.SetElement(cmbSiteReaderFeatures.SelectedItem as FeatureElement);
        }

        #region IClientModuleControl Members
        public RibbonControl MainRibbon { get; set; }
        public RibbonControl Ribbon => this.ctlRibbon;

        public RibbonPage RibbonPage => this.rpgSiteReader;

        public BarButtonItem RibbonCloseButton => this.bbiClose;

        public void MakeVisible(DockStyle dock = System.Windows.Forms.DockStyle.Fill, Control parent = null)
        {
            this.MergeRibbon(this.rpgSiteReader);
            if (parent != null)
                this.Parent = parent;
            this.Dock = dock;
            this.Visible = true;
            this.BringToFront();
        }

        public void MakeInvisible()
        {
            //this.SaveLayout();
            this.UnMergeRibbon(this.MainRibbon?.Pages[0]);
            this.Dock = DockStyle.None;
            this.SendToBack();
            this.Visible = false;
        }
        #endregion

        #region ICommandReceiver Members

        public string[] GetSupportedCommandNames()
        {
            return new string[] { };
        }

        public Type[] GetSupportedCommandArgsSimple(string commandName)
        {
            return new Type[] { };
        }

        public System.Reflection.ParameterInfo[] GetSupportedCommandArgs(string commandName)
        {
            return new System.Reflection.ParameterInfo[] { };
        }

        public int SendCommand(string commandName, object[] args, out Exception error)
        {
            error = new NotImplementedException("Command '{0}' is not implemented.".FormatWith(commandName));
            return 0;
        }
        public virtual object[] SendCommandNoArgs { get { return new object[] { }; } }

        #endregion

#pragma warning disable IDE1006 // Naming Styles
        private void bbiSiteReaderSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            this.Reader.Save();
            this.Reader.Reload();
        }
    }
}
