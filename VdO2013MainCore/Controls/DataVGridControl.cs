using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPLogHelper;
using VdO2013Data;
using VdO2013Core;
using VdO2013SRCore;

using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Columns;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraVerticalGrid.ViewInfo;
using DevExpress.Utils.Menu;

namespace VdO2013MainCore.Controls
{
    public partial class DataVGridControl : SiteReaderLinkedControl, IDataGridControl
    {
        private BaseRow _checkColumn = null;
        public DataVGridControl()
        {
            InitializeComponent();

            this.Controls.Remove(this.ctlGrid);
            this.Controls.Remove(this.ctlReaderInfos);

            this.SplitterFixedPane.Controls.Add(this.ctlGrid);
            this.SplitterFixedPane.Controls.Add(this.ctlReaderInfos);

            ctlGrid.PopupMenuShowing += (_s, _e) =>
            {
                var m = new DXMenuItem()
                {
                    Caption = "Proprietà vista",
                    Tag = _e.Row,
                    BeginGroup = true,
                };
                m.Click += (__s, __e) =>
                {
                    InvokePropertyViewer()?.Invoke(__s, new InvokePropertyViewerEventArgs(ctlGrid));
                };
                _e.Menu.Items.Add(m);
            };
        }

        /// <summary>
        /// Returns the count of checked rows.
        /// Remarks: the column value must be a Boolean
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected static int GetCheckedCount(BaseRow column)
        {
            int count = 0;
            var view = column.Grid;
            if (view == null) return count;
            var info = view.ViewInfo;

            for (int i = 0; i < view.RecordCount; i++)
            {
                var v = view.GetCellValue(column, i);
                if (!DBNull.Value.Equals(v) && v != null && (bool)v == true)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Updates the InfoPane
        /// </summary>
        /// <param name="checkColumn"></param>
        protected virtual void doUpdateReaderInfoPane(BaseRow checkColumn = null)
        {
            if (checkColumn == null) checkColumn = _checkColumn;
            try
            {
                this.ctlReaderInfos.Reader = this.Reader;
                var count = PrivateCheckGridMainView() ? MainDataLink.GridMainView.RowCount : -1;
                if (checkColumn != null)
                {
                    this.ctlReaderInfos.Elementi = "{0}/{1}".FormatWith(GetCheckedCount(checkColumn), count);
                }
                else
                    this.ctlReaderInfos.Elementi = "{0}".FormatWith(count);

                if (this.IsDataFiltered)
                    this.ctlReaderInfos.Elementi += @"*";
            }
            finally
            {
                _checkColumn = checkColumn;
            }
        }

        private bool PrivateCheckGridMainView()
        {
            return MainDataLink != null && MainDataLink.GridMainView != null;
        }

        /// <summary>
        /// Gets or sets whether the filtering functionality is enabled.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        public override bool IsDataFiltered
        {
            get
            {
                if (!PrivateCheckGridMainView()) return false;
                return !string.IsNullOrEmpty(MainDataLink.GridMainView.ActiveFilterString) && MainDataLink.GridMainView.ActiveFilterEnabled;
            }
            set
            {
                if (!PrivateCheckGridMainView()) return;
                MainDataLink.GridMainView.ActiveFilterEnabled = value && !string.IsNullOrEmpty(MainDataLink.GridMainView.ActiveFilterString);
                InternalDataLinkChanged(MainDataLink);
                doUpdateReaderInfoPane();
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        public bool IsDragDropEnabled
        {
            get => false;
            set
            {
            }
        }

        [Browsable(false)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        [DefaultValue("")]
        public override string DataFilterString
        {
            get
            {
                if (!PrivateCheckGridMainView()) return string.Empty;
                return MainDataLink.GridMainView.ActiveFilterEnabled ? MainDataLink.GridMainView.ActiveFilterString : string.Empty;
            }
            set
            {
                if (!PrivateCheckGridMainView()) return;
                MainDataLink.GridMainView.ActiveFilterString = value;
            }
        }

        public event CancelEventHandler IsDragDropEnabledChanging;
        public event EventHandler IsDragDropEnabledChanged;

        public event RowsDroppingEventHandler RowsDropping;
        public event RowDroppingEventHandler RowDropping;
        public event RowDroppedEventHandler RowDropped;
        public event RowsDroppedEventHandler RowsDropped;

        [Browsable(false)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        [DefaultValue("")]
        public override string DataFilterDisplayText
        {
            get
            {
                if (!PrivateCheckGridMainView()) return string.Empty;
                return MainDataLink.GridMainView.GetFilterDisplayText(MainDataLink.GridMainView.ActiveFilter);
            }
        }

        /// <summary>
        /// Executes after the Reader has changed
        /// </summary>
        protected override void InternalReaderChanged()
        {
            base.InternalReaderChanged();
            InternalDataLinkChanged(MainDataLink);
            doUpdateReaderInfoPane();
        }

        #region IDataGridControl Members
        public BaseRow CheckColumn
        {
            get { throw new NotImplementedException(); }
        }

        public void GridReset(bool removeDataLink = false)
        {
            throw new NotImplementedException();
            //if (DataLink.Count > 0)
            //{
            //    if (removeDataLink)
            //    {
            //        MainDataLink.Clear();
            //    }
            //    MainDataLink.GridMainView.Columns.Clear();
            //    MainDataLink.Grid.RepositoryItems.Clear();
            //    MainDataLink.GridMainView.RefreshData();
            //    doDataLinkChanged(MainDataLink);
            //}
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public SiteReaderLinkedInfoControl InfoPane { get { return this.ctlReaderInfos; } }

        [DefaultValue(true)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public bool InfoPaneVisible { get { return this.InfoPane.Visible; } set { this.InfoPane.Visible = value; this.Update(); } }

        public DevExpress.XtraGrid.Views.Grid.GridView MainDataLinkGridView
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// DevExpress navigator linked to the Grid
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public ControlNavigator MainDataLinkNavigator { get { return MainDataLink?.Navigator; } }
        #endregion
    }
}
